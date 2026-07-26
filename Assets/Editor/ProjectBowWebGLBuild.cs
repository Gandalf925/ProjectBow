#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectBowWebGLBuild
{
    private const string TemplateName = "PROJECT:ProjectBowPortrait";
    private const string BuildDirectory = "Builds/WebGLPortrait";
    private const string SessionConfigurationKey = "ProjectBow.WebGLPortrait.Configured.v2";

    static ProjectBowWebGLBuild()
    {
        EditorApplication.delayCall += ConfigureOncePerEditorSession;
    }

    [MenuItem("ProjectBow/WebGL/Configure Portrait Mobile")]
    public static void ConfigurePortraitMobile()
    {
        ApplyProjectConfiguration(true);
    }

    [MenuItem("ProjectBow/WebGL/Build Portrait WebGL")]
    public static void BuildPortraitWebGL()
    {
        ApplyProjectConfiguration(true);

        if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL))
        {
            throw new InvalidOperationException("Unity could not switch the active build target to WebGL.");
        }

        Directory.CreateDirectory(BuildDirectory);

        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            throw new InvalidOperationException("No enabled scenes were found in Editor Build Settings.");
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = BuildDirectory,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"WebGL build failed: {report.summary.result} ({report.summary.totalErrors} errors)");
        }

        Debug.Log(
            $"ProjectBow portrait WebGL build completed: {Path.GetFullPath(BuildDirectory)} " +
            $"({report.summary.totalSize} bytes)");
    }

    public static void BuildFromCommandLine()
    {
        BuildPortraitWebGL();
    }

    private static void ConfigureOncePerEditorSession()
    {
        if (SessionState.GetBool(SessionConfigurationKey, false))
        {
            return;
        }

        SessionState.SetBool(SessionConfigurationKey, true);
        ApplyProjectConfiguration(false);
    }

    private static void ApplyProjectConfiguration(bool logCompletion)
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
        PlayerSettings.allowedAutorotateToLandscapeRight = false;
        PlayerSettings.defaultWebScreenWidth = 540;
        PlayerSettings.defaultWebScreenHeight = 960;
        PlayerSettings.stripEngineCode = true;
        QualitySettings.SetQualityLevel(0, true);
        QualitySettings.vSyncCount = 0;

        SetWebGLProperty("template", TemplateName);
        SetWebGLProperty("dataCaching", true);
        SetWebGLProperty("nameFilesAsHashes", true);
        SetWebGLProperty("decompressionFallback", true);
        SetWebGLProperty("threadsSupport", false);
        SetWebGLProperty("initialMemorySize", 128);
        SetWebGLProperty("maximumMemorySize", 1024);
        SetWebGLProperty("memoryGrowthMode", "Geometric");
        SetWebGLProperty("compressionFormat", "Gzip");
        SetWebGLProperty("powerPreference", "HighPerformance");

        AssetDatabase.SaveAssets();

        if (logCompletion)
        {
            Debug.Log(
                "ProjectBow is configured for portrait mobile WebGL. " +
                "Use ProjectBow > WebGL > Build Portrait WebGL to build.");
        }
    }

    private static void SetWebGLProperty(string propertyName, object value)
    {
        PropertyInfo property = typeof(PlayerSettings.WebGL).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Static);

        if (property == null || !property.CanWrite)
        {
            Debug.LogWarning($"PlayerSettings.WebGL.{propertyName} is not available in this Unity version.");
            return;
        }

        object convertedValue;
        if (property.PropertyType.IsEnum && value is string enumName)
        {
            convertedValue = Enum.Parse(property.PropertyType, enumName, true);
        }
        else if (property.PropertyType.IsInstanceOfType(value))
        {
            convertedValue = value;
        }
        else
        {
            convertedValue = Convert.ChangeType(value, property.PropertyType);
        }

        property.SetValue(null, convertedValue);
    }
}
#endif
