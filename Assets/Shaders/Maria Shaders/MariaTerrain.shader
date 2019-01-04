// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Maria/Terrain"
{
	Properties
	{
		[HideInInspector]_Splat0("Splat0", 2D) = "white" {}
		[HideInInspector]_Splat3("Splat3", 2D) = "white" {}
		[HideInInspector]_Splat2("Splat2", 2D) = "white" {}
		[HideInInspector]_Splat1("Splat1", 2D) = "white" {}
		[HideInInspector]_Control("Control", 2D) = "white" {}
		_SoftLight("Soft Light", Range( 0 , 1)) = 0
		_Reflections("Reflections", Range( 0 , 1)) = 1
		_Spirit("Spirit", Range( 0 , 1)) = 0
		_Specular("Specular", Range( 0 , 1)) = 0
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
		uniform sampler2D _Control;
		uniform float4 _Control_ST;
		uniform sampler2D _Splat0;
		uniform float4 _Splat0_ST;
		uniform sampler2D _Splat1;
		uniform float4 _Splat1_ST;
		uniform sampler2D _Splat2;
		uniform float4 _Splat2_ST;
		uniform sampler2D _Splat3;
		uniform float4 _Splat3_ST;
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
			float temp_output_3_0_g142 = 0.3;
			float2 uv_Control = i.uv_texcoord * _Control_ST.xy + _Control_ST.zw;
			float4 tex2DNode3_g40 = tex2D( _Control, uv_Control );
			float2 uv_Splat0 = i.uv_texcoord * _Splat0_ST.xy + _Splat0_ST.zw;
			float2 uv_Splat1 = i.uv_texcoord * _Splat1_ST.xy + _Splat1_ST.zw;
			float2 uv_Splat2 = i.uv_texcoord * _Splat2_ST.xy + _Splat2_ST.zw;
			float2 uv_Splat3 = i.uv_texcoord * _Splat3_ST.xy + _Splat3_ST.zw;
			float4 temp_cast_1 = (1.0).xxxx;
			float4 clampResult30_g40 = clamp( ( ( tex2DNode3_g40.r * tex2D( _Splat0, uv_Splat0 ) ) + ( tex2DNode3_g40.g * tex2D( _Splat1, uv_Splat1 ) ) + ( tex2DNode3_g40.b * tex2D( _Splat2, uv_Splat2 ) ) + ( tex2DNode3_g40.a * tex2D( _Splat3, uv_Splat3 ) ) ) , float4( 0,0,0,0 ) , temp_cast_1 );
			float4 temp_output_35_0_g139 = clampResult30_g40;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult12_g139 = dot( ase_worldNormal , ase_worldlightDir );
			Unity_GlossyEnvironmentData g65_g139 = UnityGlossyEnvironmentSetup( 0.5, data.worldViewDir, ase_worldNormal, float3(0,0,0));
			float3 indirectSpecular65_g139 = UnityGI_IndirectSpecular( data, 1.0, ase_worldNormal, g65_g139 );
			float4 temp_output_66_0_g139 = ( ( ( unity_AmbientSky * ( 1.0 - temp_output_3_0_g142 ) ) + ( temp_output_35_0_g139 * temp_output_3_0_g142 ) ) + float4( ( saturate( ( ( dotResult12_g139 * -1.0 * _Reflections ) - 0.1 ) ) * indirectSpecular65_g139 ) , 0.0 ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_103_0_g139 = saturate( ( 0.3 + step( 0.4 , ase_lightAtten ) ) );
			float temp_output_29_0_g139 = ( step( 0.4 , ( dotResult12_g139 * ase_lightColor.a ) ) * ase_lightColor.a * temp_output_103_0_g139 );
			float temp_output_3_0_g144 = temp_output_29_0_g139;
			float lerpResult78_g139 = lerp( 0.0 , dotResult12_g139 , _SoftLight);
			float temp_output_3_0_g143 = lerpResult78_g139;
			float temp_output_3_0_g141 = 0.5;
			float4 break107_g139 = temp_output_35_0_g139;
			float temp_output_109_0_g139 = ( ( break107_g139.r + break107_g139.g + break107_g139.b ) / 3.0 );
			float4 appendResult106_g139 = (float4(temp_output_109_0_g139 , temp_output_109_0_g139 , temp_output_109_0_g139 , break107_g139.a));
			float4 lerpResult110_g139 = lerp( ( ( temp_output_66_0_g139 * ( 1.0 - temp_output_3_0_g144 ) ) + ( ( ( temp_output_35_0_g139 * ( 1.0 - temp_output_3_0_g143 ) ) + ( ( ( temp_output_35_0_g139 * ( 1.0 - temp_output_3_0_g141 ) ) + float4( ( ase_lightColor.rgb * temp_output_3_0_g141 ) , 0.0 ) ) * temp_output_3_0_g143 ) ) * temp_output_3_0_g144 ) ) , ( float4(0.4980392,0.945098,1,1) * appendResult106_g139 ) , _Spirit);
			c.rgb = lerpResult110_g139.rgb;
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
			float3 normalizeResult7_g139 = normalize( ( ase_worldlightDir + i.viewDir ) );
			float3 ase_worldNormal = i.worldNormal;
			float dotResult13_g139 = dot( normalizeResult7_g139 , ase_worldNormal );
			o.Emission = ( ase_lightColor * step( 0.5 , ( ase_lightColor.a * pow( dotResult13_g139 , 6.0 ) ) ) * _Specular ).rgb;
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
464;100;977;585;1112.204;418.0059;1.3;True;False
Node;AmplifyShaderEditor.FunctionNode;197;-674.7903,32.73339;Float;False;Terrain;0;;40;33100a7cdb988054eb9dfbd4875cd862;0;0;2;COLOR;19;COLOR;17
Node;AmplifyShaderEditor.FunctionNode;218;-479.6808,29.00955;Float;False;Maria Lighting;10;;139;6b96c0e97be1c8447840c2b26385239a;0;1;35;COLOR;1,1,1,1;False;2;COLOR;34;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-207.8213,-108.4133;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Maria/Terrain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;218;35;197;19
WireConnection;0;2;218;34
WireConnection;0;13;218;0
ASEEND*/
//CHKSM=80669C21755C4FCCBA035BD04F6D585DFE03B5D4