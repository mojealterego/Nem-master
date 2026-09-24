# LiveOps Architecture

New Master now supports a data-driven LiveOps layer built on the existing persistent GameState.LiveOps model.

## Runtime flow

1. LiveEventDefinition defines an event window in UTC.
2. LiveMissionDefinition defines mission identity, target and rewards.
3. LiveOpsProgressService owns progress, capping and one-time claiming.
4. LiveOpsProgressState.ActiveEventId isolates progress to the currently active event.
5. NewMasterLiveOpsPanel binds definitions to UGUI/TMP and exposes claim actions.
6. GameEngine emits progression hooks for the core spin loop.
7. The existing save system persists LiveOps state with the main game save.

## Event isolation

Activating a different event clears the active event's mission-progress collection. This intentionally keeps a single active-event model deterministic and prevents missions from one event being credited to another.

The existing serialized state remains backward-compatible because the new MissionProgress.EventId field is additive.

## Starter content

Unity Editor menu:

New Master/Content/Generate LiveOps Starter

The generator creates:

- event.starter
- spin.50
- spin.250
- jackpot.10

The generated event window starts at generation time and remains active for seven days.

## UI setup

Add NewMasterLiveOpsPanel to a UGUI panel and assign:

- GameEngine
- active LiveEventDefinition
- mission definition array
- event label
- mission labels
- claim buttons

Mission and button arrays are index-aligned.

## Validation boundary

The repository contains EditMode coverage for:

- progress creation and target capping;
- claim requirements;
- single-claim reward integrity;
- event isolation.

Actual Unity Editor import, Play Mode, scene wiring and Android device behavior still require Unity Editor/CI execution.
