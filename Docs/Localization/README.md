# New Master — Localization

## Supported locales

The initial production target is:

- en — English
- pl — Polish

The runtime uses stable localization keys from NewMasterTextKeys. Gameplay state stores keys and typed numeric arguments rather than player-facing Polish strings.

## Translation source

Docs/Localization/NewMaster_UI.csv is the current translation source for the first UI/runtime string set.

The CSV contains:

- stable key
- English
- Polish

It is intentionally kept separate from generated Unity Localization assets.

## Unity Localization setup

The repository already declares com.unity.localization 1.5.8.

In Unity Editor, create/configure:

1. Locale en
2. Locale pl
3. String Table Collection named New Master UI
4. Import the CSV translation source into the corresponding string table.
5. Verify all keys from NewMasterTextKeys exist in the collection.
6. Enable the selected locales in Localization Settings.
7. Test locale switching and fallback behavior in Play Mode.
8. Test pseudo-localization before production UI lock.

Unity's Localization package supports string localization, Smart Strings, pseudo-localization and CSV/XLIFF import/export.

## Runtime rules

Do not put Polish/English player-facing strings directly into gameplay services.

Use:

- NewMasterTextKeys for stable keys.
- GameStatus for transient gameplay messages.
- NewMasterLocalization.Get(...) for formatted runtime output.
- NewMasterLocalizedText / LocalizeStringEvent for component-driven UI.

LocalizedString supports runtime formatting arguments, and LocalizeStringEvent refreshes when the selected locale or string reference changes.

## Current limitation

The repository contains the runtime architecture and translation source, but the actual Unity Locale and String Table assets still require Unity Editor creation/import. Therefore multilingual runtime behavior is not yet Editor-validated.
