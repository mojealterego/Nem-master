# New Master — Android CI Build

The repository now contains a reproducible GitHub Actions Android APK pipeline for Unity 6000.3.0f1.

## Required repository secrets

Configure these GitHub Actions secrets before running the Android build:

- `UNITY_EMAIL`
- `UNITY_PASSWORD`
- `UNITY_SERIAL`

They are injected into the GameCI build runner and are not committed to the repository.

## Output

The workflow produces:

`Builds/NewMaster-Android.apk`

and uploads it as the `NewMaster-Android-APK` workflow artifact.

## What the pipeline validates

- Unity project import
- C# compilation during the Unity build
- Android player build
- configured New Master boot scene
- final APK artifact creation

A successful CI build is still not a substitute for physical Android device testing, Play Mode verification and performance profiling.
