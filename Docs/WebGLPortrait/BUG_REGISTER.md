# ProjectBow Portrait WebGL — Issue Register

| ID | Priority | State | Summary | Next verification |
|---|---:|---|---|---|
| PB-ISSUE-001 | High | Source corrected; test pending | Camera gestures could begin over UI. | Compile and test touch controls. |
| PB-ISSUE-002 | High | Source corrected; test pending | Bow UI detection did not use the touch finger ID. | Compile and test every HUD control. |
| PB-ISSUE-003 | High | Source corrected; test pending | Canceled input could leave aim state active. | Cancel a touch and background the browser while aiming. |
| PB-ISSUE-004 | High | Superseded | Stage Addressables were packed together. Pack Separately was an interim response, but runtime Addressables are now rejected for the BSV release. | Close through PB-WEB-018 monolithic migration. |
| PB-ISSUE-005 | Medium | Source corrected; test pending | CloudManager created the configured cloud population twice. | Inspect the aircraft stage. |
| PB-ISSUE-006 | Critical | RED reproduced; fixes committed; re-test pending | Licensed Unity 2023.2.3f1 import reached script compilation and failed. | Re-run CI after PB-ISSUE-013 and PB-ISSUE-014 corrections. |
| PB-ISSUE-007 | Critical | LFS checkout passed; Unity asset validation pending | Licensed Asset Store dependencies may need restoration. CI materialized the repository and LFS content, but compilation stopped before a complete asset/build validation. | Continue Unity build after compiler GREEN. |
| PB-ISSUE-008 | Critical | Open | No WebGL artifact has been produced yet. | Run the corrected ProjectBow portrait WebGL build command. |
| PB-ISSUE-009 | Critical | Open | Android Chrome and iPhone Safari are untested. | Run the device acceptance matrix. |
| PB-ISSUE-010 | High | Open historical finding | Correct-target stages may have target-list, clone-name or component-selection defects. | Reproduce Easy-13 after compilation. |
| PB-ISSUE-011 | High | Open | Repeated stage transitions may retain assets and grow WebGL memory. | Profile at least three sequential stages. |
| PB-ISSUE-012 | Medium | Open | Existing UI anchors may not all fit narrow portrait safe areas. | Inspect StageSelect and StageBase at representative portrait resolutions. |
| PB-ISSUE-013 | Critical | Source corrected; CI re-test pending | Cinemachine 2.9.7 registry source referenced the removed experimental URP `PixelPerfectCamera` namespace under Unity 2023.2/URP 16. | Resolve the pinned official `release/2.9` maintenance source and compile. |
| PB-ISSUE-014 | Critical | Source corrected; CI re-test pending | WebGL build automation referenced `AddressableAssetSettingsDefaultObject` and failed compilation. | Confirm the Addressables-free build automation compiles. |

## First licensed Unity evidence

- Date: 2026-07-27
- Host run: `Gandalf925/shardstep-unity` Actions run `30223408990`
- Secret preflight: passed without exposing values
- Git/LFS checkout: passed
- Unity version: 2023.2.3f1
- Compilation: failed with PB-ISSUE-013 and PB-ISSUE-014
- WebGL artifact: not produced
- Device/runtime status: untested

A source correction is not classified as runtime verified until its Unity or device acceptance test passes.
