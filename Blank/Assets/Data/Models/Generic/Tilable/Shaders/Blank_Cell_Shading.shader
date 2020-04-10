// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Blank_Cell_Shading"
{
	Properties
	{
		_Normal_Map("Normal_Map", 2D) = "white" {}
		[Toggle(_KEYWORD0_ON)] _Keyword0("Keyword 0", Float) = 0
		_Indirect_Spec("Indirect_Spec", Range( 0 , 1)) = 1
		_Normal_Scale("Normal_Scale", Range( 0 , 1)) = 1
		_Highlight_Tint("Highlight_Tint", Color) = (0.990566,0.990566,0.990566,0)
		_HighlightCellOffset("Highlight Cell Offset", Range( -1 , -0.5)) = -0.5
		_Highlight_Cell_Sharpness("Highlight_Cell_Sharpness", Range( 0 , 1)) = 0.01
		_Highlight("Highlight", 2D) = "white" {}
		_Base_Cell_Offset("Base_Cell_Offset", Range( -1 , 1)) = 0
		_Indirect_Diffuse("Indirect_Diffuse", Range( 0 , 1)) = 1
		_Base_ColorRGB_Outline_Width("Base_Color (RGB)_Outline_Width", 2D) = "white" {}
		_Base_Tint("Base_Tint", Color) = (1,1,1,0)
		_Rim_Offset("Rim_Offset", Range( 0 , 1)) = 0.6
		_Rim_Power("Rim_Power", Range( 0.01 , 1)) = 0.4
		_OutlineTint("Outline Tint", Color) = (0.5294118,0.5294118,0.5294118,0)
		_OutlineWidth("Outline Width", Range( 0 , 0.2)) = 0.2
		_Emissive("Emissive", 2D) = "white" {}
		_Emissivemultiplier("Emissive multiplier", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_Base_ColorRGB_Outline_Width = v.texcoord * _Base_ColorRGB_Outline_Width_ST.xy + _Base_ColorRGB_Outline_Width_ST.zw;
			float4 tex2DNode77 = tex2Dlod( _Base_ColorRGB_Outline_Width, float4( uv_Base_ColorRGB_Outline_Width, 0, 0.0) );
			float Outline_Custom_Width82 = tex2DNode77.a;
			float outlineVar = ( _OutlineWidth * Outline_Custom_Width82 );
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float3 temp_cast_0 = (1.0).xxx;
			float3 lerpResult54 = lerp( temp_cast_0 , float3(0,0,0) , _Indirect_Diffuse);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_70_0 = ( 1.0 - ( ( 1.0 - 1 ) * _WorldSpaceLightPos0.w ) );
			float2 uv_Normal_Map = i.uv_texcoord * _Normal_Map_ST.xy + _Normal_Map_ST.zw;
			float3 normalizeResult4 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _Normal_Map, uv_Normal_Map ), _Normal_Scale ) )) );
			float3 Normals5 = normalizeResult4;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult9 = dot( Normals5 , ase_worldlightDir );
			float N_dot_L12 = dotResult9;
			float lerpResult72 = lerp( temp_output_70_0 , ( saturate( ( ( N_dot_L12 + _Base_Cell_Offset ) / 0.01 ) ) * 1 ) , 0.5);
			float2 uv_Base_ColorRGB_Outline_Width = i.uv_texcoord * _Base_ColorRGB_Outline_Width_ST.xy + _Base_ColorRGB_Outline_Width_ST.zw;
			float4 tex2DNode77 = tex2D( _Base_ColorRGB_Outline_Width, uv_Base_ColorRGB_Outline_Width );
			float4 Base_Color83 = ( float4( ( ( lerpResult54 * ase_lightColor.a * temp_output_70_0 ) + ( ase_lightColor.rgb * lerpResult72 ) ) , 0.0 ) * (( tex2DNode77 * _Base_Tint )).rgba );
			o.Emission = ( Base_Color83 * float4( (_OutlineTint).rgb , 0.0 ) ).rgb;
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _KEYWORD0_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _Indirect_Diffuse;
		uniform float _Normal_Scale;
		uniform sampler2D _Normal_Map;
		uniform float4 _Normal_Map_ST;
		uniform float _Base_Cell_Offset;
		uniform sampler2D _Base_ColorRGB_Outline_Width;
		uniform float4 _Base_ColorRGB_Outline_Width_ST;
		uniform float4 _Base_Tint;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float _Emissivemultiplier;
		uniform float4 _Highlight_Tint;
		uniform sampler2D _Highlight;
		uniform float4 _Highlight_ST;
		uniform float _Indirect_Spec;
		uniform float _HighlightCellOffset;
		uniform float _Highlight_Cell_Sharpness;
		uniform float _Rim_Offset;
		uniform float _Rim_Power;
		uniform float _OutlineWidth;
		uniform float4 _OutlineTint;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 temp_cast_4 = (1.0).xxx;
			float2 uv_Normal_Map = i.uv_texcoord * _Normal_Map_ST.xy + _Normal_Map_ST.zw;
			float3 normalizeResult4 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _Normal_Map, uv_Normal_Map ), _Normal_Scale ) )) );
			float3 Normals5 = normalizeResult4;
			float3 indirectNormal20 = Normals5;
			float2 uv_Highlight = i.uv_texcoord * _Highlight_ST.xy + _Highlight_ST.zw;
			float4 temp_output_40_0 = ( _Highlight_Tint * tex2D( _Highlight, uv_Highlight ) );
			float temp_output_25_0 = (temp_output_40_0).a;
			Unity_GlossyEnvironmentData g20 = UnityGlossyEnvironmentSetup( temp_output_25_0, data.worldViewDir, indirectNormal20, float3(0,0,0));
			float3 indirectSpecular20 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal20, g20 );
			float3 lerpResult21 = lerp( temp_cast_4 , indirectSpecular20 , _Indirect_Spec);
			float4 Highlight_Color45 = (temp_output_40_0).rgba;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 LightColor_Fallof17 = ( ase_lightColor.rgb * ase_lightAtten );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult4_g1 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float dotResult29 = dot( normalizeResult4_g1 , Normals5 );
			float dotResult9 = dot( Normals5 , ase_worldlightDir );
			float N_dot_L12 = dotResult9;
			#ifdef _KEYWORD0_ON
				float staticSwitch30 = N_dot_L12;
			#else
				float staticSwitch30 = dotResult29;
			#endif
			float3 temp_cast_7 = (1.0).xxx;
			UnityGI gi52 = gi;
			float3 diffNorm52 = Normals5;
			gi52 = UnityGI_Base( data, 1, diffNorm52 );
			float3 indirectDiffuse52 = gi52.indirect.diffuse + diffNorm52 * 0.0001;
			float3 lerpResult54 = lerp( temp_cast_7 , indirectDiffuse52 , _Indirect_Diffuse);
			float temp_output_70_0 = ( 1.0 - ( ( 1.0 - ase_lightAtten ) * _WorldSpaceLightPos0.w ) );
			float lerpResult72 = lerp( temp_output_70_0 , ( saturate( ( ( N_dot_L12 + _Base_Cell_Offset ) / 0.01 ) ) * ase_lightAtten ) , 0.5);
			float2 uv_Base_ColorRGB_Outline_Width = i.uv_texcoord * _Base_ColorRGB_Outline_Width_ST.xy + _Base_ColorRGB_Outline_Width_ST.zw;
			float4 tex2DNode77 = tex2D( _Base_ColorRGB_Outline_Width, uv_Base_ColorRGB_Outline_Width );
			float4 Base_Color83 = ( float4( ( ( lerpResult54 * ase_lightColor.a * temp_output_70_0 ) + ( ase_lightColor.rgb * lerpResult72 ) ) , 0.0 ) * (( tex2DNode77 * _Base_Tint )).rgba );
			float dotResult86 = dot( Normals5 , ase_worldViewDir );
			float4 color97 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			c.rgb = ( ( float4( lerpResult21 , 0.0 ) * Highlight_Color45 * float4( LightColor_Fallof17 , 0.0 ) * pow( temp_output_25_0 , 1.0 ) * saturate( ( ( staticSwitch30 + _HighlightCellOffset ) / ( ( 1.0 - temp_output_25_0 ) * _Highlight_Cell_Sharpness ) ) ) ) + Base_Color83 + ( ( saturate( N_dot_L12 ) * pow( ( 1.0 - saturate( ( dotResult86 + _Rim_Offset ) ) ) , _Rim_Power ) ) * Highlight_Color45 * float4( LightColor_Fallof17 , 0.0 ) * (color97).rgba ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float3 temp_cast_0 = (1.0).xxx;
			float3 lerpResult54 = lerp( temp_cast_0 , float3(0,0,0) , _Indirect_Diffuse);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_70_0 = ( 1.0 - ( ( 1.0 - 1 ) * _WorldSpaceLightPos0.w ) );
			float2 uv_Normal_Map = i.uv_texcoord * _Normal_Map_ST.xy + _Normal_Map_ST.zw;
			float3 normalizeResult4 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _Normal_Map, uv_Normal_Map ), _Normal_Scale ) )) );
			float3 Normals5 = normalizeResult4;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult9 = dot( Normals5 , ase_worldlightDir );
			float N_dot_L12 = dotResult9;
			float lerpResult72 = lerp( temp_output_70_0 , ( saturate( ( ( N_dot_L12 + _Base_Cell_Offset ) / 0.01 ) ) * 1 ) , 0.5);
			float2 uv_Base_ColorRGB_Outline_Width = i.uv_texcoord * _Base_ColorRGB_Outline_Width_ST.xy + _Base_ColorRGB_Outline_Width_ST.zw;
			float4 tex2DNode77 = tex2D( _Base_ColorRGB_Outline_Width, uv_Base_ColorRGB_Outline_Width );
			float4 Base_Color83 = ( float4( ( ( lerpResult54 * ase_lightColor.a * temp_output_70_0 ) + ( ase_lightColor.rgb * lerpResult72 ) ) , 0.0 ) * (( tex2DNode77 * _Base_Tint )).rgba );
			o.Albedo = Base_Color83.rgb;
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			o.Emission = ( tex2D( _Emissive, uv_Emissive ) * _Emissivemultiplier ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
103;50;1739;926;-2302.028;-982.8467;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;6;-3453.192,-382.5463;Float;False;1424.901;309.4003;;5;1;3;4;5;2;Normals;0.2580141,0.1504094,0.490566,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-3403.192,-321.8461;Float;False;Property;_Normal_Scale;Normal_Scale;3;0;Create;True;0;0;False;0;1;0.73;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-3100.891,-332.5462;Float;True;Property;_Normal_Map;Normal_Map;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;3;-2789.692,-326.1458;Float;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;4;-2543.291,-322.9457;Float;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;13;-3452.333,-28.33962;Float;False;826.5096;440.35;;4;12;11;7;9;N dot L;0.9339623,0.390307,0.07489321,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-2271.291,-326.1458;Float;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;11;-3395.339,218.0003;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;7;-3382.148,21.66039;Float;True;5;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;9;-3104.773,27.56796;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;84;-2306.8,2253.604;Float;False;3230.837;1011.379;;25;78;82;80;79;83;81;77;62;76;58;60;59;63;64;66;71;75;65;74;73;69;70;72;68;67;Base_Color;0.5887327,0.9245283,0.87352,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-2855.545,24.12753;Float;False;N_dot_L;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-2256.8,2566.835;Float;False;Property;_Base_Cell_Offset;Base_Cell_Offset;8;0;Create;True;0;0;False;0;0;-0.26;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-2190.513,2372.02;Float;False;12;N_dot_L;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;66;-1502.293,2624.708;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1933.894,2594.807;Float;False;Constant;_Base_Cell_Sharpness;Base_Cell_Sharpness;10;0;Create;True;0;0;False;0;0.01;0;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1865.799,2369.034;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;68;-1192.892,2623.405;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;62;-1607.592,2368.608;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;56;-1477.829,1422.128;Float;False;947.9659;755.3997;Comment;5;54;55;52;53;51;Indirect_Diffuse;1,0.8749992,0.5896226,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;67;-1532.192,2818.407;Float;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;48;-1810.549,402.7346;Float;False;2235.605;977.1207;Comment;19;34;32;33;27;29;31;30;28;40;38;39;43;44;45;46;35;37;42;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;102;-1493.118,3340.689;Float;False;2509.325;815.5095;Comment;17;88;86;85;87;89;90;91;92;93;94;95;96;97;98;99;100;101;Rim_Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;64;-1385.292,2369.908;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-1355.829,1703.746;Float;False;5;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-936.7919,2689.705;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1425.902,3673.338;Float;False;5;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;39;-1701.402,644.0446;Float;True;Property;_Highlight;Highlight;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;87;-1443.118,3869.146;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.IndirectDiffuseLighting;52;-1133.106,1703.528;Float;False;World;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1166.21,1929.297;Float;False;Property;_Indirect_Diffuse;Indirect_Diffuse;9;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1062.893,2366.008;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-814.5914,2914.61;Float;False;Constant;_Shadow_Contribution;Shadow_Contribution;10;0;Create;True;0;0;False;0;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;70;-709.2922,2689.706;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1095.556,1484.128;Float;False;Constant;_Float2;Float 2;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;38;-1656.791,452.7346;Float;False;Property;_Highlight_Tint;Highlight_Tint;4;0;Create;True;0;0;False;0;0.990566,0.990566,0.990566,0;1,0.8236647,0.7168476,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;54;-845.165,1704.256;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;72;-471.3918,2650.705;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;78;-359.4371,3079.84;Float;False;Property;_Base_Tint;Base_Tint;11;0;Create;True;0;0;False;0;1,1,1,0;0.9150943,0.8743021,0.8330811,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;28;-1760.549,899.2303;Float;False;Blinn-Phong Half Vector;-1;;1;91a149ac9d615be429126c95e20753ce;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;71;-646.892,2303.604;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;77;-475.9378,2884.943;Float;True;Property;_Base_ColorRGB_Outline_Width;Base_Color (RGB)_Outline_Width;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;86;-1168.792,3674.444;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-1727.184,1090.741;Float;False;5;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-1171.792,3889.345;Float;False;Property;_Rim_Offset;Rim_Offset;12;0;Create;True;0;0;False;0;0.6;0.632;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1371.417,515.3973;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;26;-1103.969,45.24078;Float;False;299;250;Comment;1;25;Spec/Smooth;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-172.3911,2628.609;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-169.7913,2373.808;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;18;-3452.317,484.39;Float;False;871.9271;432.5496;Comment;4;17;14;16;15;Light_Fallof;1,0.8204617,0.1556604,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-71.40503,2824.612;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;29;-1486.457,898.4681;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;25;-1053.969,95.24074;Float;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-1486.835,1124.25;Float;False;12;N_dot_L;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-845.2445,3675.845;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;80;153.6895,2825.644;Float;False;True;True;True;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;24;-957.9121,-760.4868;Float;False;1020.014;705.3157;Comment;5;21;20;23;22;19;Indirect_Specular;0.9622642,0.5582948,0.7448327,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;88.67663,2520.822;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;15;-3371.84,723.9678;Float;True;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;30;-1261.549,900.2303;Float;False;Property;_Keyword0;Keyword 0;1;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-798.2283,1133.113;Float;False;Property;_Highlight_Cell_Sharpness;Highlight_Cell_Sharpness;6;0;Create;True;0;0;False;0;0.01;0.445;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;90;-611.665,3675.199;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-724.9372,701.8843;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1266.311,1135.238;Float;False;Property;_HighlightCellOffset;Highlight Cell Offset;5;0;Create;True;0;0;False;0;-0.5;-0.712;-1;-0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;14;-3402.317,534.39;Float;True;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;112;1362.912,3266.592;Float;False;1254.708;918.6167;Comment;8;104;105;107;106;108;110;109;111;Custom_Outline;0.7924528,0.2953008,0.4443123,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;91;-429.6651,3674.199;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-926.912,-467.678;Float;False;5;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;44;-555.5112,504.026;Float;False;True;True;True;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-496.4691,1058.72;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-939.6396,904.1328;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-3087.39,562.4273;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-533.6651,3898.199;Float;False;Property;_Rim_Power;Rim_Power;13;0;Create;True;0;0;False;0;0.4;1;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;424.2682,2753.404;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-504.09,3442.68;Float;False;12;N_dot_L;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;689.1432,2744.65;Float;False;Base_Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IndirectSpecularLight;20;-605.251,-491.8885;Float;False;World;3;0;FLOAT3;0,0,1;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;82;-64.8851,3040.443;Float;False;Outline_Custom_Width;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;95;-218.1936,3440.985;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;104;1412.912,3507.483;Float;False;Property;_OutlineTint;Outline Tint;14;0;Create;True;0;0;False;0;0.5294118,0.5294118,0.5294118,0;0.1860982,0.7208704,0.7735849,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-2842.39,553.4271;Float;False;LightColor_Fallof;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-504.3038,-715.4866;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;97;68.80635,3798.985;Float;False;Constant;_Rime_Color;Rime_Color;14;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-251.4238,503.8678;Float;False;Highlight_Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-574.9774,-283.1643;Float;False;Property;_Indirect_Spec;Indirect_Spec;2;0;Create;True;0;0;False;0;1;0.701;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;36;-236.482,932.69;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;92;-244.665,3673.199;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;1713.108,3316.592;Float;False;83;Base_Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;42;-11.90187,929.55;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;1709.971,3733.999;Float;False;Property;_OutlineWidth;Outline Width;15;0;Create;True;0;0;False;0;0.2;0.02;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;21;-240.1133,-504.0208;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;98;357.8065,3796.985;Float;False;True;True;True;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;68.80635,3544.985;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;395.5151,3390.689;Float;False;45;Highlight_Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;1841.076,3955.208;Float;False;82;Outline_Custom_Width;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;47;-524.3595,112.6732;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;107;1677.555,3507.343;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;391.6152,3605.191;Float;False;17;LightColor_Fallof;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-204.6329,707.184;Float;False;17;LightColor_Fallof;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;116;2830.028,1485.847;Float;False;Property;_Emissivemultiplier;Emissive multiplier;17;0;Create;True;0;0;False;0;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;211.5687,515.506;Float;False;5;5;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;2003.055,3507.259;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0.3382353,0.3382353,0.3382353;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;780.2065,3518.885;Float;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;114;2672.028,1280.847;Float;True;Property;_Emissive;Emissive;16;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;2097.089,3764.312;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;3078.028,1408.847;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;111;2367.62,3647.037;Float;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;103;2325.367,1495.144;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;2799.366,1193.699;Float;False;83;Base_Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;50;3363.152,1343.784;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Blank_Cell_Shading;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;5;2;0
WireConnection;3;0;1;0
WireConnection;4;0;3;0
WireConnection;5;0;4;0
WireConnection;9;0;7;0
WireConnection;9;1;11;0
WireConnection;12;0;9;0
WireConnection;59;0;58;0
WireConnection;59;1;60;0
WireConnection;68;0;66;0
WireConnection;62;0;59;0
WireConnection;62;1;63;0
WireConnection;64;0;62;0
WireConnection;69;0;68;0
WireConnection;69;1;67;2
WireConnection;52;0;51;0
WireConnection;65;0;64;0
WireConnection;65;1;66;0
WireConnection;70;0;69;0
WireConnection;54;0;55;0
WireConnection;54;1;52;0
WireConnection;54;2;53;0
WireConnection;72;0;70;0
WireConnection;72;1;65;0
WireConnection;72;2;73;0
WireConnection;86;0;85;0
WireConnection;86;1;87;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;74;0;71;1
WireConnection;74;1;72;0
WireConnection;75;0;54;0
WireConnection;75;1;71;2
WireConnection;75;2;70;0
WireConnection;79;0;77;0
WireConnection;79;1;78;0
WireConnection;29;0;28;0
WireConnection;29;1;27;0
WireConnection;25;0;40;0
WireConnection;88;0;86;0
WireConnection;88;1;89;0
WireConnection;80;0;79;0
WireConnection;76;0;75;0
WireConnection;76;1;74;0
WireConnection;30;1;29;0
WireConnection;30;0;31;0
WireConnection;90;0;88;0
WireConnection;37;0;25;0
WireConnection;91;0;90;0
WireConnection;44;0;40;0
WireConnection;35;0;37;0
WireConnection;35;1;34;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;16;0;14;1
WireConnection;16;1;15;0
WireConnection;81;0;76;0
WireConnection;81;1;80;0
WireConnection;83;0;81;0
WireConnection;20;0;19;0
WireConnection;20;1;25;0
WireConnection;82;0;77;4
WireConnection;95;0;94;0
WireConnection;17;0;16;0
WireConnection;45;0;44;0
WireConnection;36;0;32;0
WireConnection;36;1;35;0
WireConnection;92;0;91;0
WireConnection;92;1;93;0
WireConnection;42;0;36;0
WireConnection;21;0;22;0
WireConnection;21;1;20;0
WireConnection;21;2;23;0
WireConnection;98;0;97;0
WireConnection;96;0;95;0
WireConnection;96;1;92;0
WireConnection;47;0;25;0
WireConnection;107;0;104;0
WireConnection;43;0;21;0
WireConnection;43;1;45;0
WireConnection;43;2;46;0
WireConnection;43;3;47;0
WireConnection;43;4;42;0
WireConnection;109;0;105;0
WireConnection;109;1;107;0
WireConnection;99;0;96;0
WireConnection;99;1;100;0
WireConnection;99;2;101;0
WireConnection;99;3;98;0
WireConnection;110;0;106;0
WireConnection;110;1;108;0
WireConnection;115;0;114;0
WireConnection;115;1;116;0
WireConnection;111;0;109;0
WireConnection;111;1;110;0
WireConnection;103;0;43;0
WireConnection;103;1;83;0
WireConnection;103;2;99;0
WireConnection;50;0;113;0
WireConnection;50;2;115;0
WireConnection;50;13;103;0
WireConnection;50;11;111;0
ASEEND*/
//CHKSM=0A3BD9C5020D1DBDDD2C51395A91E4FFE0998BA8