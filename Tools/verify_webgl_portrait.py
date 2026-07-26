#!/usr/bin/env python3
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
failures: list[str] = []


def require(path: str, *needles: str) -> None:
    file_path = ROOT / path
    if not file_path.is_file():
        failures.append(f"missing file: {path}")
        return

    text = file_path.read_text(encoding="utf-8")
    for needle in needles:
        if needle not in text:
            failures.append(f"{path}: missing {needle!r}")


def forbid(path: str, *needles: str) -> None:
    file_path = ROOT / path
    if not file_path.is_file():
        failures.append(f"missing file: {path}")
        return

    text = file_path.read_text(encoding="utf-8")
    for needle in needles:
        if needle in text:
            failures.append(f"{path}: forbidden marker {needle!r}")


require(
    "Assets/Scripts/ProjectBowMobileRuntime.cs",
    "Application.targetFrameRate = 30",
    "QualitySettings.SetQualityLevel(0, true)",
    "new Vector2(1080f, 1920f)",
    "ScreenOrientation.Portrait",
    "Input.multiTouchEnabled = false",
)

require(
    "Assets/Scripts/PointerInputUtility.cs",
    "IsPointerOverGameObject(pointerId)",
    "Input.touchCount",
    "fingerId",
)

require(
    "Assets/Scripts/BowController.cs",
    "PointerInputUtility.IsPointerOverUI",
    "TouchPhase.Canceled",
    "CancelAim()",
    "activePointerId",
)

require(
    "Assets/Scripts/CameraController.cs",
    "PointerInputUtility.IsPointerOverUI",
    "TouchPhase.Canceled",
    "1080f / Mathf.Max(Screen.height, 1)",
)

require(
    "Assets/Editor/ProjectBowWebGLBuild.cs",
    "UIOrientation.Portrait",
    "PROJECT:ProjectBowPortrait",
    "initialMemorySize\", 128",
    "maximumMemorySize\", 1024",
    "report.summary.totalSize",
)

forbid(
    "Assets/Editor/ProjectBowWebGLBuild.cs",
    "UnityEditor.AddressableAssets",
    "BuildPlayerContent",
    "BundlePackingMode.PackSeparately",
    "BuildAddressables()",
)

require(
    "Packages/manifest.json",
    "Unity-Technologies/com.unity.cinemachine.git?path=/com.unity.cinemachine#release/2.9",
)

require(
    "Assets/WebGLTemplates/ProjectBowPortrait/index.html",
    "viewport-fit=cover",
    "touch-action: none",
    "devicePixelRatio: Math.min",
    "TAP TO START",
    "ProjectBow is designed for portrait play",
)

forbid(
    "Assets/Scripts/CloudManager.cs",
    "InitializationClouds();",
    "using UnityEngine.PlayerLoop;",
)

if failures:
    print("Portrait WebGL contract: FAILED")
    for failure in failures:
        print(f" - {failure}")
    sys.exit(1)

print("Portrait WebGL contract: PASS")
