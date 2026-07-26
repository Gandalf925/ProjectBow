# ProjectBow Portrait WebGL — Issue Register

| ID | Priority | State | Summary | Next verification |
|---|---:|---|---|---|
| PB-ISSUE-001 | High | Source corrected; test pending | Camera gestures could begin over UI. | Compile and test touch controls. |
| PB-ISSUE-002 | High | Source corrected; test pending | Bow UI detection did not use the touch finger ID. | Compile and test every HUD control. |
| PB-ISSUE-003 | High | Source corrected; test pending | Canceled input could leave aim state active. | Cancel a touch and background the browser while aiming. |
| PB-ISSUE-004 | High | Source corrected; size test pending | Stage Addressables were packed together. | Compare generated bundle sizes. |
| PB-ISSUE-005 | Medium | Source corrected; test pending | CloudManager created the configured cloud population twice. | Inspect the aircraft stage. |
| PB-ISSUE-006 | Critical | Open | Unity import and compile have not run in this migration. | Open the branch in Unity 2023.2.3f1. |
| PB-ISSUE-007 | Critical | Open | Licensed Asset Store dependencies may need local restoration. | Restore packages on the licensed development machine. |
| PB-ISSUE-008 | Critical | Open | No WebGL artifact has been produced yet. | Run the ProjectBow portrait WebGL build command. |
| PB-ISSUE-009 | Critical | Open | Android Chrome and iPhone Safari are untested. | Run the device acceptance matrix. |
| PB-ISSUE-010 | High | Open historical finding | Correct-target stages may have target-list, clone-name or component-selection defects. | Reproduce Easy-13 after compilation. |
| PB-ISSUE-011 | High | Open | Repeated stage transitions may retain assets and grow WebGL memory. | Profile at least three sequential stages. |
| PB-ISSUE-012 | Medium | Open | Existing UI anchors may not all fit narrow portrait safe areas. | Inspect StageSelect and StageBase at representative portrait resolutions. |

A source correction is not classified as runtime verified until its Unity or device acceptance test passes.
