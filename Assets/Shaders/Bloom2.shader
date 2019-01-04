Shader "Unlit/Bloom2"
{
	Properties
	{
		_MainTexture("Texture", 2D) = "white" {}
		_Samples("Samples", Int) = 20
		_Offset("Offset", Float) = 0.01
		_Size("Size", Int) = 5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
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
			float _Kernel[4];
			float _Size;
			
			v2f vert (appdata v)
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
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 color;

				float4 originalColor = tex2D(_MainTexture, i.uv);

				for (int j = -_Size+1; j < _Size; j++)
				{
					for (int k = -_Size+1; k < _Size; k++)
					{
						float2 samplePosition = i.uv + float2(j*_Offset, k*_Offset);
						float4 sampleColor = tex2D(_MainTexture, samplePosition);

						if (getIntensity(sampleColor) > 0.9f)
						{
							color += sampleColor * _Kernel[abs(j) + abs(k) * _Size];
						}
						else
						{
							color += originalColor * _Kernel[abs(j) + abs(k) * _Size];
						}
					}
				}

				/*if (getIntensity(originalColor) > 0.9f)
				{
					color = originalColor;
				}*/

				return color;
			}
			ENDCG
		}
	}
}
