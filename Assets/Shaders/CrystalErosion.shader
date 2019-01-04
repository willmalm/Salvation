// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Crystal Erosion"
{
	Properties
	{
		_ErosionTexture("Erosion Texture", 2D) = "white" {}
		_Erosion("Erosion", Range( 0 , 1)) = 0
		_Color("Color", 2D) = "white" {}
		_MainTexture("Main Texture", 2D) = "white" {}
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

			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;
			uniform float _Erosion;
			uniform sampler2D _ErosionTexture;
			uniform float4 _ErosionTexture_ST;
			uniform sampler2D _Color;
			uniform float4 _Color_ST;
			
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
				float2 uv_MainTexture = i.ase_texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float temp_output_17_0 = ( ( ( 0.1 + 1.0 ) * ( _Erosion + 0.01 ) ) - 0.1 );
				float2 uv_ErosionTexture = i.ase_texcoord.xy * _ErosionTexture_ST.xy + _ErosionTexture_ST.zw;
				float4 tex2DNode1 = tex2D( _ErosionTexture, uv_ErosionTexture );
				float temp_output_7_0 = step( ( 0.1 + temp_output_17_0 ) , tex2DNode1.r );
				float2 uv_Color = i.ase_texcoord.xy * _Color_ST.xy + _Color_ST.zw;
				
				
				finalColor = ( tex2D( _MainTexture, uv_MainTexture ) * ( temp_output_7_0 + ( ( step( temp_output_17_0 , tex2DNode1.r ) - temp_output_7_0 ) * tex2D( _Color, uv_Color ) ) ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15401
369;100;926;510;1635.105;422.2023;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;20;-1009.283,-288;Float;False;Constant;_1;1;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1153.283,-64;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1281.283,-192;Float;False;Property;_Erosion;Erosion;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1057.282,-432;Float;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-945.2845,-176;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-801.2867,-304;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-673.2867,-224;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;17;-519.4752,-158.4112;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-759.8669,103.0503;Float;True;Property;_ErosionTexture;Erosion Texture;0;0;Create;True;0;0;False;0;083a714e83602004ca8478195a95e795;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-311.2295,-305.5536;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;7;-152.264,-303.8078;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;2;-299.7941,-46.87746;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-80.61211,95.67362;Float;True;Property;_Color;Color;2;0;Create;True;0;0;False;0;4eb0b911a84a43443969dab21b9fe1f1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;101.721,-104.3831;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;272.2666,-104.0642;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;14;359.1344,-418.129;Float;True;Property;_MainTexture;Main Texture;3;0;Create;True;0;0;False;0;84508b93f15f2b64386ec07486afc7a3;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;512.0885,-124.843;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;761.5256,-150.2833;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1020.902,-151.7;Float;False;True;2;Float;ASEMaterialInspector;0;1;Crystal Erosion;0770190933193b94aaa3065e307002fa;0;0;SubShader 0 Pass 0;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;-1;False;-1;-1;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;19;0;9;0
WireConnection;19;1;20;0
WireConnection;18;0;19;0
WireConnection;18;1;4;0
WireConnection;17;0;18;0
WireConnection;17;1;9;0
WireConnection;8;0;9;0
WireConnection;8;1;17;0
WireConnection;7;0;8;0
WireConnection;7;1;1;1
WireConnection;2;0;17;0
WireConnection;2;1;1;1
WireConnection;11;0;2;0
WireConnection;11;1;7;0
WireConnection;10;0;11;0
WireConnection;10;1;6;0
WireConnection;12;0;7;0
WireConnection;12;1;10;0
WireConnection;13;0;14;0
WireConnection;13;1;12;0
WireConnection;0;0;13;0
ASEEND*/
//CHKSM=0B60E2B48A57799AC73109D44DB5D2598C20B84B