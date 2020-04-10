// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/S_Dissolve"
{
	Properties
	{
		[HDR]_Color1("Color 1", Color) = (0,0.7991071,1,0)
		_Thickness("Thickness", Range( -10 , 10)) = -0.2466298
		_Opacity("Opacity", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Opacity;
		uniform float4 _Color1;
		uniform float _Thickness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 uv_TexCoord220 = i.uv_texcoord * float2( 10,10 );
			float2 panner225 = ( 1.0 * _Time.y * float2( 0.1,0 ) + uv_TexCoord220);
			float simplePerlin2D219 = snoise( panner225 );
			float2 uv_TexCoord229 = i.uv_texcoord * float2( 10,10 );
			float2 panner231 = ( 1.0 * _Time.y * float2( 0,0.1 ) + uv_TexCoord229);
			float simplePerlin2D227 = snoise( panner231 );
			float temp_output_268_0 = ( ( _Thickness + ( 1.0 * ase_vertex3Pos.y ) ) / ( (0.7 + (simplePerlin2D219 - 0.0) * (0.94 - 0.7) / (1.0 - 0.0)) * (0.7 + (simplePerlin2D227 - 0.0) * (0.86 - 0.7) / (1.0 - 0.0)) ) );
			o.Emission = ( _Opacity * ( _Color1 * ( step( temp_output_268_0 , 0.74 ) - step( temp_output_268_0 , ( 0.74 / 3.8 ) ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
122;416;1492;839;1077.783;955.3838;1.384724;True;True
Node;AmplifyShaderEditor.Vector2Node;223;-2115.99,-686.9796;Float;False;Constant;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;10,10;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;230;-2136.186,-113.1059;Float;False;Constant;_Vector4;Vector 4;5;0;Create;True;0;0;False;0;10,10;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;229;-1886.706,-440.7838;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;232;-1834.464,-196.547;Float;False;Constant;_Vector5;Vector 5;3;0;Create;True;0;0;False;0;0,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;226;-1842.147,-627.4482;Float;False;Constant;_Vector3;Vector 3;3;0;Create;True;0;0;False;0;0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;220;-1868.25,-817.7986;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;231;-1617.266,-286.5941;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;225;-1604.037,-640.9611;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;219;-1261.919,-601.9849;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;227;-1275.148,-247.6178;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-918.0094,-770.4671;Float;False;Constant;_Invert;Invert;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;112;-1248.128,-834.77;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;78;-918.0184,-849.6945;Float;False;Property;_Thickness;Thickness;2;0;Create;True;0;0;False;0;-0.2466298;0.6907603;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;228;-979.4983,-166.1813;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.7;False;4;FLOAT;0.86;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-721.2473,-678.7173;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;224;-988.027,-486.3778;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.7;False;4;FLOAT;0.94;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-525.5428,-702.652;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-263.9722,-244.9233;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.74;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;233;-649.3109,-250.1727;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-268.1976,-134.0549;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;3.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;249;-41.819,-259.9052;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;268;-287.7463,-509.1289;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;250;73.60854,-576.9331;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;251;156.3661,-316.9963;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;256;535.0502,-700.9904;Float;False;Property;_Color1;Color 1;1;1;[HDR];Create;True;0;0;False;0;0,0.7991071,1,0;2.670157,2.565445,1.47644,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;252;347.9553,-457.7634;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;255;857.7382,-550.0599;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;110;1041.791,-679.0168;Float;False;Property;_Opacity;Opacity;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;1313.214,-559.743;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;272;-1037.61,-685.6309;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;271;-1190.796,-1042.429;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;182;1668.537,-397.2976;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;FX/S_Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;229;0;230;0
WireConnection;220;0;223;0
WireConnection;231;0;229;0
WireConnection;231;2;232;0
WireConnection;225;0;220;0
WireConnection;225;2;226;0
WireConnection;219;0;225;0
WireConnection;227;0;231;0
WireConnection;228;0;227;0
WireConnection;152;0;154;0
WireConnection;152;1;112;2
WireConnection;224;0;219;0
WireConnection;116;0;78;0
WireConnection;116;1;152;0
WireConnection;233;0;224;0
WireConnection;233;1;228;0
WireConnection;249;0;247;0
WireConnection;249;1;248;0
WireConnection;268;0;116;0
WireConnection;268;1;233;0
WireConnection;250;0;268;0
WireConnection;250;1;247;0
WireConnection;251;0;268;0
WireConnection;251;1;249;0
WireConnection;252;0;250;0
WireConnection;252;1;251;0
WireConnection;255;0;256;0
WireConnection;255;1;252;0
WireConnection;81;0;110;0
WireConnection;81;1;255;0
WireConnection;182;2;81;0
ASEEND*/
//CHKSM=51C9905B0DCB855C90D97C12BA5ED4213AC9E5CB