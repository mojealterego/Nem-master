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
- Localization: Unity Localization package 1.5.8 configured; stable key registry, structured GameStatus messages, runtime localization facade, persistent language controller, localized HUD/panels and English/Polish translation source added; actual Locale/String Table assets and Editor validation still require Unity Editor.
- Unity Test Framework: configured

## Current architecture
- `Assets/Scripts/Core`: runtime state, spin outcomes/rules and game orchestration.
- `Assets/Scripts/Worlds`: world definitions and scalable world catalog.
- `Assets/Scripts/Progression`: world progression rules.
- `Assets/Scripts/Village`: building data, village state and upgrade rules.
- `Assets/Scripts/Bootstrap`: New Master composition/bootstrap entry point and mobile Input System UI bootstrap.
- `Assets/Tests/EditMode`: deterministic gameplay tests.
- Runtime assembly: `NewMaster.Runtime`.
- Test assembly: `NewMaster.Tests.EditMode`.

## Multilingual foundation
The runtime no longer uses player-facing Polish status strings as gameplay state. GameStatus stores a stable localization key plus typed numeric arguments, with migration support from the previous save format. English and Polish translation source is maintained in `Docs/Localization/NewMaster_UI.csv`. The Unity String Table named `New Master UI` still needs to be created/imported in the Editor.

## Gameplay foundation
The current runtime foundation also includes persisted language/settings, collection persistence, mobile lifecycle save hooks and an Android safe-area UI component.

The current vertical foundation establishes:
1. energy-gated spin;
2. typed spin outcomes;
3. deterministic reward resolution separated from Unity presentation;
4. data-driven world progression;
5. village building progression;
6. a catalog architecture capable of scaling to large numbers of worlds without one-off gameplay code.

The repository now also contains deterministic content-generation/validation tooling for 365 worlds, economy transaction primitives, social/live-ops/monetization/analytics/online contracts, a production boot-scene generator, runtime 365-world fallback content, persisted-state normalization, and repository-level assembly/package validation. These are implementation foundations; external backend services, Unity Editor-generated content/assets and platform store integrations still require their respective environments.

## Validation status
Repository-level implementation has been updated, but Unity Editor compilation, Play Mode, Android device testing and player build generation have not been executed from this chat surface. Do not treat the repository as release-ready until those validations are run.

## Gameplay engines added
The repository now also contains a Raid Engine 2.0 layer with bounded defense mitigation and counter-attack state, a persisted World Boss engine integrated with the current world, and a Social-assembly Guild/Co-op contribution service with milestone handling. These systems have deterministic EditMode tests. They remain repository-level implementations until Unity Editor, runtime, online authority and device validation are executed in their respective environments.

## Naming rule
Use **New Master** in all product-facing text, UI and documentation. The repository slug remains `Nem-master` unless explicitly renamed.
