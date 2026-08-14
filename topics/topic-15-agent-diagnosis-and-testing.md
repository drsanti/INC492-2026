# Topic 15: Agent-Driven Diagnosis and Testing

## Theme

Use the agent to diagnose faults, select tests, and take bounded recovery actions.

## Learning outcomes

- build an evidence-based diagnostic workflow;
- compare conventional alarms, model residuals, and agent conclusions;
- require verification after every action;
- quantify diagnosis and recovery performance.

## Preparation

- Select Topic 13 scenarios for normal, clear-fault, and ambiguous-fault tests.
- Implement at least one threshold or residual-based baseline detector.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Diagnosis from symptoms, residuals, and hypotheses |
| 00:30-01:00 | Guided observe-diagnose-act-verify workflow |
| 01:00-01:30 | Metrics: detection delay, diagnosis accuracy, false action rate |
| 01:30-02:30 | Execute blind agent test scenarios |
| 02:30-02:50 | Compare agent and baseline results |
| 02:50-03:00 | Final-demo readiness review |

## Laboratory

The agent must:

1. observe current and historical telemetry;
2. list plausible causes and supporting evidence;
3. request a permitted diagnostic test when evidence is insufficient;
4. recommend or execute a bounded supervisory action;
5. verify the response and report whether recovery succeeded.

At least one test must withhold the ground-truth fault label from the students until the run is complete.

## Deliverable

Agent experiment log and comparison table covering baseline detection, agent diagnosis, actions, outcomes, and failures.

## Evidence and assessment

- Conclusions cite measured evidence
- Uncertainty is represented instead of hidden
- Actions are appropriate to the current mode and diagnosis confidence
- Performance is compared with a non-agent baseline

## Instructor notes

Reward agents that abstain appropriately. A confident but unsupported diagnosis should score lower than a cautious request for another safe test.
