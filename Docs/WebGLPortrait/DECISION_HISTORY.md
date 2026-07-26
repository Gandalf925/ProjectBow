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
- Status: Current
- Decision: Addressable bundle groups use `Pack Separately` so one selected stage does not require the entire stage group to be loaded as one bundle.

## PB-DEC-007 — Performance baseline

- Date: 2026-07-26
- Status: Current provisional until device profiling
- Decision: Start mobile WebGL at 30 fps, Performant quality, DPR maximum 1.5, WebGL threads disabled, 128 MiB initial memory and 1024 MiB maximum memory. Change these only from profiler and device evidence.
