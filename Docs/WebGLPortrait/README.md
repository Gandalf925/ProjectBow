# ProjectBow Portrait Mobile WebGL

## Purpose

This branch adapts the existing Android-oriented ProjectBow game for portrait mobile WebGL without redesigning its stage content or core archery loop.

- Source branch: `main`
- Migration branch: `webgl-portrait-mobile`
- Baseline Unity version: `2023.2.3f1`
- Intended play orientation: portrait
- Current verification: source/static checks only
- Unity compile, WebGL build, Android Chrome and iPhone Safari tests: pending

## Implemented changes

1. Portrait mobile runtime configuration
   - 30 fps target
   - VSync disabled
   - one active touch at a time
   - portrait orientation on Android/iOS
   - Performant quality level on WebGL
   - CanvasScaler normalized to 1080 × 1920
   - low-memory cleanup hook

2. Touch-safe archery input
   - touch finger IDs are retained from press to release
   - UI touches cannot start camera or bow gestures
   - canceled touches cleanly remove a nocked arrow
   - mouse input remains available for editor and desktop WebGL testing

3. Portrait WebGL template
   - safe-area support
   - scrolling, selection, pinch zoom and browser gesture suppression
   - capped device pixel ratio
   - tap-to-start gate for browser audio/input requirements
   - portrait rotation notice

4. Addressables and loading
   - player build explicitly rebuilds Addressables
   - bundled Addressable groups use Pack Separately
   - bundle cache and CRC remain enabled

5. Performance correction
   - duplicate cloud spawning removed
   - mobile WebGL defaults to the existing Performant quality preset

## Build procedure

1. Checkout `webgl-portrait-mobile`.
2. Open the project with Unity `2023.2.3f1` first.
3. Allow package and Asset Store dependencies to import.
4. Resolve any missing licensed Asset Store packages locally.
5. In Unity, run:
   - `ProjectBow > WebGL > Configure Portrait Mobile`
   - `ProjectBow > WebGL > Build Portrait WebGL`
6. The output is created at `Builds/WebGLPortrait`.
7. Serve the output through HTTP or HTTPS. Do not open `index.html` directly through `file://`.

Command-line entry point:

```text
-executeMethod ProjectBowWebGLBuild.BuildFromCommandLine
```

## Required acceptance test

The branch is not release-complete until all of the following pass:

- Unity imports with no compiler errors.
- Addressables build completes.
- WebGL player build completes.
- Stage Select loads.
- Every currently listed Easy stage opens and returns to Stage Select.
- Touch-drag rotates the view without activating UI controls.
- Touch-release fires exactly one arrow.
- Canceling or losing a touch does not leave the bow stuck.
- Portrait UI remains usable at 360 × 800, 393 × 873 and 430 × 932 CSS pixels.
- Android Chrome survives at least three sequential stages.
- iPhone Safari survives at least three sequential stages.
- Memory does not grow without recovery after repeated stage changes.

See `REQUIREMENTS_RTM.md`, `BUG_REGISTER.md`, `DECISION_HISTORY.md`, and `RELEASE_STATE.md` for status and traceability.
