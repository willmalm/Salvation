// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FogEffect"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_End("End", Float) = 20
		_FogGradient("FogGradient", 2D) = "white" {}
		_Start("Start", Float) = 20
		_ClippingFar("ClippingFar", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
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
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;
			uniform sampler2D _FogGradient;
			uniform float _ClippingFar;
			uniform sampler2D _CameraDepthTexture;
			uniform float _Start;
			uniform float _End;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
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
				float2 uv_MainTexture = i.ase_texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode2 = tex2D( _MainTexture, uv_MainTexture );
				float4 screenPos = i.ase_texcoord1;
				float eyeDepth1 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( screenPos ))));
				float clampResult37 = clamp( ( eyeDepth1 - _Start ) , 0.0 , _End );
				float clampResult13 = clamp( ( clampResult37 / _End ) , 0.0 , 1.0 );
				float temp_output_36_0 = ( ( 1.0 - step( ( _ClippingFar - 1.0 ) , eyeDepth1 ) ) * clampResult13 );
				float clampResult29 = clamp( temp_output_36_0 , 0.01 , 0.99 );
				float2 temp_cast_0 = (clampResult29).xx;
				float4 tex2DNode26 = tex2D( _FogGradient, temp_cast_0 );
				float4 lerpResult89 = lerp( tex2DNode2 , tex2DNode26 , tex2DNode26.a);
				float4 temp_cast_1 = (1.0).xxxx;
				float lerpResult64 = lerp( temp_output_36_0 , 1.0 , saturate( ( tex2DNode2 - temp_cast_1 ) ).r);
				float4 lerpResult3 = lerp( tex2DNode2 , lerpResult89 , lerpResult64);
				
				
				finalColor = lerpResult3;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "3"
	
	
}
/*ASEBEGIN
Version=15600
457;100;977;587;389.1776;624.6868;2.143731;True;False
Node;AmplifyShaderEditor.ScreenDepthNode;1;-823.3933,142.5761;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-687.4896,294.8134;Float;False;Property;_Start;Start;3;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;88;-458.7136,244.6141;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-435.5916,431.6486;Float;False;Property;_End;End;1;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-534.5857,141.16;Float;False;Property;_ClippingFar;ClippingFar;4;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;37;-232.5068,246.2151;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;42;-328.5276,146.022;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;20;71.81947,161.1081;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;34;-127.4001,121.9734;Float;False;2;0;FLOAT;0;False;1;FLOAT;800;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;13;249.5825,162.7074;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;46;11.36225,80.27143;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-437.2845,-241.1246;Float;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;-285.8605,41.87715;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;459.794,85.27888;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;52;-89.95363,-62.38251;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;29;473.2147,-369.3889;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.01;False;2;FLOAT;0.99;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;54;75.0817,-63.51749;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;65;493.3341,229.8421;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;657.4724,-599.826;Float;True;Property;_FogGradient;FogGradient;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;64;625.0981,77.85619;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;89;1076.516,-535.9599;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;3;1047.232,-230.9688;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1231.315,-230.5577;Float;False;True;2;Float;3;0;1;FogEffect;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;88;0;1;0
WireConnection;88;1;40;0
WireConnection;37;0;88;0
WireConnection;37;2;10;0
WireConnection;42;0;43;0
WireConnection;20;0;37;0
WireConnection;20;1;10;0
WireConnection;34;0;42;0
WireConnection;34;1;1;0
WireConnection;13;0;20;0
WireConnection;46;0;34;0
WireConnection;36;0;46;0
WireConnection;36;1;13;0
WireConnection;52;0;2;0
WireConnection;52;1;53;0
WireConnection;29;0;36;0
WireConnection;54;0;52;0
WireConnection;26;1;29;0
WireConnection;64;0;36;0
WireConnection;64;1;65;0
WireConnection;64;2;54;0
WireConnection;89;0;2;0
WireConnection;89;1;26;0
WireConnection;89;2;26;4
WireConnection;3;0;2;0
WireConnection;3;1;89;0
WireConnection;3;2;64;0
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=A16706E590521AC54265D2696259197EBDAB3733