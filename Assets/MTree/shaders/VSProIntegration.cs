using System.Collections;
using System.Collections.Generic;

#if VEGETATION_STUDIO_PRO
using AwesomeTechnologies.VegetationSystem;
using UnityEngine;
 
namespace AwesomeTechnologies.Shaders
{
    public class MtreeShaderController : IShaderController
    {
        public bool MatchShader(string shaderName)
        {
            return (shaderName == "Mtree/Bark" || shaderName == "Mtree/Leafs");
        }
 
        public bool MatchBillboardShader(Material[] materials)
        {
            return false;
        }
 
        public ShaderControllerSettings Settings { get; set; }
 
 
        public void CreateDefaultSettings(Material[] materials)
        {
            Settings = new ShaderControllerSettings
            {
                Heading = "Mtree settings",
                Description = "",
                LODFadePercentage = true,
                LODFadeCrossfade = true,
                SampleWind = true,
                //DynamicHUE = true,
                //BillboardHDWind = false                                              
            };
  
            Settings.AddLabelProperty("Foliage settings");
            Settings.AddColorProperty("FoliageTintColor", "Foliage tint color", "", GetLeafColor(materials));
            Settings.AddColorProperty("FoliageTranslucencyColor", "Foliage Translucency color", "",
                ShaderControllerSettings.GetColorFromMaterials(materials, "_TranslucencyColor"));

            Settings.AddLabelProperty("Bark settings");
            Settings.AddColorProperty("BarkTintColor", "Bark tint color", "", GetBarkColor(materials));
        }
       
        public void UpdateMaterial(Material material, EnvironmentSettings environmentSettings)
        {
            if (Settings == null) return;
            if (material.shader.name == "Mtree/Bark")
            {
                Color barkTintColor = Settings.GetColorPropertyValue("BarkTintColor");
                material.SetColor("_Color", barkTintColor);
            }
            if (material.shader.name == "Mtree/Leafs")
            {
                Color foliageTintColor = Settings.GetColorPropertyValue("FoliageTintColor");
                Color foliageTranslucencyColor = Settings.GetColorPropertyValue("FoliageTranslucencyColor");
                material.SetColor("_Color", foliageTintColor);
                material.SetColor("_TranslucencyColor", foliageTranslucencyColor);
                material.SetFloat("_Cutoff", material.GetFloat("_Cutoff"));

            }
         
        }
        
        Color GetLeafColor(Material[] materials)
        {
            foreach (Material mat in materials)
            {
                if (mat.shader.name == "Mtree/Leafs")
                    return mat.GetColor("_Color");
            }
            return Color.black;
        }
        Color GetBarkColor(Material[] materials)
        {
            foreach (Material mat in materials)
            {
                if (mat.shader.name == "Mtree/Bark")
                    return mat.GetColor("_Color");
            }
            return Color.black;
        }


 
        public void UpdateWind(Material material, WindSettings windSettings)
        {
           
        }
    }
}
#endif