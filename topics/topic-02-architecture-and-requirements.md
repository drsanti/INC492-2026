# Topic 2: Architecture and Requirements

## Theme

Turn the project concept into an executable digital-twin specification.

## Learning outcomes

- decompose the system into process, control, communication, visualization, safety, and agent services;
- define signals with units, ranges, sample periods, and ownership;
- specify MQTT topics and structured message payloads;
- write measurable acceptance criteria.

## Preparation

- Revise the Topic 1 concept after instructor feedback.
- Review JSON objects and publish/subscribe communication.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:25 | Review project scopes and common boundary errors |
| 00:25-01:00 | Reference architecture and data ownership |
| 01:00-01:30 | Sampling, timestamps, units, ranges, and quality indicators |
| 01:30-02:30 | Workshop: signal dictionary and MQTT interface contract |
| 02:30-02:50 | Define testable functional and safety requirements |
| 02:50-03:00 | Interface-contract review |

## Laboratory

Create a signal dictionary. Each entry must include:

- signal name and engineering unit;
- source and destination;
- valid range and safe range;
- update rate;
- data type;
- behavior when data becomes stale or invalid.

Define topic groups such as `twin/telemetry`, `twin/command`, `twin/event`, and `twin/agent/action`. Commands must include an identifier, timestamp, action, parameters, and source.

## Deliverable

`system-specification.md` containing the architecture, signal dictionary, topic contract, operating modes, and at least five acceptance tests.

## Evidence and assessment

- No signal has ambiguous ownership or units
- Commands and telemetry are separated
- Requirements are measurable
- Safety limits are defined before AI-agent development

## Instructor notes

Freeze the main interfaces after this week. Later changes are allowed, but students must document the reason and update both publisher and subscriber.
