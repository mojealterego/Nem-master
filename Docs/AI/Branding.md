# New Master — Branding Layer

## Approved game identity

The supplied New Master game icon is now stored in the Unity project as:

`Assets/NewMaster/Branding/NewMasterIcon.jpg`

The source artwork is square and is used as the canonical product icon asset for the Android-first build.

## Unity integration

The Editor-only tool:

`New Master/Branding/Apply Android App Icon`

uses Unity 6's current `PlayerSettings.SetIcons(NamedBuildTarget, Texture2D[], IconKind)` API to populate the Android application-icon slots with the approved asset.

The tool also enforces the product-facing Unity name:

`New Master`

A second menu item validates that the icon asset exists and is square:

`New Master/Branding/Validate Brand Asset`

## Runtime presentation component

`NewMasterBranding` is a small uGUI presentation component that projects the configured logo and product title into a scene. It intentionally owns no gameplay state.

## Validation boundary

The repository contains the asset, metadata and Editor integration, but the actual PlayerSettings mutation and Unity import must still be executed inside Unity Editor. The Android CI pipeline can validate the resulting player only after Unity activation/build prerequisites are configured.
