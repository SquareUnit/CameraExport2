// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveCamSpecial"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.08
		_radius("radius", Float) = 0
		_Tiling("Tiling", Range( 0 , 20)) = 0.5
		_color1("color1", Color) = (0.01183695,0.1261925,0.1320755,0)
		_Hardness("Hardness", Float) = 2
		_metallic("metallic", Range( 0 , 1)) = 0
		_color2("color2", Color) = (1,0.7313327,0.6179246,0)
		_Float14("Float 14", Range( 0.3 , 5)) = 0.7
		_Float16("Float 16", Float) = 2
		_Speed("Speed", Range( 0.1 , 0.5)) = 0.1094118
		_Float17("Float 17", Range( 1.5 , 5)) = 1.53
		_Float15("Float 15", Range( 0.1 , 0.5)) = 0.2
		_roughness("roughness", Range( 0 , 1)) = 0.5
		_occlusion("occlusion", Range( 0 , 1)) = 1
		_avatarPos("avatarPos", Vector) = (0,0,0,0)
		_Float20("Float 20", Range( 0 , 500)) = 500
		_avatarOffset("avatarOffset", Vector) = (0,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Float17;
		uniform float _Float15;
		uniform float _Float20;
		uniform float4 _color1;
		uniform float _Tiling;
		uniform float _Float14;
		uniform float _Speed;
		uniform float _Float16;
		uniform float4 _color2;
		uniform float _metallic;
		uniform float _roughness;
		uniform float _occlusion;
		uniform float3 _avatarPos;
		uniform float3 _avatarOffset;
		uniform float _radius;
		uniform float _Hardness;
		uniform float _Cutoff = 0.08;


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
			float2 temp_cast_0 = (_Float17).xx;
			float mulTime16 = _Time.y * _Float15;
			float2 temp_cast_1 = (mulTime16).xx;
			float2 uv_TexCoord18 = v.texcoord.xy * temp_cast_0 + temp_cast_1;
			float2 temp_cast_2 = (uv_TexCoord18.y).xx;
			float simplePerlin2D22 = snoise( temp_cast_2 );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 myVarName36 = ( ( simplePerlin2D22 / _Float20 ) * ase_vertex3Pos );
			v.vertex.xyz += myVarName36;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float simplePerlin3D9 = snoise( ( ase_worldPos * _Tiling ) );
			float2 temp_cast_0 = (( simplePerlin3D9 * _Float14 )).xx;
			float mulTime13 = _Time.y * _Speed;
			float2 temp_cast_1 = (( mulTime13 / _Float16 )).xx;
			float2 uv_TexCoord20 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float simplePerlin3D24 = snoise( float3( uv_TexCoord20 ,  0.0 ) );
			float temp_output_31_0 = step( simplePerlin3D24 , 0.3 );
			float4 lerpResult35 = lerp( ( _color1 + temp_output_31_0 ) , ( _color2 + step( simplePerlin3D24 , ( -0.3 / 0.3 ) ) ) , temp_output_31_0);
			o.Emission = lerpResult35.rgb;
			o.Metallic = _metallic;
			o.Smoothness = ( 1.0 - _roughness );
			o.Occlusion = _occlusion;
			o.Alpha = 1;
			float3 temp_output_45_0 = ( ( ( ase_worldPos - _avatarPos ) + _avatarOffset ) / _radius );
			float dotResult46 = dot( temp_output_45_0 , temp_output_45_0 );
			float clampResult48 = clamp( dotResult46 , 0.0 , 1.0 );
			clip( ( simplePerlin3D24 * pow( clampResult48 , _Hardness ) ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
376;72;1265;654;5856.556;-12.15912;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;3;-4082.232,-57.50123;Float;False;1543.249;486.6943;Comment;14;36;33;29;60;28;23;22;18;59;16;57;11;14;58;Vertex_Offset_Wave;0.3616198,0.1591759,0.6886792,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1;-6190.964,-360.2904;Float;False;728.9458;326.4069;Comment;4;5;9;7;6;Dissolve_Opacity_Mask;0.4622642,0.124288,0.124288,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-4045.6,83.78281;Float;False;Property;_Float15;Float 15;11;0;Create;True;0;0;False;0;0.2;0.2;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-6178.923,-111.764;Float;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;0.5;1.55;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;57;-3801.917,142.9265;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;6;-6168.932,-300.1156;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;2;-5440.529,-342.8521;Float;False;1115.083;420.1194;Comment;8;8;12;24;15;20;17;13;10;Tiling & Offset;0.9528302,0.4894989,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;37;-5612.881,153.3638;Float;False;1287.76;616.7454;;11;52;48;50;46;45;42;41;39;61;66;67;Camera Sphererical Mask;0.8018868,0.6739696,0.5257654,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-5885.936,-300.1896;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;58;-4067.914,151.9264;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-5570.188,201.1988;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;8;-5426.988,-88.4538;Float;False;Property;_Speed;Speed;9;0;Create;True;0;0;False;0;0.1094118;0.5;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;61;-5555.469,339.2115;Float;False;Property;_avatarPos;avatarPos;14;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;16;-4049.748,185.2596;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;13;-5157.332,-83.59162;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-5424.286,-231.1702;Float;False;Property;_Float14;Float 14;7;0;Create;True;0;0;False;0;0.7;1.5;0.3;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-5062.428,-1.871753;Float;False;Property;_Float16;Float 16;8;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-5334.493,209.8988;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-5660.994,-305.6159;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;67;-5554.548,499.4956;Float;False;Property;_avatarOffset;avatarOffset;16;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-5174.548,218.4956;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;15;-4905.465,-197.5866;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-5175.464,335.0757;Float;False;Property;_radius;radius;1;0;Create;True;0;0;False;0;0;2.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;4;-4292.88,-1178.404;Float;False;1289.794;778.0631;Comment;10;31;32;25;27;34;26;30;35;21;19;Colors;0.2696244,0.7531928,0.7830189,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-4048.296,-6.846789;Float;False;Property;_Float17;Float 17;10;0;Create;True;0;0;False;0;1.53;1.53;1.5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;59;-3785.917,161.9264;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-5139.628,-301.3397;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-4278.135,-641.6904;Float;True;Constant;_Float19;Float 19;1;0;Create;True;0;0;False;0;-0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-4767.623,-300.4304;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;45;-4926.689,205.4987;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-3755.323,-6.530993;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-4277.905,-915.0339;Float;True;Constant;_Float18;Float 18;2;0;Create;True;0;0;False;0;0.3;0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-3660.984,264.5479;Float;False;Property;_Float20;Float 20;15;0;Create;True;0;0;False;0;500;500;0;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;25;-4004.197,-641.5099;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;46;-4785.689,207.0987;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;60;-3310.917,261.9268;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;22;-3514.371,33.98483;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;24;-4534.517,-300.6166;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-3776.704,-821.4142;Float;False;Property;_color2;color2;6;0;Create;True;0;0;False;0;1,0.7313327,0.6179246,0;0.7647059,0.7647059,0.7647059,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;48;-4645.726,203.5838;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;-4006.774,-1128.742;Float;False;Property;_color1;color1;3;0;Create;True;0;0;False;0;0.01183695,0.1261925,0.1320755,0;0.5254902,0.4823529,0.5215687,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;31;-4004.133,-952.807;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;27;-3771.785,-641.8089;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;28;-3259.516,38.62471;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;29;-3248.438,251.642;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-4635.195,325.5107;Float;False;Property;_Hardness;Hardness;4;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-3506.475,-1134.714;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;52;-4475.89,205.8988;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-3466.094,-653.9443;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-2974.811,-422.3954;Float;False;Property;_roughness;roughness;12;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-3015.308,38.14864;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-4132.715,-295.0321;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2879.916,-499.5716;Float;False;Property;_metallic;metallic;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;-3246.307,-851.1888;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-2859.42,-344.2292;Float;False;Property;_occlusion;occlusion;13;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;64;-2707.74,-418.0353;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-2782.206,33.14672;Float;True;myVarName;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-2443.221,-528.1352;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;DissolveCamSpecial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.08;True;False;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;31.3;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;0;11;0
WireConnection;7;0;6;0
WireConnection;7;1;5;0
WireConnection;58;0;57;0
WireConnection;16;0;58;0
WireConnection;13;0;8;0
WireConnection;41;0;39;0
WireConnection;41;1;61;0
WireConnection;9;0;7;0
WireConnection;66;0;41;0
WireConnection;66;1;67;0
WireConnection;15;0;13;0
WireConnection;15;1;12;0
WireConnection;59;0;16;0
WireConnection;17;0;9;0
WireConnection;17;1;10;0
WireConnection;20;0;17;0
WireConnection;20;1;15;0
WireConnection;45;0;66;0
WireConnection;45;1;42;0
WireConnection;18;0;14;0
WireConnection;18;1;59;0
WireConnection;25;0;21;0
WireConnection;25;1;19;0
WireConnection;46;0;45;0
WireConnection;46;1;45;0
WireConnection;60;0;23;0
WireConnection;22;0;18;2
WireConnection;24;0;20;0
WireConnection;48;0;46;0
WireConnection;31;0;24;0
WireConnection;31;1;19;0
WireConnection;27;0;24;0
WireConnection;27;1;25;0
WireConnection;28;0;22;0
WireConnection;28;1;60;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;52;0;48;0
WireConnection;52;1;50;0
WireConnection;34;0;26;0
WireConnection;34;1;27;0
WireConnection;33;0;28;0
WireConnection;33;1;29;0
WireConnection;53;0;24;0
WireConnection;53;1;52;0
WireConnection;35;0;32;0
WireConnection;35;1;34;0
WireConnection;35;2;31;0
WireConnection;64;0;62;0
WireConnection;36;0;33;0
WireConnection;0;2;35;0
WireConnection;0;3;65;0
WireConnection;0;4;64;0
WireConnection;0;5;63;0
WireConnection;0;10;53;0
WireConnection;0;11;36;0
ASEEND*/
//CHKSM=0035A55B98CEB4EEB94A09578B4F9CBC88E14C84