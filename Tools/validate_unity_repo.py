#!/usr/bin/env python3
"""Repository-level Unity structure validation without requiring the Unity Editor."""

from __future__ import annotations

import json
import sys
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
ERRORS: list[str] = []


def load_json(path: Path):
    try:
        return json.loads(path.read_text(encoding="utf-8"))
    except Exception as exc:
        ERRORS.append(f"{path}: invalid JSON: {exc}")
        return None


def validate_project_version() -> None:
    path = ROOT / "ProjectSettings" / "ProjectVersion.txt"
    if not path.is_file():
        ERRORS.append(f"Missing {path}")
        return

    text = path.read_text(encoding="utf-8")
    if "m_EditorVersion:" not in text:
        ERRORS.append(f"{path}: missing m_EditorVersion")


def validate_manifest() -> None:
    path = ROOT / "Packages" / "manifest.json"
    manifest = load_json(path)
    if manifest is None:
        return

    required = {
        "com.unity.inputsystem",
        "com.unity.render-pipelines.universal",
        "com.unity.addressables",
        "com.unity.localization",
        "com.unity.test-framework",
        "com.unity.timeline",
        "com.unity.ugui",
    }
    packages = manifest.get("dependencies", {})
    missing = sorted(required - packages.keys())
    if missing:
        ERRORS.append(f"{path}: missing required packages: {', '.join(missing)}")


def validate_asmdefs() -> None:
    asmdefs = {}
    for path in ROOT.glob("Assets/**/*.asmdef"):
        data = load_json(path)
        if data is None:
            continue

        name = data.get("name")
        if not name:
            ERRORS.append(f"{path}: missing assembly name")
            continue

        if name in asmdefs:
            ERRORS.append(f"Duplicate assembly name: {name}")
        asmdefs[name] = (path, data)

    graph = {name: [] for name in asmdefs}
    for name, (path, data) in asmdefs.items():
        for reference in data.get("references", []):
            if reference in asmdefs:
                graph[name].append(reference)

    visiting: set[str] = set()
    visited: set[str] = set()

    def visit(name: str, stack: list[str]) -> None:
        if name in visiting:
            cycle = " -> ".join(stack + [name])
            ERRORS.append(f"Assembly reference cycle: {cycle}")
            return
        if name in visited:
            return

        visiting.add(name)
        for dependency in graph[name]:
            visit(dependency, stack + [name])
        visiting.remove(name)
        visited.add(name)

    for name in graph:
        visit(name, [])

    for name, (path, data) in asmdefs.items():
        for reference in data.get("references", []):
            if reference.startswith("NewMaster.") and reference not in asmdefs:
                ERRORS.append(
                    f"{path}: references missing project assembly {reference}"
                )


def validate_source_roots() -> None:
    for path in ROOT.glob("Assets/**/*.cs"):
        if "Library" in path.parts or "Temp" in path.parts:
            ERRORS.append(f"Generated Unity directory contains source file: {path}")


def main() -> int:
    validate_project_version()
    validate_manifest()
    validate_asmdefs()
    validate_source_roots()

    if ERRORS:
        print("New Master repository validation FAILED")
        for error in ERRORS:
            print(f"- {error}")
        return 1

    print("New Master repository validation PASSED")
    return 0


if __name__ == "__main__":
    sys.exit(main())
