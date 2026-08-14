# Topic 13: Fault Injection and Test Scenarios

## Theme

Create repeatable abnormal conditions and collect labeled evidence for diagnosis.

## Learning outcomes

- distinguish disturbances, degradation, faults, and communication failures;
- inject faults without corrupting the underlying experiment;
- define expected symptoms and safety responses;
- calculate detection delay and false-alarm behavior.

## Preparation

- Select one sensor, one actuator, and one process fault.
- Define baseline normal scenarios for comparison.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Fault taxonomy and test-case design |
| 00:30-01:00 | Guided bias, stuck-valve, and leakage injection |
| 01:00-01:30 | Scenario files, labels, seeds, and expected results |
| 01:30-02:30 | Implement and execute team fault scenarios |
| 02:30-02:50 | Evaluate alarms and control response |
| 02:50-03:00 | Dataset readiness review |

## Laboratory

Run a normal baseline and at least four fault scenarios. Each scenario must define start conditions, injection time, fault magnitude, expected symptoms, safety response, duration, and pass criteria. Store fault labels separately from the telemetry available to the detector.

## Deliverable

Reusable scenario definitions, labeled experiment logs, and a test-results table.

## Evidence and assessment

- Experiments are repeatable
- Fault truth is not leaked into detector inputs
- Disturbances and faults are tested separately
- Expected and observed behavior are compared explicitly

## Instructor notes

The data generated here becomes the evidence base for the AI agent. Require at least one ambiguous case where multiple faults could produce similar symptoms.
