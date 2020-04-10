// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveCamBase"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Tiling("Tiling", Float) = 1
		_Normal("Normal", 2D) = "bump" {}
		_radius("radius", Float) = 0
		_metallic("metallic", Range( 0 , 1)) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_roughnessMap("roughnessMap", 2D) = "white" {}
		_Hardness("Hardness", Float) = 2
		_color("color", Color) = (0,0,0,0)
		[Toggle]_colorOrAlbedo("colorOrAlbedo", Float) = 1
		[Toggle]_colorOrScalar("colorOrScalar", Float) = 1
		_roughness("roughness", Range( 0 , 1)) = 0.5
		_occlusion("occlusion", Range( 0 , 1)) = 1
		_avatarPos("avatarPos", Vector) = (0,0,0,0)
		_avatarOffset("avatarOffset", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" }
		Cull Off
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float _colorOrAlbedo;
		uniform float4 _color;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _metallic;
		uniform float _colorOrScalar;
		uniform sampler2D _roughnessMap;
		uniform float4 _roughnessMap_ST;
		uniform float _roughness;
		uniform float _occlusion;
		uniform float3 _avatarPos;
		uniform float3 _avatarOffset;
		uniform float _radius;
		uniform float _Hardness;
		uniform float _Tiling;
		uniform float _Cutoff = 0.5;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = lerp(_color,tex2D( _Albedo, uv_Albedo ),_colorOrAlbedo).rgb;
			o.Metallic = _metallic;
			float2 uv_roughnessMap = i.uv_texcoord * _roughnessMap_ST.xy + _roughnessMap_ST.zw;
			float4 temp_cast_1 = (_roughness).xxxx;
			o.Smoothness = ( 1.0 - lerp(tex2D( _roughnessMap, uv_roughnessMap ),temp_cast_1,_colorOrScalar) ).r;
			o.Occlusion = _occlusion;
			o.Alpha = 1;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_9_0 = ( ( ( ase_worldPos - _avatarPos ) + _avatarOffset ) / _radius );
			float dotResult10 = dot( temp_output_9_0 , temp_output_9_0 );
			float clampResult14 = clamp( dotResult10 , 0.0 , 1.0 );
			float simplePerlin3D12 = snoise( ( ase_worldPos * _Tiling ) );
			clip( ( pow( clampResult14 , _Hardness ) * ( 1.0 - simplePerlin3D12 ) ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
69;51;1829;943;2106.699;694.8816;1.6;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-2399.97,713.772;Float;False;1049.76;634.5627;;11;17;13;14;10;9;4;33;34;36;37;39;Camera Sphererical Mask;0.8018868,0.6739696,0.5257654,1;0;0
Node;AmplifyShaderEditor.Vector3Node;34;-2368.863,905.5094;Float;False;Property;_avatarPos;avatarPos;13;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;33;-2383.583,767.4967;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-2192.888,843.1967;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;36;-2371.942,1058.793;Float;False;Property;_avatarOffset;avatarOffset;14;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;4;-2120.655,760.5832;Float;False;Property;_radius;radius;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-2116.943,976.7935;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;6;-1104.295,755.0458;Float;False;855.7853;274.2475;;5;16;12;11;8;7;3D Noise Generation;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;7;-1096.314,801.4011;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;8;-1057.256,946.6008;Float;False;Property;_Tiling;Tiling;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;9;-1936.781,766.907;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;10;-1810.781,767.507;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-898.2291,799.548;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;14;-1670.819,763.9921;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1090.138,250.2488;Float;False;Property;_roughness;roughness;11;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1660.288,885.9177;Float;False;Property;_Hardness;Hardness;7;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1105.965,-0.9411926;Float;True;Property;_roughnessMap;roughnessMap;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;12;-685.1534,794.8538;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-391.8646,841.6309;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;17;-1500.984,766.3071;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-560.7287,-514.4412;Float;False;Property;_color;color;8;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-860.2343,-506.4543;Float;True;Property;_Albedo;Albedo;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;40;-691.9651,114.0588;Float;False;Property;_colorOrScalar;colorOrScalar;10;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;26;-262.3199,-222.0609;Float;False;Property;_colorOrAlbedo;colorOrAlbedo;9;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-434.1974,-9.927803;Float;False;Property;_metallic;metallic;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-182.7167,410.5515;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-733.2344,-221.4545;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;21;-355.2344,86.5455;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-438.1379,278.2488;Float;False;Property;_occlusion;occlusion;12;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;28;39,-37;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;DissolveCamBase;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;33;0
WireConnection;37;1;34;0
WireConnection;39;0;37;0
WireConnection;39;1;36;0
WireConnection;9;0;39;0
WireConnection;9;1;4;0
WireConnection;10;0;9;0
WireConnection;10;1;9;0
WireConnection;11;0;7;0
WireConnection;11;1;8;0
WireConnection;14;0;10;0
WireConnection;12;0;11;0
WireConnection;16;0;12;0
WireConnection;17;0;14;0
WireConnection;17;1;13;0
WireConnection;40;0;41;0
WireConnection;40;1;31;0
WireConnection;26;0;23;0
WireConnection;26;1;18;0
WireConnection;22;0;17;0
WireConnection;22;1;16;0
WireConnection;21;0;40;0
WireConnection;28;0;26;0
WireConnection;28;1;19;0
WireConnection;28;3;20;0
WireConnection;28;4;21;0
WireConnection;28;5;32;0
WireConnection;28;10;22;0
ASEEND*/
//CHKSM=5B8E90F1B25E1CA658B99255B75F1777CB4528AA