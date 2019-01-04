// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DepthShader"
{
	Properties
	{
		
	}
	
	SubShader
	{
		
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend DstColor Zero , DstColor Zero
		Cull Off
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		
		

		Pass
		{
			Name "SubShader 0 Pass 0"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _CameraDepthTexture;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord1.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.yzw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				fixed4 finalColor;
				float4 screenPos = i.ase_texcoord;
				float eyeDepth5 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( screenPos ))));
				float eyeDepth = i.ase_texcoord1.x;
				float smoothstepResult21 = smoothstep( 0.0 , 0.2 , ( eyeDepth5 - eyeDepth ));
				
				
				finalColor = ( ( 1.0 - smoothstepResult21 ) + float4(0.01126385,1,0,1) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15600
465;111;1011;585;999.4028;749.2731;2.213922;True;False
Node;AmplifyShaderEditor.ScreenDepthNode;5;91.87697,-153.0751;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SurfaceDepthNode;17;53.7085,91.93028;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;389.1111,-12.9196;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;21;599.1711,-10.46376;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;689.8326,173.713;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.01126385,1,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;10;789.7698,1.411305;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;1000.713,3.977095;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1170.315,7.752605;Float;False;True;2;Float;ASEMaterialInspector;0;1;DepthShader;0770190933193b94aaa3065e307002fa;0;0;SubShader 0 Pass 0;2;True;6;2;False;-1;0;False;-1;6;2;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;0;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;6;0;5;0
WireConnection;6;1;17;0
WireConnection;21;0;6;0
WireConnection;10;0;21;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;0;0;11;0
ASEEND*/
//CHKSM=98BD719F6350B7FE270C1898F91028990340CF0D