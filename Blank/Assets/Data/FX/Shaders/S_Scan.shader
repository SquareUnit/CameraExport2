// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/S_Scan"
{
	Properties
	{
		_F_Bias("F_Bias", Range( -1 , 1)) = 0
		_F_Scale("F_Scale", Range( 0 , 5)) = 0
		_F_Power("F_Power", Range( 0 , 5)) = 0
		_Add("Add", Float) = 2.94
		_Color0("Color 0", Color) = (1,0,0,1)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform float _Add;
		uniform float _F_Bias;
		uniform float _F_Scale;
		uniform float _F_Power;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV35 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode35 = ( _F_Bias + _F_Scale * pow( 1.0 - fresnelNdotV35, _F_Power ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float eyeDepth40 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float smoothstepResult34 = smoothstep( 0.0 , 1.0 , ( 1.0 - ( eyeDepth40 - ( ase_screenPos.w - -0.25 ) ) ));
			float temp_output_58_0 = ( ( _Add + fresnelNode35 ) * smoothstepResult34 );
			o.Emission = ( _Color0 + temp_output_58_0 ).rgb;
			float2 uv_TexCoord65 = i.uv_texcoord * float2( 1,5 );
			float2 panner64 = ( 1.0 * _Time.y * float2( 0,0.05 ) + uv_TexCoord65);
			float2 panner72 = ( 1.0 * _Time.y * float2( 0,-0.05 ) + i.uv_texcoord);
			o.Alpha = ( ( ( tex2D( _TextureSample0, panner64 ) * tex2D( _TextureSample1, panner72 ) ) / 3.3 ) + temp_output_58_0 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
91;376;1492;845;1083.987;-429.722;1.363464;True;True
Node;AmplifyShaderEditor.ScreenPosInputsNode;26;-1406.177,1365.041;Float;True;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-1186.393,1583.068;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;-0.25;-0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-962.2499,1462.156;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-1539.926,221.9906;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1538.536,525.0563;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;71;-1476.585,670.155;Float;False;Constant;_Vector1;Vector 1;7;0;Create;True;0;0;False;0;0,-0.05;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;66;-1477.974,367.0894;Float;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;0,0.05;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ScreenDepthNode;40;-1018.824,1330.164;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;77;-689.1074,1356.99;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;64;-1260.068,297.6121;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;72;-1258.678,600.6777;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-734.8029,860.8869;Float;False;Property;_F_Bias;F_Bias;0;0;Create;True;0;0;False;0;0;-0.5958824;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-733.0123,930.6986;Float;False;Property;_F_Scale;F_Scale;1;0;Create;True;0;0;False;0;0;0.802353;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-733.4724,1004.109;Float;False;Property;_F_Power;F_Power;2;0;Create;True;0;0;False;0;0;0.05882353;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;73;-1056.421,572.9678;Float;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;False;0;None;618b931f6fdca884d9487a4c7a141f75;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;35;-432.6504,822.7266;Float;True;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;3.06;False;2;FLOAT;5.93;False;3;FLOAT;3.39;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;59;-1057.81,269.9022;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;618b931f6fdca884d9487a4c7a141f75;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;33;-486.5248,1306.801;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-370.757,718.8552;Float;False;Property;_Add;Add;3;0;Create;True;0;0;False;0;2.94;0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-562.7288,447.2789;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-283.7337,582.106;Float;False;Constant;_Float3;Float 3;7;0;Create;True;0;0;False;0;3.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;34;-214.2234,1192.384;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-156.5445,705.1231;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-138.2274,441.042;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;23;99.31673,-45.1297;Float;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;1,0,0,1;0.6179246,1,0.8854804,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;93.01784,704.1306;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;431.6217,415.1143;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;75;386.4996,104.9991;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;55;742.9066,119.6017;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;FX/S_Scan;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;26;4
WireConnection;28;1;27;0
WireConnection;77;0;40;0
WireConnection;77;1;28;0
WireConnection;64;0;65;0
WireConnection;64;2;66;0
WireConnection;72;0;70;0
WireConnection;72;2;71;0
WireConnection;73;1;72;0
WireConnection;35;1;51;0
WireConnection;35;2;52;0
WireConnection;35;3;53;0
WireConnection;59;1;64;0
WireConnection;33;0;77;0
WireConnection;74;0;59;0
WireConnection;74;1;73;0
WireConnection;34;0;33;0
WireConnection;36;0;44;0
WireConnection;36;1;35;0
WireConnection;68;0;74;0
WireConnection;68;1;69;0
WireConnection;58;0;36;0
WireConnection;58;1;34;0
WireConnection;61;0;68;0
WireConnection;61;1;58;0
WireConnection;75;0;23;0
WireConnection;75;1;58;0
WireConnection;55;2;75;0
WireConnection;55;9;61;0
ASEEND*/
//CHKSM=A545FD56027092A8F63F7A6E52F172B561EF8CB7