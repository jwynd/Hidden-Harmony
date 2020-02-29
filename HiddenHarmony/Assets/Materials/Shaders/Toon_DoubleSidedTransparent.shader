Shader "Custom/Toon_DoubleSidedTransparent"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,0)
        _ShadowTint("Shadow Tint", Color) = (0,0,0,0)
    }
    SubShader
    {
        // transparency
        // * trying the transparent cutout queue with alpha-to-coverage command
        // * uses techniques like transparent cutout but with some blending to help
        // * reduce aliasing
        // * might need to adjust Quality settings for this
        // Tags{"RenderType" = "TransparentCutout" "Queue"="AlphaTest" "IgnoreProjector"="True"}
        // AlphaToMask On

        Tags{"RenderType"="TransparentCutout" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha // used for standard transparency
        //Blend SrcAlpha One
        // ZWrite Off // used for standard transparency
        // basic double-sided --> should really probably use two passes
        Cull off

        CGPROGRAM
        // FUCK DUDE LITERALLY JUST PUTTING THE GODDAMN ALPHA THERE FIXED THINGS HUH.
        #pragma surface surf Toon alpha
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

        // toon lighting
        sampler2D _RampTex;
        fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            // toon lighting via texture ramp
            half NdotL = max(0, dot(s.Normal, lightDir));
            NdotL = tex2D(_RampTex, fixed2(NdotL, 1));

            // shadows
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;
            c.rgb += _ShadowTint.rgb * max(0.0, (1.0 - (NdotL * atten)));
            c.a = s.Alpha;

            return c;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
