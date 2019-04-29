Shader "Custom/ToonShader"{
	Properties	// can be viewed in inspector
	{
		_MainTex("Texture", 2D) = "white" {}
		_RampTex("Ramp", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_ShadowTint("Shadow Tint", Color) = (0,0,0,0)
	}

	Subshader
	{

		Tags{"RenderType" = "Opaque"}
		CGPROGRAM
		#pragma surface surf Toon
		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _ShadowTint;
		void surf(Input IN, inout SurfaceOutput o) {

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		sampler2D _RampTex;
		fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten) {
			// atten = light/shadow attentuation -- not sure if
			// < 1 means in shadow, or < 0.5 means less than
			// originally 1 means lit, and then you multiply by 2 to make things look better, but this has changed

			// normally diff (diffuse) = max(0, dot(s.Normal, lightDir)
			// get value from -1 to 1 for the dot product of surface normal and light
			// if < 0, just do 0


			half NdotL = max(0, dot(s.Normal, lightDir));

			// adjust lighting amount to be from the texture
			// text2d(s,t): s = texture to be sampled, t = (u,v), returns float 4 rgba
			// what 
			NdotL = tex2D(_RampTex, fixed2(NdotL, 1));

			fixed4 c;
			// OKAY so atten is the shadow value, which, interestingly, applies to itelf
			// even if diffuse also calculates shadow
			// for loop, array of possible values, check if it's between this or next
			// or next and next + 1 --> apply function for choosing
			// atten is going to be it's own goddamn problem that we're gonna deal with later

			// Tinting the Shadow
			

			c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;

			c.rgb += _ShadowTint.rgb * max(0.0, (1.0 - (NdotL * atten)));

			c.a = s.Alpha;

			return c;
		}
		ENDCG
	}
	Fallback "Diffuse"
}