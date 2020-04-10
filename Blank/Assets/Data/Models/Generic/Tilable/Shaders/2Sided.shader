// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "2Sided"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Mask("Mask", 2D) = "white" {}
		_FrontAlbedo("FrontAlbedo", 2D) = "white" {}
		_FrontNormalMap("FrontNormalMap", 2D) = "bump" {}
		_FrontColor("FrontColor", Color) = (1,0.6691177,0.6691177,0)
		_BackAlbedo("BackAlbedo", 2D) = "white" {}
		_BackNormalMap("BackNormalMap", 2D) = "bump" {}
		_BackColor("BackColor", Color) = (0,0,1,0)
		_Vertex_Offset_Power("Vertex_Offset_Power", Float) = 0
		_Front_Roughness("Front_Roughness", 2D) = "white" {}
		_Back_Roughness("Back_Roughness", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			half ASEVFace : VFACE;
		};

		uniform float _Vertex_Offset_Power;
		uniform sampler2D _FrontNormalMap;
		uniform sampler2D _BackNormalMap;
		uniform sampler2D _FrontAlbedo;
		uniform float4 _FrontColor;
		uniform sampler2D _BackAlbedo;
		uniform float4 _BackColor;
		uniform sampler2D _Front_Roughness;
		uniform sampler2D _Back_Roughness;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 temp_cast_0 = (( _SinTime.w * _Vertex_Offset_Power )).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 switchResult3 = (((i.ASEVFace>0)?(UnpackNormal( tex2D( _FrontNormalMap, i.uv_texcoord ) )):(UnpackNormal( tex2D( _BackNormalMap, i.uv_texcoord ) ))));
			o.Normal = switchResult3;
			float4 switchResult2 = (((i.ASEVFace>0)?(( tex2D( _FrontAlbedo, i.uv_texcoord ) * _FrontColor )):(( tex2D( _BackAlbedo, i.uv_texcoord ) * _BackColor ))));
			o.Albedo = switchResult2.rgb;
			float4 switchResult21 = (((i.ASEVFace>0)?(( 1.0 - tex2D( _Front_Roughness, i.uv_texcoord ) )):(( 1.0 - tex2D( _Back_Roughness, i.uv_texcoord ) ))));
			o.Smoothness = switchResult21.r;
			o.Alpha = 1;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			clip( tex2D( _Mask, uv_Mask ).a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
43;44;1739;950;1684.096;978.7422;1.424338;True;False
Node;AmplifyShaderEditor.CommentaryNode;12;-2196.4,-833.3101;Float;False;1252.009;1229.961;Inspired by 2Side Sample by The Four Headed Cat;12;14;4;5;8;7;10;11;6;9;3;1;2;Two Sided Shader using Switch by Face;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2147.711,-180.51;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1790.309,-401.31;Float;True;Property;_BackAlbedo;BackAlbedo;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1790.309,-769.3101;Float;True;Property;_FrontAlbedo;FrontAlbedo;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-1710.309,-209.3099;Float;False;Property;_BackColor;BackColor;7;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-1710.309,-577.31;Float;False;Property;_FrontColor;FrontColor;4;0;Create;True;0;0;False;0;1,0.6691177,0.6691177,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-841.0685,-902.0169;Float;True;Property;_Front_Roughness;Front_Roughness;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-838.8235,-685.8335;Float;True;Property;_Back_Roughness;Back_Roughness;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1443.311,-483.2096;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1448.411,-347.2099;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-629.842,695.0602;Float;False;Property;_Vertex_Offset_Power;Vertex_Offset_Power;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1750.309,-31.30997;Float;True;Property;_FrontNormalMap;FrontNormalMap;3;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;15;-620.0142,521.5685;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;20;-479.7034,-897.0555;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;-1734.309,166.6901;Float;True;Property;_BackNormalMap;BackNormalMap;6;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;-475.8332,-680.9169;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SwitchByFaceNode;3;-1294.31,-33.30997;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;1;-1278.31,94.69007;Float;True;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwitchByFaceNode;2;-1182.31,-417.31;Float;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-342.757,584.9854;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;21;-153.073,-858.529;Float;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;280.6721,-178.2835;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;2Sided;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;1;14;0
WireConnection;4;1;14;0
WireConnection;18;1;14;0
WireConnection;22;1;14;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;9;0;8;0
WireConnection;9;1;7;0
WireConnection;10;1;14;0
WireConnection;20;0;18;0
WireConnection;11;1;14;0
WireConnection;23;0;22;0
WireConnection;3;0;10;0
WireConnection;3;1;11;0
WireConnection;2;0;6;0
WireConnection;2;1;9;0
WireConnection;17;0;15;4
WireConnection;17;1;16;0
WireConnection;21;0;20;0
WireConnection;21;1;23;0
WireConnection;0;0;2;0
WireConnection;0;1;3;0
WireConnection;0;4;21;0
WireConnection;0;10;1;4
WireConnection;0;11;17;0
ASEEND*/
//CHKSM=0CEC1E44A53C010AA173C1FA5A1C0460BCBB0699