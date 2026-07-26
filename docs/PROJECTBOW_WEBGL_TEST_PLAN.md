# ProjectBow WebGL Test Plan

## Gate 1 — CI credential and compile baseline

1. Confirm `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD` are available to the repository without printing their values.
2. Import the project with Unity 2023.2.3f1.
3. Compile all runtime and editor assemblies.
4. Switch to WebGL.
5. Build enabled scenes.
6. Verify the artifact contains `index.html`, a `Build` directory, WebAssembly, data, loader, and framework files.

Expected first outcome: either a baseline artifact or a specific reproducible dependency/compiler/build failure.

## Gate 2 — Addressables removal

1. Add a source-policy check that fails while runtime code imports or invokes Addressables.
2. Replace Addressable stage references with direct build-contained references.
3. Confirm all 21 currently registered Easy-list entries remain selectable.
4. Confirm no runtime network request is made for Addressables catalogs or AssetBundles.
5. Remove the Addressables package only after source and serialized references are migrated.

## Gate 3 — Original behavior regression

For every included stage:

- stage opens
- mission text is correct
- arrow count is correct
- aiming begins and cancels correctly
- shot force presets work
- targets register hits
- clear/fail conditions work
- star calculation completes
- next-stage unlock persists
- returning to stage select works

## Gate 4 — Browser lifecycle

Desktop and emulated touch:

- initial load succeeds
- no uncaught console exception
- no page scrolling during play
- pointer/touch cancellation clears aiming and camera movement
- focus loss and visibility change clear held input
- resizing and rotation preserve usable UI

## Gate 5 — Physical devices

Android Chrome and iPhone Safari:

- landscape launch
- safe-area fit
- touch aiming and release
- UI buttons do not move the camera or fire arrows
- app switching does not leave input stuck
- acceptable loading time
- stable repeated stage changes
- no browser memory termination during the agreed test session
- acceptable frame rate and temperature

## Gate 6 — BSV/static distribution

- no runtime API or asset dependency
- all release files use relative/content-addressable references
- MIME requirements are documented
- total bytes are recorded
- largest files are recorded
- immutable release hash/transaction mapping is documented after upload
