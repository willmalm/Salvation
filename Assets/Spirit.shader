// Upgrade NOTE: upgraded instancing buffer 'MariaSpirit' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Maria/Spirit"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0,1)
		_Value2("Value2", Range( 0 , 1)) = 0
		_Value("Value", Range( 0 , 1)) = 0
		_RemovedColor("RemovedColor", Color) = (1,0.9725649,0.01415092,1)
		_TextureSample0("Texture Sample 0", 2D) = "black" {}
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
			#include "UnityShaderVariables.cginc"
			#pragma multi_compile_instancing


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

			uniform sampler2D _Texture;
			uniform float4 _Color;
			uniform float4 _RemovedColor;
			uniform sampler2D _TextureSample0;
			uniform float _Value2;
			UNITY_INSTANCING_BUFFER_START(MariaSpirit)
				UNITY_DEFINE_INSTANCED_PROP(float, _Value)
#define _Value_arr MariaSpirit
			UNITY_INSTANCING_BUFFER_END(MariaSpirit)
			
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
				float _Value_Instance = UNITY_ACCESS_INSTANCED_PROP(_Value_arr, _Value);
				float2 uv7 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float smoothstepResult69 = smoothstep( ( _Value_Instance - 0.02 ) , _Value_Instance , uv7.x);
				float smoothstepResult75 = smoothstep( _Value_Instance , ( _Value_Instance + 0.01 ) , uv7.x);
				float2 uv5 = i.ase_texcoord.xy * float2( 0.6,0.2 ) + float2( 0,0 );
				float2 panner3 = ( 1.0 * _Time.y * float2( -0.05,0 ) + uv5);
				float4 tex2DNode1 = tex2D( _Texture, panner3 );
				float smoothstepResult93 = smoothstep( 0.2 , 1.0 , tex2DNode1.r);
				float smoothstepResult83 = smoothstep( _Value_Instance , ( _Value_Instance + 0.06 ) , uv7.x);
				float temp_output_79_0 = saturate( ( ( smoothstepResult69 - smoothstepResult75 ) + ( smoothstepResult93 * ( ( 1.0 - smoothstepResult83 ) - ( 1.0 - step( _Value_Instance , uv7.x ) ) ) ) ) );
				float smoothstepResult48 = smoothstep( _Value_Instance , ( _Value_Instance + 0.05 ) , uv7.x);
				float smoothstepResult65 = smoothstep( _Value_Instance , ( _Value_Instance + 0.2 ) , uv7.x);
				float2 uv60 = i.ase_texcoord.xy * float2( 3,1 ) + float2( 0,0 );
				float2 panner59 = ( 1.0 * _Time.y * float2( -0.3,0 ) + uv60);
				float temp_output_67_0 = ( ( 1.0 - smoothstepResult65 ) * tex2D( _TextureSample0, panner59 ).r );
				float4 lerpResult52 = lerp( _Color , _RemovedColor , saturate( ( smoothstepResult48 - temp_output_67_0 ) ));
				float clampResult95 = clamp( _Value2 , 0.0 , 1.0 );
				float smoothstepResult38 = smoothstep( ( clampResult95 + 0.05 ) , ( clampResult95 - 0.05 ) , uv7.x);
				float temp_output_6_0 = ( ( temp_output_67_0 + tex2DNode1.r ) * smoothstepResult38 );
				float lerpResult54 = lerp( 1.0 , temp_output_6_0 , saturate( ( smoothstepResult48 - temp_output_79_0 ) ));
				float4 appendResult21 = (float4(( ( temp_output_79_0 * 1.0 ) + lerpResult52 ).rgb , saturate( lerpResult54 )));
				
				
				finalColor = appendResult21;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15600
542;112;816;560;2834.252;247.3236;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-2390.642,72.75319;Float;False;InstancedProperty;_Value;Value;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-1935.082,50.84098;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.06;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2179.427,213.7444;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;4;-1125.825,-150.1454;Float;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;-0.05,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1224.722,-316.2859;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.6,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;85;-1745.068,92.49231;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;83;-1786.777,-36.3385;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3;-853.8127,-288.9906;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-2031.798,-123.2644;Float;False;2;2;0;FLOAT;0.01;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;90;-1577.734,125.6613;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-663.3553,-285.4785;Float;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;71;-2252.836,-187.0036;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;87;-1629.615,-61.6338;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;88;-1519.174,29.84067;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;93;-1573.004,-392.5814;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-1212.442,-492.206;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;75;-1797.583,-176.3638;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;69;-1870.316,-401.9156;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-1466.068,-474.408;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;77;-1601.428,-284.5595;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1587.58,-170.4501;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;59;-861.0466,-463.8965;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.3,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2410.197,393.0242;Float;False;Property;_Value2;Value2;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;65;-447.8415,-648.3979;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1449.375,594.8312;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;95;-2044.137,396.6191;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;68;-240.7224,-626.3397;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-1426.551,-288.3903;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;57;-656.4285,-499.6901;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1625.922,430.7595;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;48;-1332.716,416.3628;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;79;-1398.976,-154.9609;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-1630.613,324.3336;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-229.3975,-444.5571;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;38;-1339.227,274.1449;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-259.1816,-280.7978;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;80;-797.6183,232.8273;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;63;-1062.911,169.5622;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;64;-893.3094,162.5619;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-1134.458,418.1362;Float;False;Property;_RemovedColor;RemovedColor;4;0;Create;True;0;0;False;0;1,0.9725649,0.01415092,1;1,0.9725649,0.01415092,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-986.781,-2.014864;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;81;-520.2707,294.8304;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-1372.383,-60.64089;Float;False;Property;_Color;Color;1;0;Create;True;0;0;False;0;1,0,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;54;-617.0541,169.0698;Float;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;-410.222,137.5789;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-582.2135,0.2509756;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;46;-434.565,-5.872451;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-250.4344,76.56539;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-686.5841,332.2674;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;-97.18157,11.1627;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;47;-839.1824,342.178;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;42;-993.5345,289.5013;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;36;-1515.995,190.5335;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;91;-1399.608,-384.6627;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-724.1708,59.07748;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;10;-1307.12,121.4695;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;106.5046,-5.84659;Float;False;True;2;Float;ASEMaterialInspector;0;1;Maria/Spirit;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent;Queue=Transparent;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;84;0;9;0
WireConnection;85;0;9;0
WireConnection;85;1;7;1
WireConnection;83;0;7;1
WireConnection;83;1;9;0
WireConnection;83;2;84;0
WireConnection;3;0;5;0
WireConnection;3;2;4;0
WireConnection;70;0;9;0
WireConnection;90;0;85;0
WireConnection;1;1;3;0
WireConnection;71;0;9;0
WireConnection;87;0;83;0
WireConnection;88;0;87;0
WireConnection;88;1;90;0
WireConnection;93;0;1;1
WireConnection;75;0;7;1
WireConnection;75;1;9;0
WireConnection;75;2;70;0
WireConnection;69;0;7;1
WireConnection;69;1;71;0
WireConnection;69;2;9;0
WireConnection;66;0;9;0
WireConnection;77;0;69;0
WireConnection;77;1;75;0
WireConnection;82;0;93;0
WireConnection;82;1;88;0
WireConnection;59;0;60;0
WireConnection;65;0;7;1
WireConnection;65;1;9;0
WireConnection;65;2;66;0
WireConnection;51;0;9;0
WireConnection;95;0;8;0
WireConnection;68;0;65;0
WireConnection;89;0;77;0
WireConnection;89;1;82;0
WireConnection;57;1;59;0
WireConnection;43;0;95;0
WireConnection;48;0;7;1
WireConnection;48;1;9;0
WireConnection;48;2;51;0
WireConnection;79;0;89;0
WireConnection;41;0;95;0
WireConnection;67;0;68;0
WireConnection;67;1;57;1
WireConnection;38;0;7;1
WireConnection;38;1;43;0
WireConnection;38;2;41;0
WireConnection;58;0;67;0
WireConnection;58;1;1;1
WireConnection;80;0;48;0
WireConnection;80;1;79;0
WireConnection;63;0;48;0
WireConnection;63;1;67;0
WireConnection;64;0;63;0
WireConnection;6;0;58;0
WireConnection;6;1;38;0
WireConnection;81;0;80;0
WireConnection;54;1;6;0
WireConnection;54;2;81;0
WireConnection;52;0;15;0
WireConnection;52;1;14;0
WireConnection;52;2;64;0
WireConnection;92;0;79;0
WireConnection;46;0;54;0
WireConnection;74;0;92;0
WireConnection;74;1;52;0
WireConnection;17;0;47;0
WireConnection;17;1;14;0
WireConnection;21;0;74;0
WireConnection;21;3;46;0
WireConnection;47;0;42;0
WireConnection;42;0;38;0
WireConnection;42;1;10;0
WireConnection;36;0;7;1
WireConnection;45;0;10;0
WireConnection;45;1;6;0
WireConnection;10;0;7;1
WireConnection;10;1;9;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=FB1708F6B06403FE7ADE80BA5D470A552400F05F