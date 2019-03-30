// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MK4/Rocks desert"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo05default("Albedo (0,5 default)", Range( 0 , 1)) = 0.5
		_RockAlbedo("Rock Albedo", 2D) = "white" {}
		_RockNormal("Rock Normal", 2D) = "bump" {}
		_NormalPower("Normal Power", Range( 0 , 1)) = 0.5
		_RockMetallicGloss("Rock Metallic Gloss", 2D) = "black" {}
		_Ambient("Ambient", 2D) = "white" {}
		_AOPower("AO Power", Range( 0 , 1)) = 0.5
		_Microdetail("Microdetail", 2D) = "white" {}
		_MicrodetailNormal("Microdetail Normal", 2D) = "bump" {}
		_MicrodetailAlbedo("Microdetail Albedo", Range( 0 , 1)) = 0.5
		_MicrodetailTiling("Microdetail Tiling", Range( 0 , 1)) = 0.5
		_MicrodetailNormalPower("Microdetail Normal Power", Range( 0 , 1)) = 0.5
		_CoverAlbedo("Cover Albedo", 2D) = "white" {}
		_CoverAmount("Cover Amount", Range( 0 , 1)) = 0.5
		_CoverNormal("Cover Normal", 2D) = "bump" {}
		_CoverTiling("Cover Tiling", Range( 0 , 1)) = 0.5
		_CoverNormalPower("Cover Normal Power", Range( 0 , 1)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float2 texcoord_0;
			float2 texcoord_1;
		};

		uniform float _NormalPower;
		uniform sampler2D _RockNormal;
		uniform float4 _RockNormal_ST;
		uniform float _CoverNormalPower;
		uniform sampler2D _CoverNormal;
		uniform float _CoverTiling;
		uniform float _MicrodetailNormalPower;
		uniform sampler2D _MicrodetailNormal;
		uniform float _MicrodetailTiling;
		uniform sampler2D _Ambient;
		uniform float4 _Ambient_ST;
		uniform float _CoverAmount;
		uniform sampler2D _Microdetail;
		uniform float _MicrodetailAlbedo;
		uniform sampler2D _RockAlbedo;
		uniform float4 _RockAlbedo_ST;
		uniform float _Albedo05default;
		uniform sampler2D _CoverAlbedo;
		uniform sampler2D _RockMetallicGloss;
		uniform float4 _RockMetallicGloss_ST;
		uniform float _AOPower;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = ((0.1 + (_CoverTiling - 0.0) * (6.0 - 0.1) / (1.0 - 0.0))).xx;
			o.texcoord_0.xy = v.texcoord.xy * temp_cast_0 + float2( 0,0 );
			float2 temp_cast_1 = ((1.0 + (_MicrodetailTiling - 0.0) * (20.0 - 1.0) / (1.0 - 0.0))).xx;
			o.texcoord_1.xy = v.texcoord.xy * temp_cast_1 + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_RockNormal = i.uv_texcoord * _RockNormal_ST.xy + _RockNormal_ST.zw;
			float3 tex2DNode4 = UnpackScaleNormal( tex2D( _RockNormal, uv_RockNormal ) ,(0.0 + (_NormalPower - 0.0) * (2.0 - 0.0) / (1.0 - 0.0)) );
			float2 uv_Ambient = i.uv_texcoord * _Ambient_ST.xy + _Ambient_ST.zw;
			float3 desaturateVar83 = lerp( tex2D( _Ambient, uv_Ambient ).xyz,dot(tex2D( _Ambient, uv_Ambient ).xyz,float3(0.299,0.587,0.114)),0.0);
			float3 temp_cast_1 = ((1.2 + (_CoverAmount - 0.0) * (-4.0 - 1.2) / (1.0 - 0.0))).xxx;
			float3 temp_cast_2 = ((1.6 + (_CoverAmount - 0.0) * (0.3 - 1.6) / (1.0 - 0.0))).xxx;
			float3 temp_output_85_0 = clamp( (temp_cast_1 + (desaturateVar83 - float3( 0,0,0 )) * (temp_cast_2 - temp_cast_1) / (float3( 1,1,1 ) - float3( 0,0,0 ))) , float3( 0,0,0 ) , float3( 1,0,0 ) );
			o.Normal = normalize( lerp( BlendNormals( tex2DNode4 , UnpackScaleNormal( tex2D( _CoverNormal, i.texcoord_0 ) ,(0.0 + (_CoverNormalPower - 0.0) * (2.0 - 0.0) / (1.0 - 0.0)) ) ) , BlendNormals( tex2DNode4 , UnpackScaleNormal( tex2D( _MicrodetailNormal, i.texcoord_1 ) ,(0.0 + (_MicrodetailNormalPower - 0.0) * (2.0 - 0.0) / (1.0 - 0.0)) ) ) , temp_output_85_0.x ) );
			float4 tex2DNode38 = tex2D( _Microdetail, i.texcoord_1 );
			float2 uv_RockAlbedo = i.uv_texcoord * _RockAlbedo_ST.xy + _RockAlbedo_ST.zw;
			float4 temp_cast_4 = ((-0.1 + (_Albedo05default - 0.0) * (0.1 - -0.1) / (1.0 - 0.0))).xxxx;
			float4 temp_cast_5 = ((0.2 + (_Albedo05default - 0.0) * (1.8 - 0.2) / (1.0 - 0.0))).xxxx;
			float4 tex2DNode9 = tex2D( _CoverAlbedo, i.texcoord_0 );
			float3 temp_cast_6 = ((1.2 + (_CoverAmount - 0.0) * (-4.0 - 1.2) / (1.0 - 0.0))).xxx;
			float3 temp_cast_7 = ((1.6 + (_CoverAmount - 0.0) * (0.3 - 1.6) / (1.0 - 0.0))).xxx;
			o.Albedo = lerp( clamp( ( (float4( 0.3,0.3,0.3,0.3 ) + (lerp( float4(1,1,1,1) , tex2DNode38 , _MicrodetailAlbedo ) - float4( 0,0,0,0 )) * (float4( 1.3,1.3,1.3,1.3 ) - float4( 0.3,0.3,0.3,0.3 )) / (float4( 1,1,1,1 ) - float4( 0,0,0,0 ))) * ( clamp( (float4( -1,-1,-1,-1 ) + (lerp( float4(0.5,0.5,0.5,0.5) , tex2DNode38 , _MicrodetailAlbedo ) - float4( 0,0,0,0 )) * (float4( 0.9,0.9,0.9,0.9 ) - float4( -1,-1,-1,-1 )) / (float4( 1,1,1,1 ) - float4( 0,0,0,0 ))) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) ) + clamp( (temp_cast_4 + (tex2D( _RockAlbedo, uv_RockAlbedo ) - float4( 0,0,0,0 )) * (temp_cast_5 - temp_cast_4) / (float4( 1,1,1,1 ) - float4( 0,0,0,0 ))) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) ) , tex2DNode9 , clamp( ( tex2DNode9.a * ( 1.0 - temp_output_85_0 ) ) , float3( 0.0,0,0 ) , float3( 1.0,0,0 ) ).x ).xyz;
			float2 uv_RockMetallicGloss = i.uv_texcoord * _RockMetallicGloss_ST.xy + _RockMetallicGloss_ST.zw;
			float4 tex2DNode2 = tex2D( _RockMetallicGloss, uv_RockMetallicGloss );
			float3 desaturateVar82 = lerp( tex2DNode2.xyz,dot(tex2DNode2.xyz,float3(0.299,0.587,0.114)),0.0);
			o.Metallic = desaturateVar82.x;
			o.Smoothness = tex2DNode2.a;
			float3 temp_cast_12 = ((1.2 + (_AOPower - 0.0) * (-1.2 - 1.2) / (1.0 - 0.0))).xxx;
			o.Occlusion = clamp( (temp_cast_12 + (desaturateVar83 - float3( 0.0,0,0 )) * (float3( 1.0,0,0 ) - temp_cast_12) / (float3( 1.0,0,0 ) - float3( 0.0,0,0 ))) , float3( 0.0,0,0 ) , float3( 1.0,0,0 ) ).x;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=10001
13;195;1202;838;1810.598;935.0605;2.8;True;True
Node;AmplifyShaderEditor.RangedFloatNode;34;-2488.071,795.2385;Float;False;Property;_MicrodetailTiling;Microdetail Tiling;10;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;35;-2208.765,752.5388;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;4;FLOAT;20.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2035.261,731.1942;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;48;-396.2135,-598.4387;Float;False;Property;_MicrodetailAlbedo;Microdetail Albedo;9;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;96;-1659.631,-856.845;Float;False;Property;_Albedo05default;Albedo (0,5 default);0;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;84;-3347.812,689.3151;Float;False;Property;_CoverAmount;Cover Amount;13;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.Vector4Node;52;-346.6617,-1118.489;Float;False;Constant;_Vector2;Vector 2;12;0;0.5,0.5,0.5,0.5;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;77;-3709.874,292.8603;Float;True;Property;_Ambient;Ambient;5;0;Assets/Rock and Boulders3/origin/Textures/RockGr_04_ao.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;38;-407.492,-956.4921;Float;True;Property;_Microdetail;Microdetail;7;0;Assets/Rock and Boulders3/origin/Details/Detail_01.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;86;-3004.442,503.3911;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.2;False;4;FLOAT;-4.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;25;-2407.956,-679.1372;Float;False;Property;_CoverTiling;Cover Tiling;15;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.DesaturateOpNode;83;-3316.894,317.0612;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.TFHCRemap;94;-1131.025,-767.7297;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.2;False;4;FLOAT;1.8;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;90;-2968.09,761.3817;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.6;False;4;FLOAT;0.3;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;95;-1131.724,-923.55;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-0.1;False;4;FLOAT;0.1;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;47;15.5863,-918.9379;Float;False;3;0;FLOAT4;0.5,0.5,0.5,0.5;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;1;-1687.472,-1091.24;Float;True;Property;_RockAlbedo;Rock Albedo;1;0;Assets/Rock and Boulders3/origin/Textures/Rock01_a.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;62;-2414.635,1026.105;Float;False;Property;_MicrodetailNormalPower;Microdetail Normal Power;11;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;76;-2287.595,349.7366;Float;False;Property;_CoverNormalPower;Cover Normal Power;16;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;74;-2294.094,165.1289;Float;False;Property;_NormalPower;Normal Power;3;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.Vector4Node;58;-343.3133,-771.5894;Float;False;Constant;_Vector2;Vector 2;12;0;1,1,1,1;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;87;-2768.231,418.5249;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;3;FLOAT3;0.0;False;4;FLOAT3;0.6,0.6,0.6;False;1;FLOAT3
Node;AmplifyShaderEditor.TFHCRemap;26;-2006.38,-620.4779;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;97;-904.7144,-871.0054;Float;False;5;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0.1,0.1,0.1,0.1;False;4;FLOAT4;1;False;1;FLOAT4
Node;AmplifyShaderEditor.TFHCRemap;61;231.8875,-1035.489;Float;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;-1,-1,-1,-1;False;4;FLOAT4;0.9,0.9,0.9,0.9;False;1;FLOAT4
Node;AmplifyShaderEditor.TFHCRemap;75;-1969.481,336.2326;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;2.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;40;439.1173,-991.4132;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4
Node;AmplifyShaderEditor.ClampOpNode;85;-2542.723,417.3701;Float;False;3;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.TFHCRemap;63;-2113.529,985.387;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;2.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;98;-687.8998,-865.0607;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4
Node;AmplifyShaderEditor.LerpOp;57;11.48681,-723.9894;Float;False;3;0;FLOAT4;0.5,0.5,0.5,0.5;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1804.467,-599.0806;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;73;-1975.979,151.6249;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;2.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;9;-1102.314,-382.1933;Float;True;Property;_CoverAlbedo;Cover Albedo;12;0;Assets/Rock and Boulders3/Ground/Ground_10_no_alpha.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;89;-2328.448,448.5865;Float;False;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;78;-479.6361,811.6871;Float;False;Property;_AOPower;AO Power;6;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;42;586.1473,-818.9531;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;14;-1691.834,571.7426;Float;True;Property;_CoverNormal;Cover Normal;14;0;Assets/Rock and Boulders3/Ground/Ground_10_nmp.tif;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-1693.076,209.6875;Float;True;Property;_RockNormal;Rock Normal;2;0;Assets/Rock and Boulders3/origin/Textures/Rock01_n.tif;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;32;-1684.348,800.2623;Float;True;Property;_MicrodetailNormal;Microdetail Normal;8;0;Assets/Rock and Boulders3/origin/Details/Detail_01_nmp.tif;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;45;247.2928,-718.9731;Float;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0.3,0.3,0.3,0.3;False;4;FLOAT4;1.3,1.3,1.3,1.3;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-531.5165,-152.2744;Float;False;2;0;FLOAT;0.0;False;1;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.BlendNormalsNode;92;-1189.716,326.981;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;656.3862,-696.9377;Float;False;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.BlendNormalsNode;91;-1189.676,669.3218;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.TFHCRemap;79;-106.8795,708.3772;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.2;False;4;FLOAT;-1.2;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;43;697.3422,-480.2326;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4
Node;AmplifyShaderEditor.TFHCRemap;80;102.1315,538.711;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT3;1.0,0,0;False;3;FLOAT3;0.0;False;4;FLOAT3;1.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;93;-928.725,458.8518;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0;False;2;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.ClampOpNode;71;-363.4438,-150.2735;Float;False;3;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT3;1.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;2;-507.042,209.6545;Float;True;Property;_RockMetallicGloss;Rock Metallic Gloss;4;0;Assets/Rock and Boulders3/origin/Textures/Rock01_m.tif;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.NormalizeNode;37;-711.8246,460.1878;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ClampOpNode;81;325.454,539.7421;Float;False;3;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT3;1.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;10;316.3565,-192.8941;Float;False;3;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT3;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.DesaturateOpNode;82;-130.4679,256.2228;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.Vector3Node;30;-1629.472,417.1073;Float;False;Constant;_Vector0;Vector 0;8;0;0,0,1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;621.3304,72.07998;Float;False;True;2;Float;ASEMaterialInspector;0;Standard;MK4/Rocks desert;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;3;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;34;0
WireConnection;36;0;35;0
WireConnection;38;1;36;0
WireConnection;86;0;84;0
WireConnection;83;0;77;0
WireConnection;94;0;96;0
WireConnection;90;0;84;0
WireConnection;95;0;96;0
WireConnection;47;0;52;0
WireConnection;47;1;38;0
WireConnection;47;2;48;0
WireConnection;87;0;83;0
WireConnection;87;3;86;0
WireConnection;87;4;90;0
WireConnection;26;0;25;0
WireConnection;97;0;1;0
WireConnection;97;3;95;0
WireConnection;97;4;94;0
WireConnection;61;0;47;0
WireConnection;75;0;76;0
WireConnection;40;0;61;0
WireConnection;85;0;87;0
WireConnection;63;0;62;0
WireConnection;98;0;97;0
WireConnection;57;0;58;0
WireConnection;57;1;38;0
WireConnection;57;2;48;0
WireConnection;27;0;26;0
WireConnection;73;0;74;0
WireConnection;9;1;27;0
WireConnection;89;0;85;0
WireConnection;42;0;40;0
WireConnection;42;1;98;0
WireConnection;14;1;27;0
WireConnection;14;5;75;0
WireConnection;4;5;73;0
WireConnection;32;1;36;0
WireConnection;32;5;63;0
WireConnection;45;0;57;0
WireConnection;72;0;9;4
WireConnection;72;1;89;0
WireConnection;92;0;4;0
WireConnection;92;1;14;0
WireConnection;44;0;45;0
WireConnection;44;1;42;0
WireConnection;91;0;4;0
WireConnection;91;1;32;0
WireConnection;79;0;78;0
WireConnection;43;0;44;0
WireConnection;80;0;83;0
WireConnection;80;3;79;0
WireConnection;93;0;92;0
WireConnection;93;1;91;0
WireConnection;93;2;85;0
WireConnection;71;0;72;0
WireConnection;37;0;93;0
WireConnection;81;0;80;0
WireConnection;10;0;43;0
WireConnection;10;1;9;0
WireConnection;10;2;71;0
WireConnection;82;0;2;0
WireConnection;0;0;10;0
WireConnection;0;1;37;0
WireConnection;0;3;82;0
WireConnection;0;4;2;4
WireConnection;0;5;81;0
ASEEND*/
//CHKSM=8A4D9661703F82E67A22C9E6E58DBC6A1FC96B46