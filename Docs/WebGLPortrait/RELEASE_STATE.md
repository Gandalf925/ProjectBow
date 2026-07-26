# ProjectBow Portrait WebGL — Release State

## Current classification

- Branch: `webgl-portrait-mobile`
- Draft PR: `#1`
- Date: 2026-07-26
- Status: static migration implementation complete; not release-ready
- Baseline: Unity 2023.2.3f1
- Orientation: portrait
- Static verification: 1 / 1 GREEN in GitHub Actions run `30205177842`
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
- sparse static verification workflow for the oversized repository
- migration RTM, decisions and issue register

## Verification evidence

- Initial workflow run `30205090223`: failed during full repository checkout before tests because the repository contains more than 114,000 files and is asset-heavy.
- Workflow was corrected to use partial clone and sparse checkout of migration files only.
- Follow-up workflow run `30205177842`: checkout GREEN, static portrait contract GREEN.
- The passing static contract verifies required source markers and the absence of the former duplicate-cloud initialization path.

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
