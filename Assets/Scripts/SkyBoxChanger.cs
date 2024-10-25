using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SkyBoxType
{
    ColdSunset,
    ColdNight,
    CartoonBaseBlueSky,
    CartoonBaseNightSky,
    DeepDusk,
    EpicBlueSunset,
    EpicGloriousPink,
    NightMoonBurst,
    OvercastLow,
    SpaceAnotherPlanet
}

public class SkyBoxChanger : MonoBehaviour
{
    public Material ColdSunset;
    public Material ColdNight;
    public Material CartoonBaseBlueSky;
    public Material CartoonBaseNightSky;
    public Material DeepDusk;
    public Material EpicBlueSunset;
    public Material EpicGloriousPink;
    public Material NightMoonBurst;
    public Material OvercastLow;
    public Material SpaceAnotherPlanet;

    public void ChangeSkybox(SkyBoxType skyBoxType)
    {
        RenderSettings.skybox = GetSkyBoxMaterial(skyBoxType);
    }

    private Material GetSkyBoxMaterial(SkyBoxType skyBoxType)
    {
        switch (skyBoxType)
        {
            case SkyBoxType.ColdSunset:
                return ColdSunset;
            case SkyBoxType.ColdNight:
                return ColdNight;
            case SkyBoxType.CartoonBaseBlueSky:
                return CartoonBaseBlueSky;
            case SkyBoxType.CartoonBaseNightSky:
                return CartoonBaseNightSky;
            case SkyBoxType.DeepDusk:
                return DeepDusk;
            case SkyBoxType.EpicBlueSunset:
                return EpicBlueSunset;
            case SkyBoxType.EpicGloriousPink:
                return EpicGloriousPink;
            case SkyBoxType.NightMoonBurst:
                return NightMoonBurst;
            case SkyBoxType.OvercastLow:
                return OvercastLow;
            case SkyBoxType.SpaceAnotherPlanet:
                return SpaceAnotherPlanet;
            default:
                return ColdSunset;
        }
    }

    [Button("Change ColdSunset")]
    private void ChangeColdSunset()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.ColdSunset);
    }

    [Button("Change ColdNight")]
    private void ChangeColdNight()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.ColdNight);
    }

    [Button("Change CartoonBaseBlueSky")]
    private void ChangeCartoonBaseBlueSky()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.CartoonBaseBlueSky);
    }

    [Button("Change CartoonBaseNightSky")]
    private void ChangeCartoonBaseNightSky()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.CartoonBaseNightSky);
    }

    [Button("Change DeepDusk")]
    private void ChangeDeepDusk()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.DeepDusk);
    }

    [Button("Change EpicBlueSunset")]
    private void ChangeEpicBlueSunset()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.EpicBlueSunset);
    }

    [Button("Change EpicGloriousPink")]
    private void ChangeEpicGloriousPink()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.EpicGloriousPink);
    }

    [Button("Change NightMoonBurst")]
    private void ChangeNightMoonBurst()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.NightMoonBurst);
    }

    [Button("Change OvercastLow")]
    private void ChangeOvercastLow()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.OvercastLow);
    }

    [Button("Change SpaceAnotherPlanet")]
    private void ChangeSpaceAnotherPlanet()
    {
        RenderSettings.skybox = GetSkyBoxMaterial(SkyBoxType.SpaceAnotherPlanet);
    }
}