# Topic 11: Real-Time Twin Synchronization

## Theme

Maintain a trustworthy relationship between simulation time, received telemetry, and visual state.

## Learning outcomes

- distinguish simulation, source, arrival, and display time;
- detect dropped, duplicated, delayed, and out-of-order messages;
- interpolate visual motion without falsifying engineering values;
- measure end-to-end update latency.

## Preparation

- Integrate the Topic 10 MQTT connection.
- Review sampling and basic latency statistics.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Time semantics and synchronization failure modes |
| 00:30-01:00 | Sequence checks, stale-data timers, and buffering |
| 01:00-01:30 | Guided interpolation and timestamped latency measurement |
| 01:30-02:30 | Implement synchronization policies |
| 02:30-02:50 | Network impairment experiment |
| 02:50-03:00 | Results review |

## Laboratory

Introduce artificial delay, jitter, dropped messages, and duplicates. Record the effect on engineering values and visualization. Define thresholds for fresh, stale, and disconnected data and justify them from the update rate.

## Deliverable

Synchronization implementation and a short report containing latency distribution, loss behavior, and stale-data policy.

## Evidence and assessment

- Old data cannot overwrite newer state
- Interpolation affects presentation, not logged measurements
- Stale values are visibly marked
- Latency is measured rather than estimated informally

## Instructor notes

This week makes the distinction between an attractive animation and a trustworthy twin concrete. Ask students what an operator could incorrectly conclude from delayed but visually smooth data.
