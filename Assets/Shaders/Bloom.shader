Shader "Unlit/Bloom"
{
	Properties
	{
		_MainTexture("Texture", 2D) = "white" {}
		_Samples("Samples", Int) = 20
		_Offset("Offset", Float) = 0.01
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
				#pragma exclude_renderers d3d11 gles
							#pragma vertex vert
							#pragma fragment frag
							// make fog work
							#pragma multi_compile_fog

							#include "UnityCG.cginc"

							struct appdata
							{
								float4 vertex : POSITION;
								float2 uv : TEXCOORD0;
							};

							struct v2f
							{
								float2 uv : TEXCOORD0;
								UNITY_FOG_COORDS(1)
								float4 vertex : SV_POSITION;
							};

							sampler2D _MainTexture;
							float4 _MainTexture_ST;
							int _Samples;
							float _Offset;
							//float _kernel[20];

							v2f vert(appdata v)
							{
								v2f o;
								o.vertex = UnityObjectToClipPos(v.vertex);
								o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
								UNITY_TRANSFER_FOG(o,o.vertex);
								return o;
							}

							float getIntensity(float4 color)
							{
								float highest = 0;
								float4 newColor = color - float4(1, 1, 1, 0);

								if (newColor.x > 0)
									highest = newColor.x;
								if (newColor.y > highest)
									highest = newColor.y;
								if (newColor.z > highest)
									highest = newColor.z;


								return (color.x + color.y + color.z) / 3;
							}

							float normpdf(float x, float sigma)
							{
								return 0.39894*exp(-0.5*x*x / (sigma*sigma)) / sigma;
							}

							float getGaussian(float2 pos, float deviation)
							{

								//return (1 / sqrt(2 * 3.14159 * pow(deviation, 2))) * exp(-pow(x, 2) / (2 * pow(deviation, 2)));


								float power = -(pow(pos.x, 2) + pow(pos.y, 2)) / (2 * pow(deviation, 2));
								return exp(power) / (2 * 3.14159 * pow(deviation, 2));
							}

							fixed4 frag(v2f i) : SV_Target
							{
								float4 color;


								for (int j = -2; j < 2; j++)
								{
									for (int k = -2; k < 2; k++)
									{
										float4 originalColor = tex2D(_MainTexture, i.uv);
										float2 samplePosition = i.uv + float2(j*_Offset, k*_Offset);
										float4 sampleColor = tex2D(_MainTexture, samplePosition);
										if (getIntensity(sampleColor) > 0.8)
										{
											color += sampleColor * kernel[j, k];//getGaussian(float2(j, k), _Samples/3);
										}
										else
										{
											color += originalColor * kernel[j, k];
										}
									}
								}

								//color = color / (_Samples*2 + 1);

								return color;
							}
							ENDCG
						}
		}
}
