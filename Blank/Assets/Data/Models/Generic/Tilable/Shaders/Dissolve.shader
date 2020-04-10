// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dissolve"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Noise_Size("Noise_Size", Range( 0 , 20)) = 0.5
		_Color_01("Color_01", Color) = (0.01183695,0.1261925,0.1320755,0)
		_Color_02("Color_02", Color) = (1,0.7313327,0.6179246,0)
		_Noise_Tiling("Noise_Tiling", Range( 0.3 , 5)) = 0.7
		_Noise_Speed("Noise_Speed", Range( 0.1 , 0.5)) = 0.1517647
		_Vertex_Offset_Frequency("Vertex_Offset_Frequency", Range( 1.5 , 5)) = 1.53
		_Vertex_Offset_Speed("Vertex_Offset_Speed", Range( 0.1 , 0.5)) = 0.2
		_Vertex_Offseet_Power("Vertex_Offseet_Power", Range( 0 , 500)) = 5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Vertex_Offset_Frequency;
		uniform float _Vertex_Offset_Speed;
		uniform float _Vertex_Offseet_Power;
		uniform float4 _Color_01;
		uniform float _Noise_Size;
		uniform float _Noise_Tiling;
		uniform float _Noise_Speed;
		uniform float4 _Color_02;
		uniform float _Cutoff = 0.5;


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


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = (_Vertex_Offset_Frequency).xx;
			float mulTime241 = _Time.y * _Vertex_Offset_Speed;
			float2 temp_cast_1 = (mulTime241).xx;
			float2 uv_TexCoord243 = v.texcoord.xy * temp_cast_0 + temp_cast_1;
			float2 temp_cast_2 = (uv_TexCoord243.y).xx;
			float simplePerlin2D245 = snoise( temp_cast_2 );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 Wave249 = ( ( simplePerlin2D245 / _Vertex_Offseet_Power ) * ase_vertex3Pos );
			v.vertex.xyz += Wave249;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float simplePerlin3D49 = snoise( ( ase_worldPos * _Noise_Size ) );
			float2 temp_cast_0 = (( simplePerlin3D49 * _Noise_Tiling )).xx;
			float mulTime179 = _Time.y * _Noise_Speed;
			float2 temp_cast_1 = (( mulTime179 / 2.0 )).xx;
			float2 uv_TexCoord180 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float simplePerlin3D181 = snoise( float3( uv_TexCoord180 ,  0.0 ) );
			float temp_output_195_0 = step( simplePerlin3D181 , 0.3 );
			float4 lerpResult266 = lerp( ( _Color_01 + temp_output_195_0 ) , ( _Color_02 + step( simplePerlin3D181 , ( -0.3 / 0.3 ) ) ) , temp_output_195_0);
			o.Emission = lerpResult266.rgb;
			o.Alpha = 1;
			clip( simplePerlin3D181 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
183;151;1176;494;-1149.844;-1212.186;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;59;-1587.639,141.295;Float;False;894.5962;504.9699;Comment;4;49;51;23;50;Dissolve_Opacity_Mask;0.4622642,0.124288,0.124288,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;270;-364.985,194.1305;Float;False;1258.913;450.4222;Comment;6;177;175;178;179;180;181;Tiling & Offset;0.9528302,0.4894989,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1522.102,420.3248;Float;True;Property;_Noise_Size;Noise_Size;1;0;Create;True;0;0;False;0;0.5;0.7402047;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;23;-1474.607,197.4698;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;239;992.2649,1048.764;Float;False;1529.043;618.6884;Comment;9;248;247;246;245;244;243;242;241;240;Vertex_Offset_Wave;0.3616198,0.1591759,0.6886792,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1194.611,271.3958;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-374.9255,490.2321;Float;False;Property;_Noise_Speed;Noise_Speed;5;0;Create;True;0;0;False;0;0.1517647;0.18;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;49;-939.6684,270.9695;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-314.985,323.2758;Float;False;Property;_Noise_Tiling;Noise_Tiling;4;0;Create;True;0;0;False;0;0.7;1.35;0.3;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;240;1045.898,1385.389;Float;False;Property;_Vertex_Offset_Speed;Vertex_Offset_Speed;7;0;Create;True;0;0;False;0;0.2;0.3;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;286;-124.2769,704.6718;Float;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;179;-31.78754,497.391;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;242;1054.203,1143.759;Float;False;Property;_Vertex_Offset_Frequency;Vertex_Offset_Frequency;6;0;Create;True;0;0;False;0;1.53;4.08;1.5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;271;1343.288,-798.1842;Float;False;1525.069;1105.095;Comment;10;196;185;223;202;184;224;195;225;205;266;Colors;0.2696244,0.7531928,0.7830189,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;285;164.0793,677.3961;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;241;1237.749,1389.866;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;29.8189,244.1305;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;243;1423.177,1168.075;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;196;1395.58,-518.6453;Float;True;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0.3;0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;180;404.9216,345.5523;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;185;1393.288,-199.7591;Float;True;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;-0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;245;1687.129,1154.591;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;1658.516,1525.154;Float;False;Property;_Vertex_Offseet_Power;Vertex_Offseet_Power;8;0;Create;True;0;0;False;0;5;27.07;0;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;181;660.9286,344.3661;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;223;1752.217,-230.4608;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;184;2045.353,-240.2718;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;246;1978.985,1202.231;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;224;1930.373,99.91048;Float;False;Property;_Color_02;Color_02;3;0;Create;True;0;0;False;0;1,0.7313327,0.6179246,0;1,0.7313327,0.6179246,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;195;1775.358,-538.6094;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;247;1998.063,1504.248;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;202;1925.844,-748.1842;Float;False;Property;_Color_01;Color_01;2;0;Create;True;0;0;False;0;0.01183695,0.1261925,0.1320755,0;0.01183695,0.1261925,0.1320755,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;205;2174.388,-550.397;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;2274.192,1275.755;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;225;2309.6,-167.67;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;266;2596.358,-446.0912;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;249;2616.291,1258.753;Float;True;Wave;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;48;3814.582,72.88226;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;31.3;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;23;0
WireConnection;51;1;50;0
WireConnection;49;0;51;0
WireConnection;179;0;177;0
WireConnection;285;0;179;0
WireConnection;285;1;286;0
WireConnection;241;0;240;0
WireConnection;178;0;49;0
WireConnection;178;1;175;0
WireConnection;243;0;242;0
WireConnection;243;1;241;0
WireConnection;180;0;178;0
WireConnection;180;1;285;0
WireConnection;245;0;243;2
WireConnection;181;0;180;0
WireConnection;223;0;185;0
WireConnection;223;1;196;0
WireConnection;184;0;181;0
WireConnection;184;1;223;0
WireConnection;246;0;245;0
WireConnection;246;1;244;0
WireConnection;195;0;181;0
WireConnection;195;1;196;0
WireConnection;205;0;202;0
WireConnection;205;1;195;0
WireConnection;248;0;246;0
WireConnection;248;1;247;0
WireConnection;225;0;224;0
WireConnection;225;1;184;0
WireConnection;266;0;205;0
WireConnection;266;1;225;0
WireConnection;266;2;195;0
WireConnection;249;0;248;0
WireConnection;48;2;266;0
WireConnection;48;10;181;0
WireConnection;48;11;249;0
ASEEND*/
//CHKSM=71D11B6A31E759EB59165005DEDDA8D7E9CF2B29