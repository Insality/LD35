using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("InsalityEffects/Camera/ScreenShineEffect")]
public class ScreenShineEffect : ImageEffectBase
{
    public float intensity = 0.1f;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetFloat("_Intensity", intensity);	
        Graphics.Blit(source, destination, material);
    }
}
