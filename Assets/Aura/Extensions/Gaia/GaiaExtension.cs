///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                                                                                                                                             ///
///     MIT License                                                                                                                                             ///
///                                                                                                                                                             ///
///     Copyright (c) 2016 Raphaël Ernaelsten (@RaphErnaelsten)                                                                                                 ///
///                                                                                                                                                             ///
///     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),      ///
///     to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,                  ///
///     and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:              ///
///                                                                                                                                                             ///
///     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                          ///
///                                                                                                                                                             ///
///     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     ///
///     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER      ///
///     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS    ///
///     IN THE SOFTWARE.                                                                                                                                        ///
///                                                                                                                                                             ///
///     PLEASE CONSIDER CREDITING AURA IN YOUR PROJECTS. IF RELEVANT, USE THE UNMODIFIED LOGO PROVIDED IN THE "LICENSE" FOLDER.                                 ///
///                                                                                                                                                             ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#if GAIA_PRESENT && UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using AuraAPI;
using System.Linq;

namespace Gaia.GX.RaphaelErnaelsten
{
    /// <summary>
    /// Extension to add Aura to Gaia terrains
    /// </summary>
    public class GaiaExtension : MonoBehaviour
    {
        private static readonly Vector3Int veryLowQualityResolution = new Vector3Int(40, 23, 16);
        private static readonly Vector3Int lowQualityResolution = new Vector3Int(80, 45, 32);
        private static readonly Vector3Int mediumQualityResolution = lowQualityResolution * 2;
        private static readonly Vector3Int highQualityResolution = mediumQualityResolution * 2;
        private static readonly Vector3Int ultraHighQualityResolution = highQualityResolution * 2;

#region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Raphael Ernaelsten (@RaphErnaelsten)";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Aura - Volumetric Lighting";
        }

#endregion

#region Methods exposed by Gaia as buttons must be prefixed with GX_  
#region Presets
        public static void GX_Presets_Dawn()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.settings.density = 0.1f;
            mainComponent.frustum.settings.anisotropy = 0.75f;
            mainComponent.frustum.settings.color = Color.HSVToRGB(0.0777f, 0.50f, 1);
            mainComponent.frustum.settings.colorStrength = 0.25f;

            AuraLight[] lights = SetupLights();
            AuraLight[] directionalLights = SortOutLightsByType(lights, LightType.Directional);
            if (directionalLights.Length == 0)
            {
                directionalLights = new AuraLight[1];
                directionalLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 10.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.0777f, 0.92f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.0f;

                directionalLights[i].strength = 1.0f;
                directionalLights[i].enableOutOfPhaseColor = true;
                directionalLights[i].outOfPhaseColor = Color.HSVToRGB(0.025f, 0.6f, 1);
                directionalLights[i].outOfPhaseColorStrength = 0.5f;
            }

            AuraVolume[] auraVolumes = GetVolumes();
            DisableActiveAuraVolumes(auraVolumes);
            AuraVolume globalVolume = AuraVolume.CreateGameObject("Aura Global Volume", VolumeTypeEnum.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.transform.scale = Vector3.one * 5.0f;
            globalVolume.density.injectionParameters.enable = true;
            globalVolume.density.injectionParameters.strength = 0.1f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.contrast = 5.0f;
            globalVolume.color.injectionParameters.enable = false;
            globalVolume.anisotropy.injectionParameters.enable = true;
            globalVolume.anisotropy.injectionParameters.strength = 0.1f;
            globalVolume.anisotropy.injectionParameters.noiseMaskLevelParameters.contrast = 3.0f;
            globalVolume.anisotropy.injectionParameters.noiseMaskLevelParameters.outputLowValue = -1.0f;
        }

        public static void GX_Presets_SunnyDay()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.settings.density = 0.02f;
            mainComponent.frustum.settings.anisotropy = 0.7f;
            mainComponent.frustum.settings.color = Color.HSVToRGB(0.55f, 0.30f, 1);
            mainComponent.frustum.settings.colorStrength = 0.15f;

            AuraLight[] lights = SetupLights();
            AuraLight[] directionalLights = SortOutLightsByType(lights, LightType.Directional);
            if (directionalLights.Length == 0)
            {
                directionalLights = new AuraLight[1];
                directionalLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.12f, 0.35f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.4f;

                directionalLights[i].strength = 2.5f;
                directionalLights[i].enableOutOfPhaseColor = true;
                directionalLights[i].outOfPhaseColor = Color.HSVToRGB(0.55f, 0.6f, 1);
                directionalLights[i].outOfPhaseColorStrength = 1.5f;
            }

            AuraVolume[] auraVolumes = GetVolumes();
            DisableActiveAuraVolumes(auraVolumes);
        }

        public static void GX_Presets_RainyDay()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.settings.density = 0.5f;
            mainComponent.frustum.settings.anisotropy = 0.75f;
            mainComponent.frustum.settings.color = Color.HSVToRGB(0.533f, 0.1f, 1);
            mainComponent.frustum.settings.colorStrength = 0.1f;

            AuraLight[] lights = SetupLights();
            AuraLight[] directionalLights = SortOutLightsByType(lights, LightType.Directional);
            if (directionalLights.Length == 0)
            {
                directionalLights = new AuraLight[1];
                directionalLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.27f, 0.15f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 0.8f;

                directionalLights[i].strength = 0.5f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            AuraVolume[] auraVolumes = GetVolumes();
            DisableActiveAuraVolumes(auraVolumes);
            AuraVolume globalVolume = AuraVolume.CreateGameObject("Aura Global Volume", VolumeTypeEnum.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.speed = 0.15f;
            globalVolume.noiseMask.transform.scale = Vector3.one * 3.0f;
            globalVolume.density.injectionParameters.enable = true;
            globalVolume.density.injectionParameters.strength = 0.2f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.contrast = 15.0f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.outputLowValue = 0.0f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.outputHiValue = -1.0f;
            globalVolume.color.injectionParameters.enable = false;
            globalVolume.anisotropy.injectionParameters.enable = false;
        }

        public static void GX_Presets_Sunset()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.settings.density = 0.05f;
            mainComponent.frustum.settings.anisotropy = 0.6f;
            mainComponent.frustum.settings.color = Color.HSVToRGB(0.05f, 0.50f, 1);
            mainComponent.frustum.settings.colorStrength = 0.15f;

            AuraLight[] lights = SetupLights();
            AuraLight[] directionalLights = SortOutLightsByType(lights, LightType.Directional);
            if (directionalLights.Length == 0)
            {
                directionalLights = new AuraLight[1];
                directionalLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 10.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.036f, 0.92f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.4f;

                directionalLights[i].strength = 1.0f;
                directionalLights[i].enableOutOfPhaseColor = true;
                directionalLights[i].outOfPhaseColor = Color.HSVToRGB(0.025f, 0.77f, 1);
                directionalLights[i].outOfPhaseColorStrength = 0.5f;
            }

            AuraVolume[] auraVolumes = GetVolumes();
            DisableActiveAuraVolumes(auraVolumes);
        }

        public static void GX_Presets_Forest()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.settings.density = 0.25f;
            mainComponent.frustum.settings.anisotropy = 0.6f;
            mainComponent.frustum.settings.color = Color.HSVToRGB(0.2f, 0.4f, 1);
            mainComponent.frustum.settings.colorStrength = 0.1f;

            AuraLight[] lights = SetupLights();
            AuraLight[] directionalLights = SortOutLightsByType(lights, LightType.Directional);
            if (directionalLights.Length == 0)
            {
                directionalLights = new AuraLight[1];
                directionalLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.12f, 0.35f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.0f;

                directionalLights[i].strength = 0.7f;
                directionalLights[i].enableOutOfPhaseColor = true;
                directionalLights[i].outOfPhaseColor = Color.HSVToRGB(0.32f, 0.56f, 1);
                directionalLights[i].outOfPhaseColorStrength = 0.1f;
            }

            AuraVolume[] auraVolumes = GetVolumes();
            DisableActiveAuraVolumes(auraVolumes);
            AuraVolume globalVolume = AuraVolume.CreateGameObject("Aura Global Volume", VolumeTypeEnum.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.speed = 0.15f;
            globalVolume.noiseMask.transform.scale = Vector3.one * 3.0f;
            globalVolume.density.injectionParameters.enable = true;
            globalVolume.density.injectionParameters.strength = 0.1f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.contrast = 15.0f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.outputLowValue = 0.0f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.outputHiValue = -1.0f;
            globalVolume.color.injectionParameters.enable = false;
            globalVolume.anisotropy.injectionParameters.enable = false;
        }

        public static void GX_Presets_Desert()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.settings.density = 0.35f;
            mainComponent.frustum.settings.anisotropy = 0.15f;
            mainComponent.frustum.settings.color = Color.HSVToRGB(0.122f, 0.50f, 1);
            mainComponent.frustum.settings.colorStrength = 0.2f;

            AuraLight[] lights = SetupLights();
            AuraLight[] directionalLights = SortOutLightsByType(lights, LightType.Directional);
            if (directionalLights.Length == 0)
            {
                directionalLights = new AuraLight[1];
                directionalLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.09f, 0.5f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.4f;

                directionalLights[i].strength = 0.7f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            AuraVolume[] auraVolumes = GetVolumes();
            DisableActiveAuraVolumes(auraVolumes);
            AuraVolume globalVolume = AuraVolume.CreateGameObject("Aura Global Volume", VolumeTypeEnum.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.speed = 0.15f;
            globalVolume.noiseMask.transform.scale = Vector3.one * 3.0f;
            globalVolume.density.injectionParameters.enable = true;
            globalVolume.density.injectionParameters.strength = 0.2f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.contrast = 15.0f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.outputLowValue = 0.0f;
            globalVolume.density.injectionParameters.noiseMaskLevelParameters.outputHiValue = -1.0f;
            globalVolume.color.injectionParameters.enable = false;
            globalVolume.anisotropy.injectionParameters.enable = false;
        }

        public static void GX_Presets_RemoveAuraComponents()
        {
            AuraVolume[] auraVolumes = FindObjectsOfType<AuraVolume>();
            for (int i = 0; i < auraVolumes.Length; ++i)
            {
                DestroyImmediate(auraVolumes[i]);
            }

            AuraLight[] auraLights = FindObjectsOfType<AuraLight>();
            for (int i = 0; i < auraLights.Length; ++i)
            {
                DestroyImmediate(auraLights[i]);
            }

            Aura[] auraComponents = FindObjectsOfType<Aura>();
            for (int i = 0; i < auraComponents.Length; ++i)
            {
                DestroyImmediate(auraComponents[i]);
            }
        }
#endregion

#region Quality
        public static void GX_Quality_SetVeryLowQuality()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.SetResolution(veryLowQualityResolution);
        }

        public static void GX_Quality_SetLowQuality()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.SetResolution(lowQualityResolution);
        }

        public static void GX_Quality_SetMediumQuality()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.SetResolution(mediumQualityResolution);
        }

        public static void GX_Quality_SetHighQuality()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.SetResolution(highQualityResolution);
        }

        public static void GX_Quality_SetUltraHighQuality()
        {
            Aura mainComponent = SetupAura();
            mainComponent.frustum.SetResolution(ultraHighQualityResolution);
        }
#endregion

#region More Information
        public static void GX_MoreInformation_AboutAura()
        {
            EditorUtility.DisplayDialog("About Aura", "Aura is an open source volumetric lighting solution for Unity. Aura simulates the scattering of the light in the environmental medium and the illumination of micro-particles that are present in this environment but invisible to the eye/camera. This phoenomenon is commonly known as \"volumetric fog\".", "OK");
        }

        public static void GX_MoreInformation_Twitter()
        {
            Application.OpenURL("https://twitter.com/RaphErnaelsten");
        }

        public static void GX_MoreInformation_VisitAuraOnGithub()
        {
            Application.OpenURL("https://github.com/raphael-ernaelsten/Aura");
        }

        public static void GX_MoreInformation_VisitAuraOnTheAssetStore()
        {
            Application.OpenURL("http://u3d.as/16gj");
        }
#endregion

#region Functions
        private static AuraLight[] SetupLights()
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            AuraLight[] auraLights = new AuraLight[lights.Length];
            for (int i = 0; i < lights.Length; ++i)
            {
                AuraLight auraLight = lights[i].gameObject.GetComponent<AuraLight>();
                if (auraLight == null)
                {
                    auraLight = lights[i].gameObject.AddComponent<AuraLight>();
                }

                auraLights[i] = auraLight;
            }

            return auraLights;
        }

        public static AuraLight[] SortOutLightsByType(AuraLight[] auraLights, LightType type)
        {
            return auraLights.Where(x => x.Type == type).ToArray();
        }

        private static AuraVolume[] GetVolumes()
        {
            AuraVolume[] auraVolumes = GameObject.FindObjectsOfType<AuraVolume>();

            return auraVolumes;
        }

        private static void DisableActiveAuraVolumes(AuraVolume[] auraVolumes)
        {
            for (int i = 0; i < auraVolumes.Length; ++i)
            {
                if (auraVolumes[i].gameObject.activeInHierarchy)
                {
                    auraVolumes[i].gameObject.SetActive(false);
                    Debug.LogWarning("The AuraVolume's gameObject named \"" + auraVolumes[i].gameObject.name + "\" has been disabled to achieve the Preset's goal.", auraVolumes[i]);
                }
            }
        }

        private static Aura SetupAura()
        {
            Aura mainComponent = FindObjectOfType<Aura>();
            if (mainComponent == null)
            {
                Camera camera = Camera.main;
                if (camera == null)
                {
                    camera = FindObjectOfType<Camera>();
                }
                if (camera == null)
                {
                    camera = new GameObject("Main Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
                }

                mainComponent = camera.gameObject.AddComponent<Aura>();
                mainComponent.frustum.SetResolution(highQualityResolution);
            }

            mainComponent.frustum.settings.occlusionCullingAccuracy = OcclusionCullingAccuracyEnum.Highest; // Because of the trees' leaves

            return mainComponent;
        }
#endregion
#endregion

#region Helper methods

        /// <summary>
        /// Get the asset path of the first thing that matches the name
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns></returns>
        private static string GetAssetPath(string name)
        {
#if UNITY_EDITOR
            string[] assets = AssetDatabase.FindAssets(name, null);
            if (assets.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(assets[0]);
            }
#endif
            return null;
        }

        /// <summary>
        /// Get the asset prefab if we can find it in the project
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetAssetPrefab(string name)
        {
#if UNITY_EDITOR
            string[] assets = AssetDatabase.FindAssets(name, null);
            for (int idx = 0; idx < assets.Length; idx++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
                if (path.Contains(".prefab"))
                {
                    return AssetDatabase.LoadAssetAtPath<GameObject>(path);
                }
            }
#endif
            return null;
        }

        /// <summary>
        /// Get the range from the terrain or return a default range if no terrain
        /// </summary>
        /// <returns></returns>
        public static float GetRangeFromTerrain()
        {
            Terrain terrain = GetActiveTerrain();
            if (terrain != null)
            {
                return Mathf.Max(terrain.terrainData.size.x, terrain.terrainData.size.z) / 2f;
            }
            return 1024f;
        }

        /// <summary>
        /// Get the currently active terrain - or any terrain
        /// </summary>
        /// <returns>A terrain if there is one</returns>
        public static Terrain GetActiveTerrain()
        {
            //Grab active terrain if we can
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null && terrain.isActiveAndEnabled)
            {
                return terrain;
            }

            //Then check rest of terrains
            for (int idx = 0; idx < Terrain.activeTerrains.Length; idx++)
            {
                terrain = Terrain.activeTerrains[idx];
                if (terrain != null && terrain.isActiveAndEnabled)
                {
                    return terrain;
                }
            }
            return null;
        }

#endregion
    }
}
#endif