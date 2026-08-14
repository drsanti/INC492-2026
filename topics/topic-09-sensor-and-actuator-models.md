# Topic 9: Sensor and Actuator Models

## Theme

Make the simulated hardware behave like instrumentation rather than perfect variables.

## Learning outcomes

- model noise, bias, drift, quantization, and sampling;
- model actuator saturation, rate limits, deadband, and delay;
- distinguish true process state from measured and commanded values;
- define data-quality flags.

## Preparation

- Identify realistic limitations for each project sensor and actuator.
- Review first-order dynamics and discrete sampling.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Measurement chain and actuator chain |
| 00:30-01:00 | Guided noisy level transmitter and delayed valve |
| 01:00-01:30 | Quality flags, calibration, and deterministic random seeds |
| 01:30-02:30 | Add project instrumentation models |
| 02:30-02:50 | Compare ideal and non-ideal closed-loop results |
| 02:50-03:00 | Instrumentation review |

## Laboratory

Implement at least three sensor effects and two actuator effects. Log true value, measured value, requested command, and actual actuator output as separate signals.

## Deliverable

Updated simulator and a comparison report showing how non-ideal instrumentation changes control performance.

## Evidence and assessment

- Effects use documented parameters and units
- Random behavior can be reproduced
- True and measured values are never conflated
- Invalid or unavailable measurements carry a quality state

## Instructor notes

These models provide the evidence needed for later fault diagnosis. Encourage students to choose effects that matter to their selected process instead of adding every possible imperfection.
