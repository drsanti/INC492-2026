"""Deterministic supervisory + simple auto control for Lab 02."""

from __future__ import annotations

from dataclasses import dataclass


@dataclass
class TankController:
    """Manual valve or automatic P-control on tank-2 level."""

    mode: str = "manual"  # manual | auto
    setpoint_m: float = 0.8
    kp: float = 80.0
    valve_min: float = 0.0
    valve_max: float = 100.0
    level_min_m: float = 0.0
    level_max_m: float = 3.0

    def compute_valve(self, level2_m: float, manual_valve_pct: float) -> float:
        if self.mode == "auto":
            error = self.setpoint_m - level2_m
            valve = 50.0 + self.kp * error
        else:
            valve = manual_valve_pct
        return max(self.valve_min, min(self.valve_max, valve))

    def apply_command(self, action: str, params: dict) -> tuple[bool, str, dict]:
        """Validate and apply a command. Returns (ok, reason, echo)."""
        action = (action or "").strip()
        params = params or {}

        if action == "set_mode":
            mode = str(params.get("mode", "")).lower()
            if mode not in {"manual", "auto"}:
                return False, "mode must be 'manual' or 'auto'", {}
            self.mode = mode
            return True, "mode updated", {"mode": self.mode}

        if action == "set_setpoint":
            if "level_m" not in params:
                return False, "params.level_m is required", {}
            try:
                sp = float(params["level_m"])
            except (TypeError, ValueError):
                return False, "params.level_m must be a number", {}
            if not (self.level_min_m <= sp <= self.level_max_m):
                return False, f"level_m out of range [{self.level_min_m}, {self.level_max_m}]", {}
            self.setpoint_m = sp
            return True, "setpoint updated", {"level_m": self.setpoint_m}

        if action == "set_valve":
            if "valve_pct" not in params:
                return False, "params.valve_pct is required", {}
            try:
                valve = float(params["valve_pct"])
            except (TypeError, ValueError):
                return False, "params.valve_pct must be a number", {}
            if not (self.valve_min <= valve <= self.valve_max):
                return False, f"valve_pct out of range [{self.valve_min}, {self.valve_max}]", {}
            if self.mode != "manual":
                return False, "set_valve only allowed in manual mode", {}
            return True, "valve command accepted", {"valve_pct": valve}

        return False, f"unknown action '{action}'", {}
