// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VertexPaintShader"
{
	Properties
	{
		_Albedo_01("Albedo_01", 2D) = "white" {}
		[Normal]_Normal_01("Normal_01", 2D) = "white" {}
		_Roughness_01("Roughness_01", 2D) = "white" {}
		_Tiling_01("Tiling_01", Range( 0 , 10)) = 1
		_Albedo_02("Albedo_02", 2D) = "white" {}
		[Normal]_Normal_02("Normal_02", 2D) = "white" {}
		_Roughness_02("Roughness_02", 2D) = "white" {}
		_Tiling_02("Tiling_02", Range( 0 , 10)) = 1
		_Albedo_03("Albedo_03", 2D) = "white" {}
		[Normal]_Normal_03("Normal_03", 2D) = "white" {}
		_Roughness_03("Roughness_03", 2D) = "white" {}
		_Tiling_03("Tiling_03", Range( 0 , 10)) = 1
		_Albedo_04("Albedo_04", 2D) = "white" {}
		[Normal]_Normal_04("Normal_04", 2D) = "white" {}
		_Roughness_04("Roughness_04", 2D) = "white" {}
		_Tiling_04("Tiling_04", Range( 0 , 10)) = 1
		[Toggle(_LAYER_01_ISUV2_ON)] _LAYER_01_ISUV2("LAYER_01_ISUV2", Float) = 0
		[Toggle(_LAYER_03_ISUV2_ON)] _LAYER_03_ISUV2("LAYER_03_ISUV2", Float) = 0
		[Toggle(_LAYER_02_ISUV2_ON)] _LAYER_02_ISUV2("LAYER_02_ISUV2", Float) = 0
		[Toggle(_LAYER_04_ISUV2_ON)] _LAYER_04_ISUV2("LAYER_04_ISUV2", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _LAYER_01_ISUV2_ON
		#pragma shader_feature _LAYER_02_ISUV2_ON
		#pragma shader_feature _LAYER_03_ISUV2_ON
		#pragma shader_feature _LAYER_04_ISUV2_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Normal_01;
		uniform float _Tiling_01;
		uniform sampler2D _Normal_02;
		uniform float _Tiling_02;
		uniform sampler2D _Normal_03;
		uniform float _Tiling_03;
		uniform sampler2D _Normal_04;
		uniform float _Tiling_04;
		uniform sampler2D _Albedo_01;
		uniform sampler2D _Albedo_02;
		uniform sampler2D _Albedo_03;
		uniform sampler2D _Albedo_04;
		uniform sampler2D _Roughness_01;
		uniform sampler2D _Roughness_02;
		uniform sampler2D _Roughness_03;
		uniform sampler2D _Roughness_04;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Tiling_01).xx;
			float2 uv_TexCoord101 = i.uv_texcoord * temp_cast_0;
			float2 temp_cast_1 = (_Tiling_01).xx;
			float2 uv_TexCoord102 = i.uv_texcoord * temp_cast_1;
			#ifdef _LAYER_01_ISUV2_ON
				float2 staticSwitch103 = uv_TexCoord102;
			#else
				float2 staticSwitch103 = uv_TexCoord101;
			#endif
			float2 Tiling_01104 = staticSwitch103;
			float2 temp_cast_2 = (_Tiling_02).xx;
			float2 uv_TexCoord107 = i.uv_texcoord * temp_cast_2;
			float2 temp_cast_3 = (_Tiling_02).xx;
			float2 uv_TexCoord106 = i.uv_texcoord * temp_cast_3;
			#ifdef _LAYER_02_ISUV2_ON
				float2 staticSwitch108 = uv_TexCoord106;
			#else
				float2 staticSwitch108 = uv_TexCoord107;
			#endif
			float2 Tiling_02109 = staticSwitch108;
			float VC_R9 = i.vertexColor.r;
			float4 lerpResult56 = lerp( tex2D( _Normal_01, Tiling_01104 ) , tex2D( _Normal_02, Tiling_02109 ) , VC_R9);
			float2 temp_cast_4 = (_Tiling_03).xx;
			float2 uv_TexCoord121 = i.uv_texcoord * temp_cast_4;
			float2 temp_cast_5 = (_Tiling_03).xx;
			float2 uv_TexCoord120 = i.uv_texcoord * temp_cast_5;
			#ifdef _LAYER_03_ISUV2_ON
				float2 staticSwitch122 = uv_TexCoord120;
			#else
				float2 staticSwitch122 = uv_TexCoord121;
			#endif
			float2 Tiling_03124 = staticSwitch122;
			float VC_G10 = i.vertexColor.g;
			float4 lerpResult57 = lerp( lerpResult56 , tex2D( _Normal_03, Tiling_03124 ) , VC_G10);
			float2 temp_cast_6 = (_Tiling_04).xx;
			float2 uv_TexCoord117 = i.uv_texcoord * temp_cast_6;
			float2 temp_cast_7 = (_Tiling_04).xx;
			float2 uv_TexCoord116 = i.uv_texcoord * temp_cast_7;
			#ifdef _LAYER_04_ISUV2_ON
				float2 staticSwitch118 = uv_TexCoord116;
			#else
				float2 staticSwitch118 = uv_TexCoord117;
			#endif
			float2 Tiling_04119 = staticSwitch118;
			float VC_B12 = i.vertexColor.b;
			float4 lerpResult59 = lerp( lerpResult57 , tex2D( _Normal_04, Tiling_04119 ) , VC_B12);
			float4 Normal_Blend61 = lerpResult59;
			o.Normal = Normal_Blend61.rgb;
			float4 lerpResult6 = lerp( tex2D( _Albedo_01, Tiling_01104 ) , tex2D( _Albedo_02, Tiling_02109 ) , VC_R9);
			float4 lerpResult8 = lerp( lerpResult6 , tex2D( _Albedo_03, Tiling_03124 ) , VC_G10);
			float4 lerpResult7 = lerp( lerpResult8 , tex2D( _Albedo_04, Tiling_04119 ) , VC_B12);
			float4 Albedo_blend17 = lerpResult7;
			o.Albedo = Albedo_blend17.rgb;
			float4 lerpResult33 = lerp( tex2D( _Roughness_01, Tiling_01104 ) , tex2D( _Roughness_02, Tiling_02109 ) , VC_R9);
			float4 lerpResult35 = lerp( lerpResult33 , tex2D( _Roughness_03, Tiling_03124 ) , VC_G10);
			float4 lerpResult37 = lerp( lerpResult35 , tex2D( _Roughness_04, Tiling_04119 ) , VC_B12);
			float4 Roughness_Blend38 = ( 1.0 - lerpResult37 );
			o.Smoothness = Roughness_Blend38.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
43;50;1739;944;3713.971;-1169.624;2.32357;True;False
Node;AmplifyShaderEditor.CommentaryNode;94;-4529.667,1853.885;Float;False;1402.244;1213.775;Comment;20;119;109;115;105;118;117;116;108;107;106;104;103;101;102;100;121;120;122;124;123;Tiling;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-4508.762,1996.574;Float;False;Property;_Tiling_01;Tiling_01;3;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-4499.939,2266.597;Float;False;Property;_Tiling_02;Tiling_02;7;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-4517.192,2558.025;Float;False;Property;_Tiling_03;Tiling_03;11;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;101;-4086.836,1923.451;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-4078.013,2193.474;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-4083.637,2335.152;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;102;-4092.46,2065.131;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-4508.558,2847.471;Float;False;Property;_Tiling_04;Tiling_04;15;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;16;-4516.202,1351.997;Float;False;541.262;333.0001;Comment;4;1;9;10;12;VertexColors;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;103;-3712.724,1990.087;Float;False;Property;_LAYER_01_ISUV2;LAYER_01_ISUV2;16;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;121;-4095.266,2484.902;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;120;-4100.89,2626.58;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;108;-3703.903,2260.109;Float;False;Property;_LAYER_02_ISUV2;LAYER_02_ISUV2;18;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;-4092.259,2916.024;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;122;-3721.154,2551.537;Float;False;Property;_LAYER_03_ISUV2;LAYER_03_ISUV2;17;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;-3360.702,2240.61;Float;True;Tiling_02;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-4086.635,2774.349;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;104;-3369.523,1970.587;Float;True;Tiling_01;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;1;-4466.201,1429.193;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;90;-3083.936,551.5255;Float;False;109;Tiling_02;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-4221.938,1401.997;Float;False;VC_R;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;118;-3712.522,2840.982;Float;False;Property;_LAYER_04_ISUV2;LAYER_04_ISUV2;19;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-3083.656,353.7461;Float;False;104;Tiling_01;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;42;-1373.891,601.049;Float;False;1538.55;657.9575;Comment;8;32;33;34;35;36;37;38;40;Roughness_Blend;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;124;-3377.953,2532.038;Float;True;Tiling_03;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2126.09,466.6744;Float;False;381.9905;919.2676;Comment;4;23;22;21;20;Roughness;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-3369.321,2821.483;Float;True;Tiling_04;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-4217.938,1481.997;Float;False;VC_G;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-2146.792,-1151.184;Float;False;1969.162;980.0751;Comment;11;17;7;8;5;15;14;6;4;3;13;2;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;-3100.259,840.7197;Float;False;124;Tiling_03;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-1323.891,749.6207;Float;False;9;VC_R;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;20;-2065.099,516.6744;Float;True;Property;_Roughness_01;Roughness_01;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;67;-1468.556,2163.428;Float;False;1512.551;657.957;Comment;7;55;56;57;58;59;61;54;Normal_Blend;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;66;-2083.185,2103.212;Float;False;395.6165;915.1433;Comment;4;62;63;64;65;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;21;-2067.217,736.9412;Float;True;Property;_Roughness_02;Roughness_02;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;54;-1418.556,2312.001;Float;False;9;VC_R;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-2104.781,-871.9896;Float;True;Property;_Albedo_02;Albedo_02;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;13;-1440.479,-766.2246;Float;False;9;VC_R;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2112.005,-1080.767;Float;True;Property;_Albedo_01;Albedo_01;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-2069.308,950.2283;Float;True;Property;_Roughness_03;Roughness_03;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-4228.939,1569.997;Float;False;VC_B;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1108.36,651.049;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;62;-2033.186,2153.212;Float;True;Property;_Normal_01;Normal_01;1;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;92;-3100.502,1127.257;Float;False;119;Tiling_04;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;63;-2020.877,2362.462;Float;True;Property;_Normal_02;Normal_02;5;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;34;-1062.991,959.7206;Float;False;10;VC_G;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-2076.09,1155.942;Float;True;Property;_Roughness_04;Roughness_04;14;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;14;-1179.578,-556.1246;Float;False;10;VC_G;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2106.209,-632.5938;Float;True;Property;_Albedo_03;Albedo_03;8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;6;-1224.948,-864.7963;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-943.4623,1144.006;Float;False;12;VC_B;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;64;-2015.954,2584.025;Float;True;Property;_Normal_03;Normal_03;9;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;56;-1203.025,2213.428;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-1157.656,2522.101;Float;False;10;VC_G;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;-868.9627,818.0143;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;-985.5505,-697.8311;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-2110.562,-400.6987;Float;True;Property;_Albedo_04;Albedo_04;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;65;-2008.57,2788.355;Float;True;Property;_Normal_04;Normal_04;13;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;-638.0139,1051.236;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-1038.127,2706.385;Float;False;12;VC_B;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;57;-963.6274,2380.395;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1060.051,-371.8382;Float;False;12;VC_B;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;-732.6786,2613.615;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;40;-353.3298,1075.835;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;7;-754.6015,-464.6092;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-104.3403,1058.259;Float;False;Roughness_Blend;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-199.0049,2621.639;Float;False;Normal_Blend;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;93;978.0878,324.2677;Float;False;667.2334;539.4107;Comment;4;39;68;18;0;Output;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-451.4594,-426.3274;Float;False;Albedo_blend;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;1061.292,395.1324;Float;False;17;Albedo_blend;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;1039.489,645.6055;Float;False;38;Roughness_Blend;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;1069.697,491.1694;Float;False;61;Normal_Blend;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1412.155,403.2789;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;VertexPaintShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;101;0;100;0
WireConnection;107;0;105;0
WireConnection;106;0;105;0
WireConnection;102;0;100;0
WireConnection;103;1;101;0
WireConnection;103;0;102;0
WireConnection;121;0;123;0
WireConnection;120;0;123;0
WireConnection;108;1;107;0
WireConnection;108;0;106;0
WireConnection;116;0;115;0
WireConnection;122;1;121;0
WireConnection;122;0;120;0
WireConnection;109;0;108;0
WireConnection;117;0;115;0
WireConnection;104;0;103;0
WireConnection;9;0;1;1
WireConnection;118;1;117;0
WireConnection;118;0;116;0
WireConnection;124;0;122;0
WireConnection;119;0;118;0
WireConnection;10;0;1;2
WireConnection;20;1;89;0
WireConnection;21;1;90;0
WireConnection;3;1;90;0
WireConnection;2;1;89;0
WireConnection;22;1;91;0
WireConnection;12;0;1;3
WireConnection;33;0;20;0
WireConnection;33;1;21;0
WireConnection;33;2;32;0
WireConnection;62;1;89;0
WireConnection;63;1;90;0
WireConnection;23;1;92;0
WireConnection;4;1;91;0
WireConnection;6;0;2;0
WireConnection;6;1;3;0
WireConnection;6;2;13;0
WireConnection;64;1;91;0
WireConnection;56;0;62;0
WireConnection;56;1;63;0
WireConnection;56;2;54;0
WireConnection;35;0;33;0
WireConnection;35;1;22;0
WireConnection;35;2;34;0
WireConnection;8;0;6;0
WireConnection;8;1;4;0
WireConnection;8;2;14;0
WireConnection;5;1;92;0
WireConnection;65;1;92;0
WireConnection;37;0;35;0
WireConnection;37;1;23;0
WireConnection;37;2;36;0
WireConnection;57;0;56;0
WireConnection;57;1;64;0
WireConnection;57;2;55;0
WireConnection;59;0;57;0
WireConnection;59;1;65;0
WireConnection;59;2;58;0
WireConnection;40;0;37;0
WireConnection;7;0;8;0
WireConnection;7;1;5;0
WireConnection;7;2;15;0
WireConnection;38;0;40;0
WireConnection;61;0;59;0
WireConnection;17;0;7;0
WireConnection;0;0;18;0
WireConnection;0;1;68;0
WireConnection;0;4;39;0
ASEEND*/
//CHKSM=DA01F20BEBC716156269485493F18EE4760DF8C9