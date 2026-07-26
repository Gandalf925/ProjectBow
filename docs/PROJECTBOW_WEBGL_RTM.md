# ProjectBow WebGL / Bitcoin SV Distribution RTM

Date: 2026-07-27  
Repository: `Gandalf925/ProjectBow`  
Working branch: `agent/projectbow-webgl-bsv`  
Baseline Unity version: `2023.2.3f1`

Status legend:

- `RED`: absent, failing, or not yet verified
- `GREEN`: implemented and verified by automated test/build
- `REVIEW`: implementation exists but requires human or physical-device validation
- `DEFERRED`: intentionally outside the current phase

| ID | Requirement | Source | Acceptance criteria | Verification | Status |
|---|---|---|---|---|---|
| PB-WEBGL-001 | Establish a reproducible Unity WebGL build | User request | GitHub Actions builds `StageSelect` and `StageBase` with Unity 2023.2.3f1 and uploads `index.html`, `.wasm`, `.data`, and loader/framework files | Manual GitHub Actions run | RED — workflow added, not run |
| PB-WEBGL-002 | Reuse the existing Unity Actions credentials without exposing them | User decision | Workflow references `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD`; no secret value appears in source or logs | Secret-presence preflight + review | RED — repository availability not yet run |
| PB-WEBGL-003 | Do not depend on runtime Addressables delivery | User correction | Released game performs no Addressables catalog or AssetBundle request after page load | Source scan + browser network capture | RED |
| PB-WEBGL-004 | Package all playable stages in the immutable WebGL release | User correction / BSV distribution | Selecting any included stage works using only files in the release artifact | Stage-by-stage browser smoke | RED |
| PB-WEBGL-005 | Preserve existing game progression and stage behavior | Existing game | Stage selection, arrow counts, clear rules, stars, unlock state, and save data remain functional | Regression playtest | RED |
| PB-WEBGL-006 | Support mobile landscape WebGL | User request | Android Chrome and iPhone Safari load the game in landscape without cropped essential UI | Physical-device matrix | RED |
| PB-WEBGL-007 | Prevent browser gesture/input conflicts | Derived mobile requirement | Page scrolling, pinch zoom, context menu, touch cancellation, focus loss, and rotation do not leave aiming/camera input stuck | Browser smoke + device test | RED |
| PB-WEBGL-008 | Use a low-load mobile graphics profile | User request | Mobile defaults disable expensive shadows/effects, cap device pixel ratio, and maintain a measured playable frame rate | Profiler/build review + device test | RED |
| PB-WEBGL-009 | Produce BSV/static-host-compatible output | User request | Build does not require server-side compression headers and uses content-address-friendly hashed filenames | Build artifact inspection | RED — build policy added, not run |
| PB-WEBGL-010 | Record size and memory evidence | Engineering requirement | Build report records total size; largest assets and peak memory are measured before release approval | Build report + profiler | RED |
| PB-WEBGL-011 | Do not modify `main` before validation | Safety decision | Changes remain on a dedicated branch and PR stays unmerged until CI is green | GitHub branch/PR state | GREEN |
| PB-WEBGL-012 | Do not claim smartphone completion from CI alone | Verification rule | Android and iPhone physical-device checks remain open after successful CI | Release review | GREEN — policy documented |

## TDD / validation order

1. **RED:** Run the baseline WebGL workflow and capture the first compile/build failure.
2. **GREEN:** Fix only the dependency, compiler, or build-configuration failures required to produce the baseline artifact.
3. **RED:** Add a repository check that detects runtime Addressables API usage and confirm the current implementation fails it.
4. **GREEN:** Replace Addressables-backed stage references with direct build-contained stage data/prefabs and remove the package dependency.
5. **REFACTOR:** Consolidate stage loading, unloading, and save compatibility while keeping all stage behavior unchanged.
6. Add responsive mobile template and lifecycle-safe touch input tests.
7. Build and inspect the final artifact.
8. Run desktop, Android Chrome, and iPhone Safari acceptance tests.

## Out of scope for the first build gate

- Bitcoin SV payment, wallet, NFT, or minting implementation
- Uploading the WebGL build on-chain before size/runtime validation
- Content redesign or new stages
- Upgrading the project to Unity 6 before the 2023.2.3f1 baseline result is known

## Current verification classification

- Repository static review: completed
- CI configuration: implemented, unexecuted
- Unity compilation: unverified
- WebGL build: unverified
- Runtime: unverified
- Mobile hardware: unverified
- Production / BSV distribution: unverified
