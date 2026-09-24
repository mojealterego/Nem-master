# Nem Master — Unity Project Context

## Status
- Repository: `mojealterego/Nem-master`
- Project state: newly initialized from an empty repository.
- Engine target: Unity 6.3 LTS.
- Primary target: Android/mobile first.
- Architectural direction: modular, data-driven mobile game architecture.
- Source basis: supplied Coin Master analysis document.

## Confirmed design goals
- Slot/spin core loop.
- Coins and energy.
- Data-driven worlds; target architecture supports 365+ worlds without one-off gameplay code.
- Village/building progression.
- Shop/monetization layer.
- Leaderboards/social systems.
- Cloud synchronization and server authority as later production layers.
- Addressable/on-demand content for scalable world assets.
- Mobile-oriented performance and adaptive UI.

## Initial architecture
- `Assets/Scripts/Core`: runtime game state and engine.
- `Assets/Scripts/Worlds`: ScriptableObject world definitions.
- `Assets/Scripts/Bootstrap`: application composition/bootstrap.
- Future modules: Save, Networking, Economy, Village, Social, LiveOps, UI, Audio, VFX, Analytics.

## Validation
This repository has not yet been opened in the Unity Editor from this chat surface. Compilation, scene validation, Play Mode behavior, device testing and Android builds remain unverified.

## Source
The supplied analysis document describes a progression from a simple slot prototype toward a production architecture with data-driven worlds, persistence, networking, DI, analytics, monetization, visual effects and live content.
