using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ProjectBow.Editor
{
    /// <summary>
    /// Deterministic WebGL build entry point used by local development and GitHub Actions.
    /// This phase only establishes a reproducible compile/build baseline. Addressables
    /// removal and monolithic stage packaging are tracked separately in the WebGL RTM.
    /// </summary>
    public static class ProjectBowWebGLBuild
    {
        private const string DefaultOutputPath = "build/WebGL";

        [MenuItem("ProjectBow/Build/WebGL CI")]
        public static void BuildWebGL()
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled && File.Exists(scene.path))
                .Select(scene => scene.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                throw new InvalidOperationException(
                    "ProjectBow WebGL build aborted: no enabled build scenes were found.");
            }

            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL, BuildTarget.WebGL))
            {
                throw new InvalidOperationException(
                    "ProjectBow WebGL build aborted: the WebGL build support module is unavailable.");
            }

            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(
                    BuildTargetGroup.WebGL,
                    BuildTarget.WebGL))
            {
                throw new InvalidOperationException(
                    "ProjectBow WebGL build aborted: Unity could not switch to WebGL.");
            }

            ConfigurePlayerSettings();

            string outputPath = Environment.GetEnvironmentVariable("BUILD_PATH");
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                outputPath = DefaultOutputPath;
            }

            outputPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(outputPath);

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outputPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.CleanBuildCache | BuildOptions.StrictMode
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            if (summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"ProjectBow WebGL build failed: result={summary.result}, " +
                    $"errors={summary.totalErrors}, warnings={summary.totalWarnings}");
            }

            Debug.Log(
                $"ProjectBow WebGL build complete: {outputPath} " +
                $"({summary.totalSize} bytes, warnings={summary.totalWarnings})");
        }

        private static void ConfigurePlayerSettings()
        {
            PlayerSettings.companyName = "G925 INTERACTIVE";
            PlayerSettings.productName = "ProjectBow";
            PlayerSettings.runInBackground = false;
            PlayerSettings.captureSingleScreen = true;

            // Disabled compression avoids requiring server-specific Content-Encoding
            // headers. This is the safest baseline for immutable BSV/static hosting.
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.WebGL.nameFilesAsHashes = true;

#pragma warning disable CS0618
            PlayerSettings.SetScriptingBackend(
                BuildTargetGroup.WebGL,
                ScriptingImplementation.IL2CPP);
#pragma warning restore CS0618
        }
    }
}
