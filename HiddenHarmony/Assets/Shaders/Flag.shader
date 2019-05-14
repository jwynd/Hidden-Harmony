// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Flag" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Speed("Speed", Range(0, 5.0)) = 1
		_Frequency("Frequency", Range(0, 1.3)) = 1
		_Amplitude("Amplitude", Range(0, 5.0)) = 1

		_Dist("Distance", Float) = 100.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull off

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _Speed;
	float _Frequency;
	float _Amplitude;

	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
	};


	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		half2 uv2 : TEXCOORD1;
		UNITY_FOG_COORDS(2)
	};


	v2f vert(appdata_base v)
	{
		v2f o;
		float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
		v.vertex.y += cos((v.vertex.x + _Time.y * _Speed) * _Frequency) * _Amplitude * (v.vertex.x - 5);
		o.pos = UnityObjectToClipPos(v.vertex);

		v.vertex = mul(UNITY_MATRIX_P, vPos);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.uv2 = v.texcoord;

		UNITY_TRANSFER_FOG(o, v.vertex);

		return o;
	}


		half4 frag(v2f i) : COLOR
	{
		half4 col = tex2D(_MainTex, i.uv.xy);

		UNITY_APPLY_FOG(i.fogCoord, col);
		UNITY_OPAQUE_ALPHA(col.a);

		return col;
	}


		ENDCG

	}
	}
		FallBack "Diffuse"
}