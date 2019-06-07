Shader "Custom/OrcastraShader"
{
    Properties
    {
        [Header(Basic Properties)]
        _MainTex("Main Texture", 2D) = "white" {}
        _Color ("Tint to main texture", Color) = (1,1,1,1)
        _MainTexDirection("MainTex Movement (x,y,cos,sin)", Vector) = (0, .5, 0, 0)

        [Header(Emission Layer)]
        _EmissionTex("Emission Texture", 2D) = "black" {}
        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _EmissionIntensity("Emission Intensity", float) = 1
        _EmissionDirection("Emission Movement (x,y,cos,sin)", Vector) = (0, .5, 0, 0)
        _Perlin("Emission Noise", 2D) = "white" {}
        _PerlinDirection("Perlin Movement (x,y,cos,sin)", Vector) = (0, .5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        uniform sampler2D _MainTex;
        uniform fixed4 _Color;
        uniform fixed4 _MainTexDirection;
        uniform sampler2D _EmissionTex;
        uniform fixed4 _EmissionColor;
        uniform fixed _EmissionIntensity;
        uniform fixed4 _EmissionDirection;
        uniform sampler2D _Perlin;
        uniform fixed4 _PerlinDirection;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionTex;
            float2 uv_Perlin;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed2 mainUV = IN.uv_MainTex + (_MainTexDirection.xy * _Time.y);
            fixed4 col = tex2D(_MainTex, mainUV) * _Color;


            fixed2 emissionUV = IN.uv_EmissionTex + (_EmissionDirection.xy * _Time.y);
            emissionUV.x += cos(_Time.y) * _EmissionDirection.z;
            emissionUV.y += sin(_Time.y) * _EmissionDirection.w;
            fixed4 emissionTX = tex2D(_EmissionTex, emissionUV);
            // get the maximum emission intensity for this pixel (multiplied by alpha)
            fixed emissionI = emissionTX.a * _EmissionIntensity;

            // sample perlin noise to mask emission
            fixed2 perlinUV = IN.uv_Perlin + (_PerlinDirection.xy * _Time.y);
            perlinUV.x += cos(_Time.y) * _PerlinDirection.z;
            perlinUV.y += sin(_Time.y) * _PerlinDirection.w;
            fixed perlinMask = tex2D(_Perlin, perlinUV).r;

            // multiple emission for this pixel by the mask
            emissionI *= perlinMask;
            col.rgb += _EmissionColor.rgb * emissionI;

            o.Albedo = col.rgb;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
