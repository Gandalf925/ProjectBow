# ProjectBow WebGL Release State

Date: 2026-07-27

## Current classification

**Development branch / CI bootstrap — NO-GO for release**

## Current branch

- Branch: `agent/projectbow-webgl-bsv`
- Base: `main`
- Unity baseline: `2023.2.3f1`

## Implemented

- Dedicated branch preserving `main`
- GitHub Actions WebGL workflow
- Existing Unity credential secret names referenced without exposing values
- Secret-presence preflight before Unity activation
- Deterministic WebGL build method using enabled build scenes
- Static-host-safe uncompressed WebGL baseline policy
- RTM and decision record

## Not yet verified

- ProjectBow repository access to the Unity credential secrets
- Unity compilation
- WebGL build artifact generation
- Runtime loading of StageSelect and StageBase
- Stage content after Addressables removal
- Desktop browser execution
- Android Chrome execution
- iPhone Safari execution
- Build size and peak memory
- Bitcoin SV distribution

## Release blockers

1. First GitHub Actions run must complete or yield a reproducible failure.
2. Addressables runtime dependency must be removed.
3. All included stages must load from the immutable release files.
4. Browser and physical-device tests must pass.
5. Build size and memory must be accepted for the intended BSV distribution method.

## Go/no-go rule

Do not merge or publish as a playable release until the RTM build, packaging, runtime, mobile, and size gates are closed. CI success alone is insufficient.
