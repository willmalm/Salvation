// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Maria/UI/Bar"
{
	Properties
	{
		_Color("Color", Color) = (1,0,0,1)
		_Removed("Removed", Float) = 0
		_Value("Value", Float) = 0
		_RemovedColor("RemovedColor", Color) = (1,0.9725649,0.01415092,1)
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend DstColor Zero , DstColor Zero
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

			uniform float4 _Color;
			uniform float _Value;
			uniform float _Removed;
			uniform float4 _RemovedColor;
			
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
				float2 uv5 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_6_0 = step( uv5.x , _Value );
				float temp_output_7_0 = step( uv5.x , _Removed );
				float4 appendResult15 = (float4(( ( _Color * temp_output_6_0 ) + ( saturate( ( temp_output_7_0 - temp_output_6_0 ) ) * _RemovedColor ) ).rgb , saturate( ( temp_output_6_0 + temp_output_7_0 ) )));
				
				
				finalColor = appendResult15;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15600
454;92;984;593;647.6572;384.5196;1.842123;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-544.8427,127.4868;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-334.5425,255.5512;Float;False;Property;_Removed;Removed;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-350.2969,63.46115;Float;False;Property;_Value;Value;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;6;-130.4962,89.68674;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;7;-111.4962,251.6867;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;56.98783,230.5596;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;14;216.1355,241.4533;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;0.563386,378.3528;Float;False;Property;_RemovedColor;RemovedColor;3;0;Create;True;0;0;False;0;1,0.9725649,0.01415092,1;1,0.9725649,0.01415092,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-237.3615,-100.4242;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;1,0,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;51.68604,60.87342;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;397.4859,221.7733;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;302.5243,133.8032;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;542.9628,122.353;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;17;566.3024,298.908;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;719.067,128.0718;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;867.9919,117.1725;Float;False;True;2;Float;ASEMaterialInspector;0;1;Maria/UI/Bar;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;6;2;False;-1;0;False;-1;6;2;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;6;0;5;1
WireConnection;6;1;3;0
WireConnection;7;0;5;1
WireConnection;7;1;4;0
WireConnection;8;0;7;0
WireConnection;8;1;6;0
WireConnection;14;0;8;0
WireConnection;9;0;2;0
WireConnection;9;1;6;0
WireConnection;10;0;14;0
WireConnection;10;1;11;0
WireConnection;16;0;6;0
WireConnection;16;1;7;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;17;0;16;0
WireConnection;15;0;12;0
WireConnection;15;3;17;0
WireConnection;0;0;15;0
ASEEND*/
//CHKSM=E57EB35DD92667CA982C22589987ACF7F3E1B441