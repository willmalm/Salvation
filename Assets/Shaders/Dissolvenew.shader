// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveNew"
{
	Properties
	{
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( 0 , 1)) = 1
		_GradientTexture("Gradient Texture", 2D) = "white" {}
		_MainTexture("Main Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "SubShader 0 Pass 0"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform sampler2D _GradientTexture;
			uniform float _Dissolve;
			uniform sampler2D _NoiseTexture;
			uniform float4 _NoiseTexture_ST;
			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				fixed4 finalColor;
				float2 uv_NoiseTexture = i.ase_texcoord.xy * _NoiseTexture_ST.xy + _NoiseTexture_ST.zw;
				float temp_output_4_0 = ( (-0.66 + (( 1.0 - _Dissolve ) - 0.0) * (0.65 - -0.66) / (1.0 - 0.0)) + tex2D( _NoiseTexture, uv_NoiseTexture ).r );
				float temp_output_10_0 = (-3.0 + (temp_output_4_0 - 0.0) * (3.0 - -3.0) / (1.0 - 0.0));
				float clampResult15 = clamp( temp_output_10_0 , 0.01 , 0.99 );
				float2 temp_cast_0 = (( 1.0 - clampResult15 )).xx;
				float4 tex2DNode11 = tex2D( _GradientTexture, temp_cast_0 );
				float2 uv_MainTexture = i.ase_texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode13 = tex2D( _MainTexture, uv_MainTexture );
				float4 lerpResult19 = lerp( tex2DNode11 , tex2DNode13 , clampResult15);
				float4 break14 = lerpResult19;
				float4 appendResult7 = (float4(break14.r , break14.g , break14.b , step( 0.5 , temp_output_4_0 )));
				
				
				finalColor = appendResult7;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15600
630;100;874;554;1873.601;282.9734;1.6;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1387,-5.428161;Float;False;Property;_Dissolve;Dissolve;1;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;6;-1099.164,-0.8114109;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1000.598,186.5588;Float;True;Property;_NoiseTexture;Noise Texture;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;2;-897.3133,0.9398479;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.66;False;4;FLOAT;0.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-617.803,62.80039;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;10;-350.883,-133.3678;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-3;False;4;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;15;-60.97169,-207.4173;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.01;False;2;FLOAT;0.99;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;100.9655,-207.555;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;280.3428,-236.9407;Float;True;Property;_GradientTexture;Gradient Texture;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;54.78978,-652.176;Float;True;Property;_MainTexture;Main Texture;3;0;Create;True;0;0;False;0;84508b93f15f2b64386ec07486afc7a3;84508b93f15f2b64386ec07486afc7a3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;669.1848,-239.1252;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-228.8949,140.6235;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;8;9.075031,42.91472;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;14;898.4178,-256.4055;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;18;-17.36275,-401.053;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;1175.34,-245.9558;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;712.7845,-400.7386;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;392.8396,-412.2611;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1315.466,-246.4291;Float;False;True;2;Float;ASEMaterialInspector;0;1;DissolveNew;0770190933193b94aaa3065e307002fa;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent;Queue=Transparent;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;6;0;3;0
WireConnection;2;0;6;0
WireConnection;4;0;2;0
WireConnection;4;1;1;1
WireConnection;10;0;4;0
WireConnection;15;0;10;0
WireConnection;16;0;15;0
WireConnection;11;1;16;0
WireConnection;19;0;11;0
WireConnection;19;1;13;0
WireConnection;19;2;15;0
WireConnection;8;0;9;0
WireConnection;8;1;4;0
WireConnection;14;0;19;0
WireConnection;18;0;10;0
WireConnection;7;0;14;0
WireConnection;7;1;14;1
WireConnection;7;2;14;2
WireConnection;7;3;8;0
WireConnection;12;0;17;0
WireConnection;12;1;11;0
WireConnection;17;0;13;0
WireConnection;17;1;4;0
WireConnection;0;0;7;0
ASEEND*/
//CHKSM=DAB7D549DBFDD1C130528C3D15CE97A3CCF58F84