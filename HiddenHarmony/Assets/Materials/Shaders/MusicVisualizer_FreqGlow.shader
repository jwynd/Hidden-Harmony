Shader "Custom/MusicVisualizer_FreqGlow"
{
    Properties
    {
        // color provided by material -- alpha used as multiplier for emission
        _Color ("Color", Color) = (1,1,1,1)
        _EmissionMax("Maximum Emission Amount", Float) = 2
        _RampTex("Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model
        #pragma surface surf Standard

        uniform fixed4 _Color;
        uniform fixed _EmissionMax;
        uniform sampler2D _RampTex;

        struct Input
        {
            // unity yells at me if I dont leave this here
            float2 uv_RampTex;
        };

        // ???????
        // does this make it faster
        // ???????
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // get normal Albedo
            o.Albedo = _Color.rgb;
            // calculate emission intensity
            // sample from (intensity of spectrumData, center)
            fixed4 ramp = tex2D(_RampTex, float2(_Color.a, 0.5)).r;
            ramp *= _EmissionMax;
            fixed3 emission = _Color.rgb * ramp; // might wanna try 4
            o.Albedo += emission;

            // put all the other shit in
            o.Smoothness = 0.1;
            o.Metallic = 0.0;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
