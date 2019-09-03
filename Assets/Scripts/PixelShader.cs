using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class PixelShader : MonoBehaviour
{
    public Material material;
    public int pixelDensity = 80;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Vector2 aspectRatioData;
        if (Screen.height > Screen.width)
            aspectRatioData = new Vector2((float)Screen.width / Screen.height, 1);
        else
            aspectRatioData = new Vector2(1, (float)Screen.height / Screen.width);
        material.SetVector("_AspectRatioMultiplier", aspectRatioData);
        material.SetInt("_PixelDensity", pixelDensity);
        Graphics.Blit(source, destination, material);
    }
}