"""MQTT helpers: envelope, seq, status/LWT, publish/subscribe."""

from __future__ import annotations

import json
import threading
from datetime import datetime, timezone
from typing import Any, Callable

import paho.mqtt.client as mqtt


def utc_now_iso() -> str:
    return datetime.now(timezone.utc).isoformat(timespec="milliseconds").replace("+00:00", "Z")


class TwinMqtt:
    def __init__(
        self,
        host: str = "localhost",
        port: int = 1883,
        source: str = "sim/python",
        client_id: str = "inc492-sim",
    ) -> None:
        self.host = host
        self.port = port
        self.source = source
        self._seq = 0
        self._lock = threading.Lock()
        self._on_command: Callable[[dict], None] | None = None

        self.client = mqtt.Client(
            mqtt.CallbackAPIVersion.VERSION2,
            client_id=client_id,
            protocol=mqtt.MQTTv311,
        )
        self.client.on_connect = self._on_connect
        self.client.on_message = self._on_message

        # Last Will: if we disconnect uncleanly, status becomes offline.
        offline = self.envelope({"state": "offline"})
        self.client.will_set(
            "twin/status/sim",
            json.dumps(offline),
            qos=1,
            retain=True,
        )

    def set_command_handler(self, handler: Callable[[dict], None]) -> None:
        self._on_command = handler

    def connect(self) -> None:
        self.client.connect(self.host, self.port, keepalive=30)
        self.client.loop_start()

    def disconnect(self) -> None:
        online = False
        try:
            self.publish_status("offline")
            online = True
        finally:
            self.client.loop_stop()
            self.client.disconnect()
            _ = online

    def envelope(self, payload: dict[str, Any]) -> dict[str, Any]:
        with self._lock:
            self._seq += 1
            seq = self._seq
        return {
            "schema_version": "1.0",
            "source": self.source,
            "timestamp_utc": utc_now_iso(),
            "seq": seq,
            "payload": payload,
        }

    def publish_json(self, topic: str, payload: dict[str, Any], qos: int = 0, retain: bool = False) -> None:
        body = self.envelope(payload)
        self.client.publish(topic, json.dumps(body), qos=qos, retain=retain)

    def publish_status(self, state: str) -> None:
        self.publish_json("twin/status/sim", {"state": state}, qos=1, retain=True)

    def publish_telemetry(self, payload: dict[str, Any]) -> None:
        self.publish_json("twin/telemetry/tank", payload, qos=0, retain=False)

    def publish_event(self, payload: dict[str, Any]) -> None:
        self.publish_json("twin/event", payload, qos=1, retain=True)

    def _on_connect(self, client, userdata, flags, reason_code, properties=None) -> None:
        if reason_code != 0 and getattr(reason_code, "value", reason_code) != 0:
            print(f"[mqtt] connect failed: {reason_code}")
            return
        client.subscribe("twin/command/controller", qos=1)
        self.publish_status("online")
        print(f"[mqtt] connected to {self.host}:{self.port}, subscribed twin/command/controller")

    def _on_message(self, client, userdata, msg) -> None:
        if msg.topic != "twin/command/controller":
            return
        try:
            data = json.loads(msg.payload.decode("utf-8"))
        except json.JSONDecodeError:
            self.publish_event(
                {
                    "command_id": None,
                    "result": "rejected",
                    "reason": "payload is not valid JSON",
                }
            )
            return
        if self._on_command:
            self._on_command(data)
