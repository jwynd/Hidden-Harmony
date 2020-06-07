Shader "Custom/SurfaceWater"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // Effect Layer 0
        _EffectSpeed0 ("Effect 0 - Speed", Vector) = (1,1,1,1)
        _EffectTint0 ("Effect 0 - Tint", Color) = (1,1,1,1)
        _EffectTex0 ("Effect 0 - Texture", 2D) = "white" {}

        // Perlin Layer
        _PerlinSpeed ("Perlin Mask Speed", Vector) = (0,0,0,0)
        _Perlin ("Perlin Texture", 2D) = "black" {}
        _PerlinMod("Perlin Intensity Modifier", Float) = 1

        _Smoothness("Smoothness", Float) = 0.2
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200 // ???

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // textures
        sampler2D _MainTex;
        sampler2D _EffectTex0;
        sampler2D _Perlin;

        // speeds
        fixed2 _EffectSpeed0;
        fixed2 _PerlinSpeed;

        // colors and tints
        fixed4 _Color;
        fixed4 _EffectTint0;
        fixed _Smoothness;
        fixed _PerlinMod;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EffectTex0;
            float2 uv_Perlin;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Base water texture
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            // Perlin Masking Texture
            fixed4 perlinMask = tex2D(_Perlin, IN.uv_Perlin + (_PerlinSpeed.xy * _Time.y));

            // Effect 0 layer
            fixed2 e0_uvAdjust = fixed2( IN.uv_EffectTex0 + (_EffectSpeed0.xy * _Time.y));
            // Moving streams with perlin noise
            fixed4 effect0 = tex2D(_EffectTex0, e0_uvAdjust) * _EffectTint0;
            c.rgb += effect0.rgb * effect0.a * (perlinMask.r * perlinMask.a * _PerlinMod);
            
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
