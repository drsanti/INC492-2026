"""Lab 02 — Coupled-tank simulator entry point.

Requires Lab 01 broker running on localhost:1883.
"""

from __future__ import annotations

import argparse
import signal
import sys
import time
from pathlib import Path

# Allow `python src/main.py` from labs/simulator
sys.path.insert(0, str(Path(__file__).resolve().parent))

from controller import TankController
from mqtt_client import TwinMqtt
from process import CoupledTankPlant


def validate_envelope(msg: dict) -> tuple[bool, str]:
    if not isinstance(msg, dict):
        return False, "message must be a JSON object"
    if msg.get("schema_version") != "1.0":
        return False, "schema_version must be '1.0'"
    if "payload" not in msg or not isinstance(msg["payload"], dict):
        return False, "payload object is required"
    return True, "ok"


def main() -> int:
    parser = argparse.ArgumentParser(description="INC492 Lab 02 coupled-tank simulator")
    parser.add_argument("--host", default="localhost", help="MQTT broker host")
    parser.add_argument("--port", type=int, default=1883, help="MQTT broker port")
    parser.add_argument("--rate", type=float, default=2.0, help="Telemetry rate in Hz")
    args = parser.parse_args()

    if args.rate <= 0:
        print("--rate must be > 0")
        return 1

    dt = 1.0 / args.rate
    plant = CoupledTankPlant()
    ctrl = TankController(mode="manual", setpoint_m=0.8)
    mqtt = TwinMqtt(host=args.host, port=args.port)

    manual_valve = plant.valve_pct
    stop = False

    def handle_command(msg: dict) -> None:
        nonlocal manual_valve
        ok, reason = validate_envelope(msg)
        command_id = None
        if ok:
            payload = msg["payload"]
            command_id = payload.get("command_id")
            action = payload.get("action")
            params = payload.get("params", {})
            if not action:
                ok, reason = False, "payload.action is required"
            else:
                ok, reason, echo = ctrl.apply_command(str(action), params)
                if ok and action == "set_valve":
                    manual_valve = float(echo["valve_pct"])
                    plant.valve_pct = manual_valve

        mqtt.publish_event(
            {
                "command_id": command_id,
                "result": "accepted" if ok else "rejected",
                "reason": reason,
                "mode": ctrl.mode,
                "setpoint_m": ctrl.setpoint_m,
                "valve_pct": plant.valve_pct,
            }
        )
        print(f"[cmd] {'OK' if ok else 'REJECT'} {reason}")

    mqtt.set_command_handler(handle_command)

    def _stop(*_args) -> None:
        nonlocal stop
        stop = True

    signal.signal(signal.SIGINT, _stop)
    signal.signal(signal.SIGTERM, _stop)

    print(f"[sim] connecting to {args.host}:{args.port} at {args.rate} Hz")
    try:
        mqtt.connect()
    except Exception as exc:  # noqa: BLE001 — student-facing startup error
        print(f"[sim] cannot connect to broker: {exc}")
        print("[sim] Start Lab 01 first: cd labs/docker && docker compose up -d")
        return 1

    time.sleep(0.5)

    try:
        while not stop:
            t0 = time.perf_counter()
            plant.valve_pct = ctrl.compute_valve(plant.level2_m, manual_valve)
            plant.step(dt)
            snap = plant.snapshot()
            snap["controller_mode"] = ctrl.mode
            snap["setpoint_m"] = round(ctrl.setpoint_m, 4)
            mqtt.publish_telemetry(snap)
            elapsed = time.perf_counter() - t0
            time.sleep(max(0.0, dt - elapsed))
    finally:
        mqtt.disconnect()
        print("[sim] stopped")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
