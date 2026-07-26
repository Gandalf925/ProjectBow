# ProjectBow Portrait WebGL — Requirements Traceability Matrix

Status legend:

- `IMPLEMENTED_STATIC`: source change exists and static contract covers it
- `PENDING_UNITY`: requires Unity import, compile or build
- `PENDING_MIGRATION`: implementation must be replaced or completed
- `PENDING_DEVICE`: requires real browser/device validation
- `SUPERSEDED`: retained only as historical evidence
- `DEFERRED`: intentionally outside this migration slice

| ID | Requirement | Acceptance evidence | Status |
|---|---|---|---|
| PB-WEB-001 | Preserve portrait play | Player settings, runtime lock and portrait template all select portrait operation | IMPLEMENTED_STATIC |
| PB-WEB-002 | Retain existing archery loop | Press/touch nocks an arrow, drag remains available for camera aiming, release shoots once | IMPLEMENTED_STATIC |
| PB-WEB-003 | Prevent UI/input conflict | Camera and bow reject gesture starts over UI using the active touch finger ID | IMPLEMENTED_STATIC |
| PB-WEB-004 | Recover from canceled input | Touch cancellation, disabled object and missing pointer clear aiming state and remove the temporary arrow | IMPLEMENTED_STATIC |
| PB-WEB-005 | Scale UI for portrait screens | Non-world-space canvases use Scale With Screen Size and 1080 × 1920 reference resolution | IMPLEMENTED_STATIC |
| PB-WEB-006 | Reduce mobile rendering cost | WebGL uses quality level 0, 30 fps, no VSync and DPR capped at 1.5 | IMPLEMENTED_STATIC |
| PB-WEB-007 | Avoid loading every stage as one Addressables bundle | Historical Pack Separately configuration | SUPERSEDED by PB-WEB-018 |
| PB-WEB-008 | Build current Addressables with player | Historical Addressables player-content build | SUPERSEDED by PB-WEB-018 |
| PB-WEB-009 | Support ordinary static hosting | Template uses compression fallback and hashed file names | IMPLEMENTED_STATIC; BSV artifact policy still requires review |
| PB-WEB-010 | Avoid duplicate cloud population | CloudManager creates at most the configured cloud count | IMPLEMENTED_STATIC |
| PB-WEB-011 | Compile in baseline Unity | Unity 2023.2.3f1 imports all changed scripts with zero compiler errors | PENDING_UNITY |
| PB-WEB-012 | Produce a complete WebGL artifact | WebGL build succeeds and contains index, loader/framework, Wasm and data files | PENDING_UNITY |
| PB-WEB-013 | Preserve all listed stages | Every Easy-stage entry loads, plays and exits without exception | PENDING_UNITY |
| PB-WEB-014 | Operate on Android mobile browser | Current Android Chrome completes at least three sequential stages without reload or crash | PENDING_DEVICE |
| PB-WEB-015 | Operate on iPhone mobile browser | Current iPhone Safari completes at least three sequential stages without reload or crash | PENDING_DEVICE |
| PB-WEB-016 | Maintain stable memory | Repeated stage transitions do not cause unbounded memory growth or browser termination | PENDING_DEVICE |
| PB-WEB-017 | Upgrade to Unity 6 only after baseline recovery | A separate upgrade commit/branch is evaluated after the 2023.2.3f1 baseline build is reproduced | DEFERRED |
| PB-WEB-018 | Remove runtime Addressables for Bitcoin SV distribution | No runtime source imports Addressables; no catalog or AssetBundle request occurs; every playable stage is embedded in the immutable WebGL release | PENDING_MIGRATION |
| PB-WEB-019 | Reuse existing Unity Actions secret names without exposing values | Preflight confirms `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD` availability before checkout/build; source and logs contain no values | PENDING_UNITY |
| PB-WEB-020 | Keep `main` unchanged until gates pass | Draft PR remains unmerged until Unity build, stage regression and mobile device acceptance pass | IMPLEMENTED_STATIC |
| PB-WEB-021 | Record release size and peak memory | Build artifact byte size, largest files and repeated-stage peak memory are documented | PENDING_UNITY |

## TDD and validation sequence

1. **RED:** Run the Unity CI credential preflight and historical 2023.2.3f1 build; record the first reproducible failure.
2. **GREEN:** Restore zero-error Unity import and a baseline WebGL artifact without redesigning gameplay.
3. **RED:** Add a static/runtime contract that fails while Addressables imports, catalog definitions or runtime calls remain.
4. **GREEN:** Replace Addressables-backed stage loading with direct build-contained stage references and remove the package dependency.
5. **REFACTOR:** Consolidate stage creation/destruction and preserve save compatibility.
6. Run every listed stage, browser lifecycle checks, repeated-stage memory tests and physical-device acceptance.

## Static verification

`python3 Tools/verify_webgl_portrait.py`

The current static contract checks portrait/input/performance source markers only. It is not evidence of Unity compilation, successful WebGL output, Addressables removal, gameplay correctness or mobile-browser performance.
