using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFog: MonoBehaviour
{
    public Material material;
    private Camera camera;
    public Color fogColor;
    public float end;
    public float start;
    public float gradientDistance;
    public Texture2D fogGradient;
    public bool useGradient;
    public float clippingFar;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //clippingFar = camera.farClipPlane;
        float intensity = 1f - Mathf.Clamp(Vector3.Dot(gameObject.transform.forward, Vector3.up), 0, 1) * 0.3f;
        //intensity = 1;
        material.SetColor("_FogColor", fogColor);
        material.SetFloat("_End", end);
        material.SetFloat("_GradientDistance", gradientDistance);
        material.SetFloat("_Start", start);
        material.SetFloat("_ClippingFar", clippingFar);
        material.SetTexture("_FogGradient", fogGradient);
        if (useGradient)
            material.SetFloat("_UseGradient", 1);
        else
            material.SetFloat("_UseGradient", 0);
        material.SetFloat("_Intensity", intensity);
        material.SetTexture("_MainTexture", source);
        Graphics.Blit(source, destination, material);
    }
}
