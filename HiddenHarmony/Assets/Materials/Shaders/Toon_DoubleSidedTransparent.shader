Shader "Custom/Toon_DoubleSidedTransparent"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,0)
        _ShadowTint("Shadow Tint", Color) = (0,0,0,0)
        _Cutoff("Cutoff", Float) = 0.5
    }
    SubShader
    {
        // put in the transparent queue or he die
        Tags{"RenderType"="TransparentCutout" "Queue"="Transparent"}

        // Render Pixels with at least CUTOFF's opacity, set in editor
        AlphaTest Greater [_Cutoff]
        // Render both front and back faces
        // Note: the undersides are not properly lit but like...we're not reversing
        // the leaves so.  it's fine.
        Cull Off

        CGPROGRAM
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
