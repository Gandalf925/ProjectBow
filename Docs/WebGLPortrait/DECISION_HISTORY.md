# ProjectBow Portrait WebGL — Decision History

## PB-DEC-001 — Portrait orientation

- Date: 2026-07-26
- Status: Current / binding
- Authority: Explicit user correction
- Decision: ProjectBow is played in portrait orientation. Landscape recommendations from the initial static assessment are superseded.

## PB-DEC-002 — Preserve the historical game

- Date: 2026-07-26
- Status: Current
- Decision: The migration must retain the existing archery loop and stage content. WebGL work is a platform, input, loading and performance adaptation, not a game redesign.

## PB-DEC-003 — Isolated migration branch

- Date: 2026-07-26
- Status: Current
- Decision: Implement on `webgl-portrait-mobile`; do not modify `main` until a Unity build and mobile acceptance test pass.

## PB-DEC-004 — Recover baseline before engine upgrade

- Date: 2026-07-26
- Status: Current provisional engineering sequence
- Decision: First import and build using the recorded Unity `2023.2.3f1` baseline. Evaluate a Unity 6 upgrade separately after the historical baseline can be reproduced.

## PB-DEC-005 — One-finger shared gesture retained

- Date: 2026-07-26
- Status: Current
- Decision: A single touch begins bow aiming, dragging rotates the view, and releasing shoots. The migration resolves UI and pointer-ID conflicts without separating aim and fire into new controls.

## PB-DEC-006 — Stage assets stream independently

- Date: 2026-07-26
- Status: Superseded on 2026-07-27
- Historical decision: Addressable bundle groups used `Pack Separately` so one selected stage did not require the entire stage group to be loaded as one bundle.
- Superseded by: `PB-DEC-008` after the user explicitly rejected Addressables for Bitcoin SV distribution.

## PB-DEC-007 — Performance baseline

- Date: 2026-07-26
- Status: Current provisional until device profiling
- Decision: Start mobile WebGL at 30 fps, Performant quality, DPR maximum 1.5, WebGL threads disabled, 128 MiB initial memory and 1024 MiB maximum memory. Change these only from profiler and device evidence.

## PB-DEC-008 — Immutable BSV release without runtime Addressables

- Date: 2026-07-27
- Status: Current / binding
- Authority: Explicit user decision
- Decision: The final ProjectBow WebGL release must not use Addressables catalogs or runtime AssetBundle loading. All playable stages and required assets must be contained in the immutable WebGL release files and require no stage-specific network request after startup.
- Consequence: `StageData`, `StageLoader`, serialized stage references, build automation, package dependencies and static verification must be migrated away from Addressables while preserving the original stage behavior and progress data.

## PB-DEC-009 — Reuse existing Unity Actions credentials safely

- Date: 2026-07-27
- Status: Current
- Authority: Explicit user decision
- Decision: Unity CI references the previously used secret names `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD`.
- Security boundary: Secret values are not retrieved, displayed, copied between repositories, written to source, or stored in documentation. A preflight job only verifies whether the names resolve to non-empty secrets in ProjectBow.

## PB-DEC-010 — CI does not prove smartphone completion

- Date: 2026-07-27
- Status: Current / binding verification rule
- Decision: Unity import and WebGL artifact generation are automated gates only. Android Chrome and iPhone Safari physical-device testing remain required before smartphone support or release readiness is claimed.
