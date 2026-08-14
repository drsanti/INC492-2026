# Topic 10: MQTT Communication

## Theme

Connect the Python simulation and Godot twin through a versioned, observable communication contract.

## Learning outcomes

- explain brokered publish/subscribe communication;
- publish and consume JSON telemetry and commands;
- validate payloads and handle disconnection;
- inspect traffic independently of both applications.

## Preparation

- Install or obtain access to the class MQTT broker.
- Finalize the Topic 2 topic and payload specification.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | MQTT roles, topics, QoS, retained data, and security scope |
| 00:30-01:10 | Connect Python publisher and independent subscriber |
| 01:10-01:30 | Payload validation, timestamps, and sequence numbers |
| 01:30-02:30 | Connect Godot and exchange project telemetry |
| 02:30-02:50 | Disconnect, malformed-message, and recovery tests |
| 02:50-03:00 | Integration checkpoint |

## Laboratory

Publish telemetry at the specified rate and receive commands on a separate topic. Every message must contain a schema version, source, timestamp, sequence number, and payload. Godot must visibly report connected, stale, and disconnected conditions.

## Deliverable

Python and Godot integration plus captured evidence of valid traffic, malformed-message rejection, and reconnection.

## Evidence and assessment

- Topic names and payloads match the interface contract
- Invalid messages do not crash either application
- Connection state is observable
- Commands contain enough information for audit and acknowledgment

## Instructor notes

Use a local broker for the laboratory when possible. The architecture may later be adapted to OPC UA or an industrial protocol, but one reliable protocol is sufficient for this course.
