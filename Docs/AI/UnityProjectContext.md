# New Master — Unity Project Context

## Project summary
**New Master** is an original next-generation Android/mobile game designed as a successor to the Coin Master-style progression formula, not a 1:1 clone. The repository is being developed as a data-driven game platform with an original economy, progression, worlds, village systems, social/live-ops layers and presentation.

## Confirmed environment
- Repository: `mojealterego/Nem-master`
- Product name: **New Master**
- Unity target: 6000.3.0f1
- Primary platform: Android/mobile
- Render pipeline: Universal Render Pipeline package configured
- Input: Unity Input System package configured
- Addressables: configured as a planned content-delivery dependency
- Localization: Unity Localization package configured; runtime localization facade, stable key registry and language controller added; Locale/String Table asset creation still requires Unity Editor.
- Unity Test Framework: configured

## Current architecture
- `Assets/Scripts/Core`: runtime state, spin outcomes/rules and game orchestration.
- `Assets/Scripts/Worlds`: world definitions and scalable world catalog.
- `Assets/Scripts/Progression`: world progression rules.
- `Assets/Scripts/Village`: building data, village state and upgrade rules.
- `Assets/Scripts/Bootstrap`: New Master composition/bootstrap entry point.
- `Assets/Tests/EditMode`: deterministic gameplay tests.
- Runtime assembly: `NewMaster.Runtime`.
- Test assembly: `NewMaster.Tests.EditMode`.

## Gameplay foundation
The current vertical foundation establishes:
1. energy-gated spin;
2. typed spin outcomes;
3. deterministic reward resolution separated from Unity presentation;
4. data-driven world progression;
5. village building progression;
6. a catalog architecture capable of scaling to large numbers of worlds without one-off gameplay code.

The supplied design analysis supports the broader target of scalable worlds, persistence, networking, DI, analytics, monetization, visual effects and live content. Those systems are not yet represented as completed production implementations in this repository.

## Validation status
Repository-level implementation has been updated, but Unity Editor compilation, Play Mode, Android device testing and player build generation have not been executed from this chat surface. Do not treat the repository as release-ready until those validations are run.

## Naming rule
Use **New Master** in all product-facing text, UI and documentation. The repository slug remains `Nem-master` unless explicitly renamed.
