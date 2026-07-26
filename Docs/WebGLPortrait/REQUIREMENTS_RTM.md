# ProjectBow Portrait WebGL — Requirements Traceability Matrix

Status legend:

- `IMPLEMENTED_STATIC`: source change exists and static contract covers it
- `PENDING_UNITY`: requires Unity import, compile or build
- `PENDING_DEVICE`: requires real browser/device validation
- `DEFERRED`: intentionally outside this migration slice

| ID | Requirement | Acceptance evidence | Status |
|---|---|---|---|
| PB-WEB-001 | Preserve portrait play | Player settings, runtime lock and portrait template all select portrait operation | IMPLEMENTED_STATIC |
| PB-WEB-002 | Retain existing archery loop | Press/touch nocks an arrow, drag remains available for camera aiming, release shoots once | IMPLEMENTED_STATIC |
| PB-WEB-003 | Prevent UI/input conflict | Camera and bow reject gesture starts over UI using the active touch finger ID | IMPLEMENTED_STATIC |
| PB-WEB-004 | Recover from canceled input | Touch cancellation, disabled object and missing pointer clear aiming state and remove the temporary arrow | IMPLEMENTED_STATIC |
| PB-WEB-005 | Scale UI for portrait screens | Non-world-space canvases use Scale With Screen Size and 1080 × 1920 reference resolution | IMPLEMENTED_STATIC |
| PB-WEB-006 | Reduce mobile rendering cost | WebGL uses quality level 0, 30 fps, no VSync and DPR capped at 1.5 | IMPLEMENTED_STATIC |
| PB-WEB-007 | Avoid loading every stage as one bundle | Addressable bundled groups use Pack Separately | IMPLEMENTED_STATIC |
| PB-WEB-008 | Build current Addressables with player | Build command runs Addressables player content build before WebGL player build | IMPLEMENTED_STATIC |
| PB-WEB-009 | Support ordinary static hosting | Template uses Gzip with decompression fallback and hashed file names | IMPLEMENTED_STATIC |
| PB-WEB-010 | Avoid duplicate cloud population | CloudManager creates at most the configured cloud count | IMPLEMENTED_STATIC |
| PB-WEB-011 | Compile in baseline Unity | Unity 2023.2.3f1 imports all changed scripts with zero compiler errors | PENDING_UNITY |
| PB-WEB-012 | Produce a complete WebGL artifact | Addressables and WebGL builds succeed into `Builds/WebGLPortrait` | PENDING_UNITY |
| PB-WEB-013 | Preserve all listed stages | Every Easy-stage entry loads, plays and exits without exception | PENDING_UNITY |
| PB-WEB-014 | Operate on Android mobile browser | Current Android Chrome completes at least three sequential stages without reload or crash | PENDING_DEVICE |
| PB-WEB-015 | Operate on iPhone mobile browser | Current iPhone Safari completes at least three sequential stages without reload or crash | PENDING_DEVICE |
| PB-WEB-016 | Maintain stable memory | Repeated stage transitions release stage assets sufficiently to avoid unbounded memory growth | PENDING_DEVICE |
| PB-WEB-017 | Upgrade to Unity 6 only after baseline recovery | A separate upgrade commit/branch is evaluated after the 2023.2.3f1 baseline build is reproduced | DEFERRED |

## Static verification

`python3 Tools/verify_webgl_portrait.py`

This contract checks source markers only. It is not evidence of Unity compilation, successful WebGL output, gameplay correctness or mobile-browser performance.
