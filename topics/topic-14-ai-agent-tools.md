# Topic 14: AI Agent Tools and Structured Actions

## Theme

Give an AI agent restricted tools for observing and supervising the twin.

## Learning outcomes

- distinguish an AI agent from a prediction model or chatbot;
- expose telemetry, history, alarms, and commands as bounded tools;
- require structured outputs and validate every requested action;
- record the evidence and rationale used for a decision.

## Preparation

- Complete the safe command gateway from Topic 12.
- Prepare compact telemetry summaries and tool descriptions.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Agent loop: observe, reason, act, verify |
| 00:30-01:00 | Tool contracts and minimum necessary context |
| 01:00-01:30 | Structured action schema and safety validation |
| 01:30-02:30 | Implement read-only agent tools, then one guarded action |
| 02:30-02:50 | Test malformed, unsupported, and unsafe requests |
| 02:50-03:00 | Agent-boundary review |

## Laboratory

Provide tools such as:

- `get_current_state()`;
- `get_signal_history(signal, duration)`;
- `get_active_alarms()`;
- `propose_setpoint(value, reason)`;
- `request_test_scenario(name)`.

Begin in advisory mode. Enable actions only after they pass schema, range, mode, interlock, and authorization checks. Log the observation, proposed action, validation result, acknowledgment, and resulting state.

## Deliverable

An agent interface that summarizes current conditions, explains one recommendation, and submits one validated supervisory action.

## Evidence and assessment

- Agent access is limited to explicit tools
- Outputs conform to a schema
- Unsafe or unknown actions are rejected deterministically
- Decisions and outcomes are auditable

## Instructor notes

An LLM service is optional. A local rule-based planner can exercise the same tool boundary if external model access is unavailable. Assess architecture and evidence, not persuasive natural-language style.
