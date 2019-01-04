using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bloom : MonoBehaviour
{
    public Material material;
    private Camera camera;

    public int size = 5;
    public bool recalculate = true;

    public float[] kernel;


    void Start()
    {
        kernel = new float[size * size];
        camera = GetComponent<Camera>();
        recalculate = true;
    }

    float getGaussian(Vector2 pos, float deviation)
    {

        //return (1 / sqrt(2 * 3.14159 * pow(deviation, 2))) * exp(-pow(x, 2) / (2 * pow(deviation, 2)));


        float power = -(Mathf.Pow(pos.x, 2f) + Mathf.Pow(pos.y, 2f)) / (2f * Mathf.Pow(deviation, 2f));
        return Mathf.Exp(power) / (2f * Mathf.PI * Mathf.Pow(deviation, 2f));
    }

    void CalculateKernel()
    {
        kernel = new float[size * size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                kernel[i + j * size] = getGaussian(new Vector2(i, j), (size) / 3f);
            }
        }
        recalculate = false;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (recalculate)
            CalculateKernel();

        material.SetTexture("_MainTexture", source);
        material.SetFloatArray("_Kernel", kernel);
        material.SetFloat("_Size", size);
        Graphics.Blit(source, destination, material);

    //    Vector2 kernel[5][5] = {
				//	{0.003765f,	0.015019f,	0.023792f,	0.015019f,	0.003765f},
				//	{0.015019f,	0.059912f,	0.094907f,	0.059912f,	0.015019f},
				//	{0.023792f,	0.094907f,	0.150342f,	0.094907f,	0.023792f},
				//	{0.015019f,	0.059912f,	0.094907f,	0.059912f,	0.015019f},
				//	{0.003765f,	0.015019f,	0.023792f,	0.015019f,	0.003765f}
				//};
    }
}