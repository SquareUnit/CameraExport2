// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dome_Base"
{
	Properties
	{
		_Noise_Size("Noise_Size", Range( 0 , 20)) = 0.5
		_Color_01("Color_01", Color) = (0.01183695,0.1261925,0.1320755,0)
		_Color_02("Color_02", Color) = (1,0.7313327,0.6179246,0)
		_Noise_Tiling("Noise_Tiling", Range( 0.3 , 5)) = 0.7
		_Noise_Speed("Noise_Speed", Range( 0.1 , 0.5)) = 0.1517647
		_Vertex_Offset_Frequency("Vertex_Offset_Frequency", Range( 1.5 , 5)) = 1.53
		_Vertex_Offset_Speed("Vertex_Offset_Speed", Range( 0.1 , 0.5)) = 0.2
		_Vertex_Offset_Power("Vertex_Offset_Power", Range( 0 , 500)) = 210
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Vertex_Offset_Frequency;
		uniform float _Vertex_Offset_Speed;
		uniform float _Vertex_Offset_Power;
		uniform float4 _Color_01;
		uniform float _Noise_Size;
		uniform float _Noise_Tiling;
		uniform float _Noise_Speed;
		uniform float4 _Color_02;


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
			float mulTime15 = _Time.y * _Vertex_Offset_Speed;
			float2 temp_cast_1 = (mulTime15).xx;
			float2 uv_TexCoord21 = v.texcoord.xy * temp_cast_0 + temp_cast_1;
			float2 temp_cast_2 = (uv_TexCoord21.y).xx;
			float simplePerlin2D25 = snoise( temp_cast_2 );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 Wave35 = ( ( simplePerlin2D25 / _Vertex_Offset_Power ) * ase_vertex3Pos );
			v.vertex.xyz += Wave35;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float simplePerlin3D13 = snoise( ( ase_worldPos * _Noise_Size ) );
			float2 temp_cast_0 = (( simplePerlin3D13 * _Noise_Tiling )).xx;
			float mulTime10 = _Time.y * _Noise_Speed;
			float2 temp_cast_1 = (( mulTime10 / 2.0 )).xx;
			float2 uv_TexCoord18 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float simplePerlin3D23 = snoise( float3( uv_TexCoord18 ,  0.0 ) );
			float temp_output_26_0 = step( simplePerlin3D23 , 0.3 );
			float4 lerpResult36 = lerp( ( _Color_01 + temp_output_26_0 ) , ( _Color_02 + step( simplePerlin3D23 , ( -0.3 / 0.3 ) ) ) , temp_output_26_0);
			o.Emission = lerpResult36.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
223;103;1176;571;-1740.652;158.4635;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-2860.349,-86.91557;Float;False;894.5962;504.9699;Comment;4;13;7;6;5;Dissolve_Opacity_Mask;0.4622642,0.124288,0.124288,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;6;-2747.317,-30.74077;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;5;-2794.812,192.1142;Float;True;Property;_Noise_Size;Noise_Size;1;0;Create;True;0;0;False;0;0.5;0.7402047;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2467.321,43.18524;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1647.635,262.0215;Float;False;Property;_Noise_Speed;Noise_Speed;5;0;Create;True;0;0;False;0;0.1517647;0.18;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;3;-280.4451,820.5535;Float;False;1529.043;618.6884;Comment;9;32;31;28;25;22;21;17;15;11;Vertex_Offset_Wave;0.3616198,0.1591759,0.6886792,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2;-1637.695,-34.08008;Float;False;1258.913;450.4222;Comment;5;23;18;16;12;10;Tiling & Offset;0.9528302,0.4894989,0,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;13;-2212.378,42.75894;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1396.987,476.4612;Float;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;10;-1304.498,269.1804;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-240.6038,1045.119;Float;False;Property;_Vertex_Offset_Speed;Vertex_Offset_Speed;7;0;Create;True;0;0;False;0;0.2;0.3;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1587.695,95.06522;Float;False;Property;_Noise_Tiling;Noise_Tiling;4;0;Create;True;0;0;False;0;0.7;1.35;0.3;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-34.96094,1161.655;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-218.507,915.5485;Float;False;Property;_Vertex_Offset_Frequency;Vertex_Offset_Frequency;6;0;Create;True;0;0;False;0;1.53;4.08;1.5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;4;70.578,-1026.395;Float;False;1525.069;1105.095;Comment;10;36;34;33;30;29;27;26;24;20;19;Colors;0.2696244,0.7531928,0.7830189,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1242.891,15.91992;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;14;-1108.631,449.1855;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;120.578,-427.9697;Float;True;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;-0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;150.467,939.8644;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;122.87,-746.8559;Float;True;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0.3;0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-867.7883,117.3417;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;479.5071,-458.6714;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;426.806,1169.943;Float;False;Property;_Vertex_Offset_Power;Vertex_Offset_Power;8;0;Create;True;0;0;False;0;210;120;0;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;23;-611.7814,116.1555;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;25;414.4191,926.3804;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;30;772.6431,-468.4824;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;26;502.6481,-766.8199;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;653.134,-976.3948;Float;False;Property;_Color_01;Color_01;2;0;Create;True;0;0;False;0;0.01183695,0.1261925,0.1320755,0;0.01183695,0.1261925,0.1320755,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;657.6631,-128.3001;Float;False;Property;_Color_02;Color_02;3;0;Create;True;0;0;False;0;1,0.7313327,0.6179246,0;1,0.7313327,0.6179246,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;706.275,974.0204;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;28;725.353,1276.037;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;33;1036.89,-395.8806;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;1001.482,1047.544;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;901.678,-778.6075;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;36;1323.648,-674.3018;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;1343.581,1030.542;Float;True;Wave;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2364.292,24.60206;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Dome_Base;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;6;0
WireConnection;7;1;5;0
WireConnection;13;0;7;0
WireConnection;10;0;8;0
WireConnection;15;0;11;0
WireConnection;16;0;13;0
WireConnection;16;1;12;0
WireConnection;14;0;10;0
WireConnection;14;1;9;0
WireConnection;21;0;17;0
WireConnection;21;1;15;0
WireConnection;18;0;16;0
WireConnection;18;1;14;0
WireConnection;24;0;19;0
WireConnection;24;1;20;0
WireConnection;23;0;18;0
WireConnection;25;0;21;2
WireConnection;30;0;23;0
WireConnection;30;1;24;0
WireConnection;26;0;23;0
WireConnection;26;1;20;0
WireConnection;31;0;25;0
WireConnection;31;1;22;0
WireConnection;33;0;29;0
WireConnection;33;1;30;0
WireConnection;32;0;31;0
WireConnection;32;1;28;0
WireConnection;34;0;27;0
WireConnection;34;1;26;0
WireConnection;36;0;34;0
WireConnection;36;1;33;0
WireConnection;36;2;26;0
WireConnection;35;0;32;0
WireConnection;0;2;36;0
WireConnection;0;11;35;0
ASEEND*/
//CHKSM=5ABBED50E13F112FDB0F9977E827383A96D831A1