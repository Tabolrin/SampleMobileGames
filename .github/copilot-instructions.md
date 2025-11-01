# COPILOT / AI contributor instructions — SampleMobileGames

This Unity project is a small collection of 2D mobile minigames. These notes focus on the concrete, discoverable patterns and workflows an AI coding agent should know before editing code.

- Unity editor version: see `ProjectSettings/ProjectVersion.txt` (currently 6000.2.8f1). Prefer edits compatible with this editor/runtime.
- Key folders:
  - `Assets/Scripts/` — game logic (no asmdef files; uses the default `Assembly-CSharp` project).
  - `Assets/Scenes/` — scene assets and entry points. Open scenes in the Editor to test changes.
  - `Packages/manifest.json` — lists packages used (notably `com.unity.inputsystem`, URP, 2D packages, and `com.unity.test-framework`).

- Project conventions and important patterns
  - Uses Unity's new Input System: `Assets/InputSystem_Actions.inputactions` and code references like `Touchscreen.current.primaryTouch.position.ReadValue()` (see `Assets/Scripts/BallHandler.cs`). When suggesting edits, account for devices where `Touchscreen.current` may be null (Editor) and prefer defensive checks.
  - No assembly-definition files: keep changes inside `Assets/Scripts/` or add a new asmdef only if you explicitly update project structure and CI (ask before adding asmdefs).
  - Code targets C# language level shown in `Assembly-CSharp.csproj` (LangVersion=9.0, TargetFramework=netstandard2.1). Prefer using C# 9 features only.

- Build & run notes (how a human/dev runs the project)
  - Typical workflow is via the Unity Editor (open the project folder). To run a headless CI build on Windows PowerShell (example):
    & 'C:\Program Files\Unity\Hub\Editor\6000.2.8f1\Editor\Unity.exe' -batchmode -projectPath 'E:\__Unity\SampleMobileGames\2D Mobile Minigames' -buildWindows64Player 'Builds\Game.exe' -quit
    (Paths may differ on the maintainer's machine; consult `Assembly-CSharp.csproj` analyzer hint paths for a likely Unity install location.)
  - There are no visible automated test assemblies out-of-the-box, though `com.unity.test-framework` is in `manifest.json` — search `Assets/Tests` if adding tests.

- Integration points & external dependencies
  - Input System (`com.unity.inputsystem`) — changes to input bindings should update `InputSystem_Actions.inputactions` and be tested via Editor Input Debug or device.
  - Universal Render Pipeline (`com.unity.render-pipelines.universal`) — rendering changes should be tested in the Editor with URP pipeline asset in `Assets/`.

- Code examples & quick fixes to watch for
  - BallHandler.cs currently reads touch input directly. Example defensive pattern to suggest:
    - Check `if (Touchscreen.current == null) return;` before reading position.
    - Use `Touchscreen.current.primaryTouch.press.isPressed` or check `primaryTouch.press.ReadValue()` for touch validity.
  - When adding new C# APIs, prefer methods compatible with Unity 6.x and netstandard2.1.

- When merging or modifying files
  - Preserve meta files (.meta) — Unity relies on them. Don't remove or reformat meta files.
  - Avoid adding large binary assets in edits. If you must, call out the change in the PR and explain why.

If anything is ambiguous in these notes or you'd like more examples (scene names, common MonoBehaviour entry points, or a short test harness), tell me which area to expand and I'll update this file.
