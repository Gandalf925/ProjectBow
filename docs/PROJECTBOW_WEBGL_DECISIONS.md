# ProjectBow WebGL Decision Record

## PB-D-001 — Bitcoin SV distribution must not rely on Addressables

- Date: 2026-07-27
- Status: Current / binding
- Authority: Explicit user decision
- Decision: The released ProjectBow WebGL game must not depend on Addressables catalog or AssetBundle retrieval at runtime. The final release is an immutable, self-contained set of WebGL files suitable for static or Bitcoin SV content-addressed distribution.
- Consequence: Existing `StageData` / `StageLoader` Addressables references must be replaced while preserving all stage behavior and progression.

## PB-D-002 — Preserve the original game before content changes

- Date: 2026-07-27
- Status: Current
- Decision: The first WebGL phase is a platform and packaging conversion. Stage rules, progression, aiming, shooting, stars, and unlock behavior are not redesigned unless a verified defect blocks the conversion.

## PB-D-003 — Use a dedicated branch

- Date: 2026-07-27
- Status: Current
- Decision: Work proceeds on `agent/projectbow-webgl-bsv`. `main` remains unchanged until automated WebGL build validation succeeds and the diff is reviewed.

## PB-D-004 — Reuse existing Unity Actions credential names

- Date: 2026-07-27
- Status: Current
- Authority: Explicit user decision
- Decision: ProjectBow GitHub Actions references the same secret names previously used successfully: `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD`.
- Security boundary: Secret values are never read, copied into source, printed, or stored in documentation. Availability is verified only inside GitHub Actions through empty/non-empty checks.
- Known limitation: GitHub repository secrets are commonly repository-scoped. The first manual workflow run determines whether these secrets are already available to ProjectBow; a missing-secret result is configuration evidence, not a code failure.

## PB-D-005 — Keep Unity 2023.2.3f1 for the first baseline

- Date: 2026-07-27
- Status: Current / provisional
- Decision: Match the repository's recorded Unity version for the first CI compile and WebGL build. Engine upgrade evaluation occurs only after the original baseline and dependency failures are known.

## PB-D-006 — Manual workflow execution only

- Date: 2026-07-27
- Status: Current
- Decision: Unity CI uses `workflow_dispatch` only. It must not run automatically on every push because failed activation retries can lock or throttle the Unity account and the repository is unusually large.

## PB-D-007 — CI success is not mobile completion

- Date: 2026-07-27
- Status: Current / binding verification rule
- Decision: A successful GitHub Actions build proves compilation and artifact generation only. Android Chrome and iPhone Safari physical-device validation remain mandatory before declaring smartphone support.
