Shader "Custom/Seamless Portal" {
	Properties{
		 _MainTex("Render Texture", 2D) = "white" {}
		_MaskTex("Portal Mask", 2D) = "white" {}
		_BGTex("Masked Background Texture", 2D) = "white" {}
		_TexLight("Enable Mask Background Lighting", Int) = 0
	}

	SubShader{
		LOD 250
		Pass{
			Tags {"LightMode"="ForwardBase"}
			CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				#include "unitycg.cginc"
				#include "UnityLightingCommon.cginc"

				sampler2D _MainTex;

				sampler2D _MaskTex;
				float4 _MaskTex_ST;

				sampler2D _BGTex;
				float4 _BGTex_ST;

				int _TexLight;

				struct VertInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uvM : TEXCOORD0;
					float2 uvBG : TEXCOORD1;
				};

				struct VertOutput {
					float4 vertex : SV_POSITION;
					fixed4 diff : COLOR0;
					float2 uvM : TEXCOORD0;
					float2 uvBG : TEXCOORD1;
					Vector screenPos : TEXCOORD2;
				};

				VertOutput vert(VertInput i) {
					VertOutput o;
					o.vertex = UnityObjectToClipPos(i.vertex);
					o.uvM = TRANSFORM_TEX(i.uvM,_MaskTex);
					o.uvBG = TRANSFORM_TEX(i.uvBG,_BGTex);
					o.screenPos = ComputeScreenPos(o.vertex);

					half3 worldNormal = UnityObjectToWorldNormal(i.normal.xyz);
					half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
					o.diff = nl * _LightColor0;
					o.diff.rgb += ShadeSH9(half4(worldNormal,1));

					return o;
				}

				half4 frag(VertOutput i) : SV_Target{
					i.screenPos /= i.screenPos.w;

					float4 tex = tex2D(_MaskTex, i.uvM) * tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));
					
					if(_TexLight == 1)
						tex += (1-tex2D(_MaskTex, i.uvM)) * tex2D(_BGTex, i.uvBG) * i.diff;
					else
						tex += (1-tex2D(_MaskTex, i.uvM)) * tex2D(_BGTex, i.uvBG);
				
					return tex;
				}

			ENDCG
		}
	}

}
