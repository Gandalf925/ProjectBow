# ProjectBow Portrait WebGL — Release State

## Current classification

- Branch: `webgl-portrait-mobile`
- Date: 2026-07-26
- Status: implementation complete for the static migration slice; not release-ready
- Baseline: Unity 2023.2.3f1
- Orientation: portrait
- Verification completed: repository diff review and static contract definition
- Verification not completed: Unity compile, Addressables build, WebGL build, gameplay smoke, mobile device performance

## Implemented scope

- Portrait runtime and UI scaling policy
- Touch-aware bow and camera gesture ownership
- UI touch exclusion
- canceled-input recovery
- 30 fps Performant WebGL baseline
- custom portrait WebGL shell
- Addressables per-entry bundle configuration
- explicit Addressables-before-player build flow
- duplicate cloud population correction
- static verification workflow
- migration RTM, decisions and issue register

## Release blockers

1. Import the branch in the baseline Unity editor.
2. Restore any locally licensed Asset Store dependencies.
3. Reach zero C# compiler errors.
4. Build Addressables and WebGL successfully.
5. Smoke-test StageSelect and all listed Easy stages.
6. Verify portrait HUD anchors and safe areas.
7. Profile repeated stage changes on Android Chrome.
8. Validate startup, input and memory on iPhone Safari.
9. Resolve any correct-target stage failures found during smoke testing.

## Evidence boundary

No generated WebGL build, frame-time measurement, memory capture or mobile-browser execution result exists yet. The branch must remain separate from `main` until those gates are satisfied.
