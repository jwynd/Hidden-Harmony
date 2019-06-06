Shader "Custom/TransparentWater"
{
    Properties
    {
        _Color ("Base River Color", Color) = (1,1,1,1)

        [Header(Spec Layer 1)]
        _Specs1 ("Specs", 2D) = "black" {}
        _SpecColor1("Spec Color", Color) = (1,1,1,1)
        _SpecDirection1("Spec Direction (x, y, cos, sin)", Vector) = (0,0.5,.1,0)

        [Header(Spec Layer 2)]
        _Specs2 ("Specs", 2D) = "black" {}
        _SpecColor2("Spec Color", Color) = (1,1,1,1)
        _SpecDirection2("Spec Direction (x, y, cos, sin)", Vector) = (0,0.5,.1,0)

        [Header(Foam)]
        _FoamNoise("Foam Noise", 2D) = "white" {}
        _FoamDirection("Foam Direction", Vector) = (0,1,0,0)
        _FoamColor("Foam Color", Color) = (1,1,1,1)
        _FoamAmount("Foam Amount", Range(0,2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "ForceNoShadowCasting"="True"}
        LOD 200

        CGPROGRAM
        // surface shader declation
        // alpha attribute added for a transparent shader
        // add custom vertex shader to get depth
        #pragma surface surf Standard vertex:vert fullforwardshadows alpha
        // Use 4.0 target to use more interpolators
        #pragma target 4.0

        // variables
        // Base
        uniform fixed4 _Color;
        // Spec layer 1
        uniform sampler2D _Specs1;
        uniform fixed4 _SpecColor1;
        float4 _SpecDirection1;
        // Spec layer 2
        uniform sampler2D _Specs2;
        uniform fixed4 _SpecColor2;
        float4 _SpecDirection2;
        // Foam
        uniform sampler2D _FoamNoise;
        uniform fixed4 _FoamColor;
        uniform float _FoamAmount;
        uniform float2 _FoamDirection;
        sampler2D_float _CameraDepthTexture;


        struct Input
        {
            float2 uv_Specs1;
            float2 uv_Specs2;
            float2 uv_FoamNoise;
            float eyeDepth;
            float4 screenPos;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            COMPUTE_EYEDEPTH(o.eyeDepth);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Base water
            float4 col = _Color;

            // Spec Layers
            // calculate spec layer 1
            float2 specCoords1 = IN.uv_Specs1 + _SpecDirection1.xy * _Time.y;
            specCoords1.x += cos(_Time.y) * _SpecDirection1.w;
            specCoords1.y += sin(_Time.y) * _SpecDirection1.z;
            fixed4 specLayer1 = tex2D(_Specs1, specCoords1) * _SpecColor1;
            col.rgb = lerp(col.rgb, specLayer1.rgb, specLayer1.a);
            col.a = lerp(col.a, 1, specLayer1.a);

            // calculate spec layer 1
            float2 specCoords2 = IN.uv_Specs2 + _SpecDirection2.xy * _Time.y;
            specCoords2.x += cos(_Time.y) * _SpecDirection2.w;
            specCoords2.y += sin(_Time.y) * _SpecDirection2.z;
            fixed4 specLayer2 = tex2D(_Specs2, specCoords2) * _SpecColor2;
            col.rgb = lerp(col.rgb, specLayer2.rgb, specLayer2.a);
            col.a = lerp(col.a, 1, specLayer2.a);

            // Foam
            // get some depth values
            // all hail Ronja
            float4 projCoords = UNITY_PROJ_COORD(IN.screenPos);
            float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, projCoords);
            float sceneZ = LinearEyeDepth(rawZ);
            float surfaceZ = IN.eyeDepth;
            
            // foam perlin noise to add some variation
            float2 foamCoords = IN.uv_FoamNoise + _FoamDirection * _Time.y;
            float foamNoise = tex2D(_FoamNoise, foamCoords).r;
            // compare depth values to create foam around objs
            float foam = 1-((sceneZ - surfaceZ) / _FoamAmount);
            // use saturate to clamp between 0 and 1
            foam = saturate(foam - foamNoise);
            col.rgb = lerp(col.rgb, _FoamColor.rgb, foam);
            col.a = lerp(col.a, 1, foam * _FoamColor.a);

            // assign final color and alpha
            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
