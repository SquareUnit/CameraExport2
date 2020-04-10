// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TATShaders/FX/FX_Standard"
{
	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_SRC("SRC", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_DST("DST", Float) = 1
		_Emissive("Emissive", 2D) = "white" {}
		[Toggle(_SELFBLENDEDEMISSIVEON_ON)] _SelfBlendedEmissiveON("Self Blended Emissive ON?", Float) = 0
		[Toggle(_USEALPHACHANNEL_ON_ON)] _USEALPHACHANNEL_ON("_USEALPHACHANNEL_ON", Float) = 0
		[HDR]_Color("Color", Color) = (0.1098039,0.04313726,0.454902,1)
		_Mask("Mask", 2D) = "white" {}
		_Emissive1XYEmissive2ZW("Emissive1  (XY) / Emissive2 (ZW)", Vector) = (0,0,0,0)
		_MaskXYAnchorsZRotTimeW("Mask (XY) / Anchors (Z) / RotTime (W)", Vector) = (0,0,0,0)
		_TimeScale("TimeScale", Float) = 1
		_Offset("Offset", Vector) = (0,0,0,0)
		[Toggle(_DEPTHFADE_ON_ON)] _DEPTHFADE_ON("_DEPTHFADE_ON", Float) = 0
		_DepthDistance("Depth Distance", Range( 0.1 , 1)) = 0.5
		[KeywordEnum(XZ,XY,ZX)] _AXISSWITCH("_AXISSWITCH", Float) = 0
		[Toggle(_CONTINUOUSROTATION_ON_ON)] _CONTINUOUSROTATION_ON("_CONTINUOUSROTATION_ON", Float) = 0
		[Toggle(_WORLDPOSITION_ON_ON)] _WORLDPOSITION_ON("_WORLDPOSITION_ON", Float) = 0
		_EmissiveWPTilingXYEmissiveWPTilingZW("Emissive WP Tiling (XY) / Emissive WP Tiling (ZW)", Vector) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_CullMode]
		ZWrite Off
		Blend [_SRC] [_DST]
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _SELFBLENDEDEMISSIVEON_ON
		#pragma shader_feature _WORLDPOSITION_ON_ON
		#pragma shader_feature _AXISSWITCH_XZ _AXISSWITCH_XY _AXISSWITCH_ZX
		#pragma shader_feature _USEALPHACHANNEL_ON_ON
		#pragma shader_feature _CONTINUOUSROTATION_ON_ON
		#pragma shader_feature _DEPTHFADE_ON_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float _CullMode;
		uniform float _SRC;
		uniform float _DST;
		uniform float3 _Offset;
		uniform float4 _Color;
		uniform sampler2D _Emissive;
		uniform float _TimeScale;
		uniform float4 _Emissive1XYEmissive2ZW;
		uniform float4 _EmissiveWPTilingXYEmissiveWPTilingZW;
		uniform sampler2D _Mask;
		uniform float4 _MaskXYAnchorsZRotTimeW;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += _Offset;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 temp_cast_0 = (0.0).xxxx;
			float mulTime26 = _Time.y * _TimeScale;
			float2 appendResult39 = (float2(_Emissive1XYEmissive2ZW.x , _Emissive1XYEmissive2ZW.y));
			float3 ase_worldPos = i.worldPos;
			float2 appendResult130 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.y));
			float2 appendResult132 = (float2(ase_worldPos.x , ase_worldPos.z));
			#if defined(_AXISSWITCH_XZ)
				float2 staticSwitch136 = appendResult130;
			#elif defined(_AXISSWITCH_XY)
				float2 staticSwitch136 = appendResult131;
			#elif defined(_AXISSWITCH_ZX)
				float2 staticSwitch136 = appendResult132;
			#else
				float2 staticSwitch136 = appendResult130;
			#endif
			float2 WPAxis137 = staticSwitch136;
			float2 appendResult122 = (float2(_EmissiveWPTilingXYEmissiveWPTilingZW.x , _EmissiveWPTilingXYEmissiveWPTilingZW.y));
			#ifdef _WORLDPOSITION_ON_ON
				float2 staticSwitch140 = ( WPAxis137 * appendResult122 );
			#else
				float2 staticSwitch140 = i.uv_texcoord;
			#endif
			float2 panner20 = ( mulTime26 * appendResult39 + staticSwitch140);
			float4 tex2DNode78 = tex2D( _Emissive, panner20 );
			float2 appendResult40 = (float2(_Emissive1XYEmissive2ZW.z , _Emissive1XYEmissive2ZW.w));
			float2 panner37 = ( mulTime26 * appendResult40 + ( staticSwitch140 * 0.5 ));
			#ifdef _SELFBLENDEDEMISSIVEON_ON
				float4 staticSwitch80 = ( tex2DNode78 * tex2D( _Emissive, panner37 ) );
			#else
				float4 staticSwitch80 = tex2DNode78;
			#endif
			float4 lerpResult52 = lerp( temp_cast_0 , ( _Color * staticSwitch80 ) , i.vertexColor);
			float4 temp_cast_1 = (0.0).xxxx;
			float2 appendResult86 = (float2(_MaskXYAnchorsZRotTimeW.x , _MaskXYAnchorsZRotTimeW.y));
			float2 appendResult143 = (float2(_EmissiveWPTilingXYEmissiveWPTilingZW.z , _EmissiveWPTilingXYEmissiveWPTilingZW.w));
			#ifdef _WORLDPOSITION_ON
				float2 staticSwitch154 = ( WPAxis137 * appendResult143 );
			#else
				float2 staticSwitch154 = i.uv_texcoord;
			#endif
			float2 panner83 = ( 1.0 * _Time.y * appendResult86 + staticSwitch154);
			float2 temp_cast_2 = (_MaskXYAnchorsZRotTimeW.z).xx;
			float mulTime103 = _Time.y * _MaskXYAnchorsZRotTimeW.w;
			#ifdef _CONTINUOUSROTATION_ON_ON
				float staticSwitch152 = mulTime103;
			#else
				float staticSwitch152 = radians( _MaskXYAnchorsZRotTimeW.w );
			#endif
			float cos102 = cos( staticSwitch152 );
			float sin102 = sin( staticSwitch152 );
			float2 rotator102 = mul( panner83 - temp_cast_2 , float2x2( cos102 , -sin102 , sin102 , cos102 )) + temp_cast_2;
			float4 tex2DNode34 = tex2D( _Mask, rotator102 );
			float4 temp_cast_3 = (tex2DNode34.a).xxxx;
			#ifdef _USEALPHACHANNEL_ON_ON
				float4 staticSwitch157 = temp_cast_3;
			#else
				float4 staticSwitch157 = tex2DNode34;
			#endif
			float4 lerpResult55 = lerp( temp_cast_1 , staticSwitch157 , i.vertexColor.a);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth56 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth56 = abs( ( screenDepth56 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			#ifdef _DEPTHFADE_ON_ON
				float staticSwitch155 = saturate( distanceDepth56 );
			#else
				float staticSwitch155 = 1.0;
			#endif
			o.Emission = ( lerpResult52 * lerpResult55 * staticSwitch155 ).rgb;
			o.Alpha = ( _Color.a * i.vertexColor.a * lerpResult55 * staticSwitch155 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
116;88;1549;1004;5308.631;498.9025;1.696836;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;127;-4382.158,821.4103;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;132;-4047.846,960.5558;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;131;-4051.758,838.0894;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;130;-4051.067,701.3819;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;136;-3840.366,725.1395;Float;False;Property;_AXISSWITCH;_AXISSWITCH;15;0;Create;True;0;0;False;0;0;0;0;True;;KeywordEnum;3;XZ;XY;ZX;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;121;-4725.344,577.5358;Float;False;Property;_EmissiveWPTilingXYEmissiveWPTilingZW;Emissive WP Tiling (XY) / Emissive WP Tiling (ZW);18;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;137;-3504.311,781.014;Float;False;WPAxis;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;122;-4378.562,196.6448;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-4624.161,4.50696;Float;False;137;WPAxis;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;139;-4084.714,240.6287;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-4183.854,91.81049;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;140;-3796.054,105.88;Float;False;Property;_WORLDPOSITION_ON;_WORLDPOSITION_ON;17;0;Create;True;0;0;False;0;0;0;0;True;_WORLDPOSITION_ON;Toggle;2;Key0;Key1;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;23;-4375.992,-741.6536;Float;False;Property;_Emissive1XYEmissive2ZW;Emissive1  (XY) / Emissive2 (ZW);8;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-3674.354,-631.0215;Float;False;Property;_TimeScale;TimeScale;11;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-3727.095,922.2977;Float;False;Constant;_2ndEmissiveScale;2nd Emissive Scale;16;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;-3108.568,-100.1459;Float;False;137;WPAxis;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;143;-3117.967,367.3985;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-3373.842,24.01376;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-3950.927,-757.2864;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-2850.862,-284.2653;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;-3950.927,-629.2864;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;85;-2548.921,-7.360021;Float;False;Property;_MaskXYAnchorsZRotTimeW;Mask (XY) / Anchors (Z) / RotTime (W);10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;26;-3482.354,-631.0215;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;149;-3162.808,34.36832;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;86;-2190.912,-91.94741;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;77;-3079.585,-589.5848;Float;True;Property;_Emissive;Emissive;3;0;Create;True;0;0;False;0;None;0c92f7a19e20e8c40ad2006d71d1a646;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RadiansOpNode;151;-1994.194,42.59122;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-3084.389,-317.4731;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;20;-3088,-784;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;154;-2648.13,-206.5352;Float;False;Property;_Keyword0;Keyword 0;17;0;Fetch;True;0;0;True;0;0;0;0;False;_WORLDPOSITION_ON;Toggle;2;Key0;Key1;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;103;-2005.851,172.8111;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;79;-2768,-608;Float;True;Property;_TextureSample1;Texture Sample 1;17;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;152;-1667.077,46.17729;Float;False;Property;_CONTINUOUSROTATION_ON;_CONTINUOUSROTATION_ON;16;0;Create;True;0;0;False;0;0;0;0;True;_CONTINUOUSROTATION_ON;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;78;-2752,-848;Float;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;83;-2050.51,-245.9684;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-2352.915,-726.5662;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;102;-1766.195,-247.2402;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1672.466,271.4986;Float;False;Property;_DepthDistance;Depth Distance;14;0;Create;True;0;0;False;0;0.5;0.5;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;56;-1368.466,255.4986;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;80;-2008.931,-841.694;Float;False;Property;_SelfBlendedEmissiveON;Self Blended Emissive ON?;4;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-1936,-1024;Float;False;Property;_Color;Color;6;1;[HDR];Create;True;0;0;False;0;0.1098039,0.04313726,0.454902,1;0.5,0.5,0.5,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-1527.195,-237.2402;Float;True;Property;_Mask;Mask;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;76;-1115.991,251.7676;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-1120.725,88.61011;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;12;-1600,-512;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-1020,-322;Float;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1431.964,-862.8685;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1200.241,-968.0732;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;157;-1157.935,-152.6559;Float;False;Property;_USEALPHACHANNEL_ON;_USEALPHACHANNEL_ON;5;0;Create;True;0;0;False;0;0;0;0;True;_USEALPHACHANNEL_ON;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;55;-799.379,-283.7319;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;155;-852.5921,120.5753;Float;False;Property;_DEPTHFADE_ON;_DEPTHFADE_ON;13;0;Create;True;0;0;False;0;0;0;0;True;_DEPTHFADE_ON;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;-1026.627,-872.1285;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-535.9852,-370.9135;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-555.8618,-656.8212;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;117;72.11292,-423.2267;Float;False;Property;_SRC;SRC;1;1;[Enum];Create;True;0;1;UnityEngine.Rendering.BlendMode;True;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;72.11292,-327.2267;Float;False;Property;_DST;DST;2;1;[Enum];Create;True;1;Option1;0;1;UnityEngine.Rendering.BlendMode;True;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;72.11292,-519.2266;Float;False;Property;_CullMode;Cull Mode;0;1;[Enum];Create;True;3;Backface;2;Frontface;1;Double Sided;0;1;UnityEngine.Rendering.CullMode;True;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;18;-480.2471,37.54538;Float;False;Property;_Offset;Offset;12;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-228.4872,-511.4266;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;TATShaders/FX/FX_Standard;False;False;False;False;True;True;True;True;True;True;False;True;False;False;True;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;5;4;True;117;1;True;118;0;5;False;-1;10;False;-1;0;False;119;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;9;-1;-1;-1;0;False;0;0;True;16;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;132;0;127;1
WireConnection;132;1;127;3
WireConnection;131;0;127;1
WireConnection;131;1;127;2
WireConnection;130;0;127;1
WireConnection;130;1;127;3
WireConnection;136;1;130;0
WireConnection;136;0;131;0
WireConnection;136;2;132;0
WireConnection;137;0;136;0
WireConnection;122;0;121;1
WireConnection;122;1;121;2
WireConnection;123;0;126;0
WireConnection;123;1;122;0
WireConnection;140;1;139;0
WireConnection;140;0;123;0
WireConnection;143;0;121;3
WireConnection;143;1;121;4
WireConnection;141;0;140;0
WireConnection;141;1;148;0
WireConnection;39;0;23;1
WireConnection;39;1;23;2
WireConnection;153;0;150;0
WireConnection;153;1;143;0
WireConnection;40;0;23;3
WireConnection;40;1;23;4
WireConnection;26;0;27;0
WireConnection;86;0;85;1
WireConnection;86;1;85;2
WireConnection;151;0;85;4
WireConnection;37;0;141;0
WireConnection;37;2;40;0
WireConnection;37;1;26;0
WireConnection;20;0;140;0
WireConnection;20;2;39;0
WireConnection;20;1;26;0
WireConnection;154;1;149;0
WireConnection;154;0;153;0
WireConnection;103;0;85;4
WireConnection;79;0;77;0
WireConnection;79;1;37;0
WireConnection;152;1;151;0
WireConnection;152;0;103;0
WireConnection;78;0;77;0
WireConnection;78;1;20;0
WireConnection;83;0;154;0
WireConnection;83;2;86;0
WireConnection;81;0;78;0
WireConnection;81;1;79;0
WireConnection;102;0;83;0
WireConnection;102;1;85;3
WireConnection;102;2;152;0
WireConnection;56;0;57;0
WireConnection;80;1;78;0
WireConnection;80;0;81;0
WireConnection;34;1;102;0
WireConnection;76;0;56;0
WireConnection;3;0;1;0
WireConnection;3;1;80;0
WireConnection;157;1;34;0
WireConnection;157;0;34;4
WireConnection;55;0;54;0
WireConnection;55;1;157;0
WireConnection;55;2;12;4
WireConnection;155;1;156;0
WireConnection;155;0;76;0
WireConnection;52;0;53;0
WireConnection;52;1;3;0
WireConnection;52;2;12;0
WireConnection;104;0;1;4
WireConnection;104;1;12;4
WireConnection;104;2;55;0
WireConnection;104;3;155;0
WireConnection;42;0;52;0
WireConnection;42;1;55;0
WireConnection;42;2;155;0
WireConnection;0;2;42;0
WireConnection;0;9;104;0
WireConnection;0;11;18;0
ASEEND*/
//CHKSM=029316E546EA3F07BF3061B66EB7CBB1C6C2143F