// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Maria/Standard"
{
	Properties
	{
		_Color("Color", Color) = (0,0.9999995,1,1)
		_SoftLight("Soft Light", Range( 0 , 1)) = 0
		_Reflections("Reflections", Range( 0 , 1)) = 1
		_Spirit("Spirit", Range( 0 , 1)) = 0
		_Specular("Specular", Range( 0 , 1)) = 0
		_MainTexture("Main Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _Specular;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float4 _Color;
		uniform float _Reflections;
		uniform float _SoftLight;
		uniform float _Spirit;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float temp_output_3_0_g160 = 0.3;
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 temp_output_35_0_g157 = ( tex2D( _MainTexture, uv_MainTexture ) * _Color );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult12_g157 = dot( ase_worldNormal , ase_worldlightDir );
			Unity_GlossyEnvironmentData g65_g157 = UnityGlossyEnvironmentSetup( 0.5, data.worldViewDir, ase_worldNormal, float3(0,0,0));
			float3 indirectSpecular65_g157 = UnityGI_IndirectSpecular( data, 1.0, ase_worldNormal, g65_g157 );
			float4 temp_output_66_0_g157 = ( ( ( unity_AmbientSky * ( 1.0 - temp_output_3_0_g160 ) ) + ( temp_output_35_0_g157 * temp_output_3_0_g160 ) ) + float4( ( saturate( ( ( dotResult12_g157 * -1.0 * _Reflections ) - 0.1 ) ) * indirectSpecular65_g157 ) , 0.0 ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_103_0_g157 = saturate( ( 0.3 + step( 0.4 , ase_lightAtten ) ) );
			float temp_output_29_0_g157 = ( step( 0.4 , ( dotResult12_g157 * ase_lightColor.a ) ) * ase_lightColor.a * temp_output_103_0_g157 );
			float temp_output_3_0_g162 = temp_output_29_0_g157;
			float lerpResult78_g157 = lerp( 0.0 , dotResult12_g157 , _SoftLight);
			float temp_output_3_0_g161 = lerpResult78_g157;
			float temp_output_3_0_g159 = 0.5;
			float4 break107_g157 = temp_output_35_0_g157;
			float temp_output_109_0_g157 = ( ( break107_g157.r + break107_g157.g + break107_g157.b ) / 3.0 );
			float4 appendResult106_g157 = (float4(temp_output_109_0_g157 , temp_output_109_0_g157 , temp_output_109_0_g157 , break107_g157.a));
			float4 lerpResult110_g157 = lerp( ( ( temp_output_66_0_g157 * ( 1.0 - temp_output_3_0_g162 ) ) + ( ( ( temp_output_35_0_g157 * ( 1.0 - temp_output_3_0_g161 ) ) + ( ( ( temp_output_35_0_g157 * ( 1.0 - temp_output_3_0_g159 ) ) + float4( ( ase_lightColor.rgb * temp_output_3_0_g159 ) , 0.0 ) ) * temp_output_3_0_g161 ) ) * temp_output_3_0_g162 ) ) , ( float4(0.4980392,0.945098,1,1) * appendResult106_g157 ) , _Spirit);
			c.rgb = lerpResult110_g157.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult7_g157 = normalize( ( ase_worldlightDir + i.viewDir ) );
			float3 ase_worldNormal = i.worldNormal;
			float dotResult13_g157 = dot( normalizeResult7_g157 , ase_worldNormal );
			o.Emission = ( ase_lightColor * step( 0.5 , ( ase_lightColor.a * pow( dotResult13_g157 , 6.0 ) ) ) * _Specular ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
464;100;977;585;1963.68;528.4632;2.202301;True;False
Node;AmplifyShaderEditor.CommentaryNode;182;-1019.064,-192.8541;Float;False;489.5337;430.9799;;3;90;103;4;Diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;4;-966.4583,50.25008;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0,0.9999995,1,1;0,0.9999995,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;103;-998.8232,-144.1742;Float;True;Property;_MainTexture;Main Texture;8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-674.0328,27.22261;Float;False;2;2;0;COLOR;1,1,1,1;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;289;-479.6808,29.00955;Float;False;Maria Lighting;1;;157;6b96c0e97be1c8447840c2b26385239a;0;1;35;COLOR;1,1,1,1;False;2;COLOR;34;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-207.8213,-108.4133;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Maria/Standard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;90;0;103;0
WireConnection;90;1;4;0
WireConnection;289;35;90;0
WireConnection;0;2;289;34
WireConnection;0;13;289;0
ASEEND*/
//CHKSM=5CAB3A5EEB78B437766C17E18BDC5ABB185FA7F1