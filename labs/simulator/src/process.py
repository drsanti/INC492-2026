"""Coupled-tank process model (Lab 02).

Two tanks connected by an orifice. Inflow enters tank 1 through a valve.
Outflow leaves tank 2. Units: level in meters, flows in m^3/s (scaled for a
small bench-top plant so levels move visibly in a few seconds).
"""

from __future__ import annotations

from dataclasses import dataclass


@dataclass
class CoupledTankPlant:
    """Simple nonlinear coupled-tank dynamics."""

    area_m2: float = 0.05
    level1_m: float = 0.4
    level2_m: float = 0.3
    level_min_m: float = 0.0
    level_max_m: float = 3.0
    q_in_max_m3s: float = 0.008
    k12: float = 0.01
    k_out: float = 0.008
    valve_pct: float = 40.0

    def step(self, dt_s: float) -> None:
        valve = max(0.0, min(100.0, self.valve_pct))
        q_in = self.q_in_max_m3s * (valve / 100.0)

        dh = self.level1_m - self.level2_m
        q_12 = self.k12 * (1.0 if dh >= 0 else -1.0) * (abs(dh) ** 0.5)

        q_out = self.k_out * (max(self.level2_m, 0.0) ** 0.5)

        a = self.area_m2
        self.level1_m += dt_s * (q_in - q_12) / a
        self.level2_m += dt_s * (q_12 - q_out) / a

        self.level1_m = max(self.level_min_m, min(self.level_max_m, self.level1_m))
        self.level2_m = max(self.level_min_m, min(self.level_max_m, self.level2_m))

    def snapshot(self) -> dict:
        valve = max(0.0, min(100.0, self.valve_pct))
        q_in = self.q_in_max_m3s * (valve / 100.0)
        return {
            "level1_m": round(self.level1_m, 4),
            "level2_m": round(self.level2_m, 4),
            "valve_pct": round(valve, 2),
            "inflow_m3s": round(q_in, 6),
            "quality": "good",
        }
