Shader "Custom/TransparentWater"
{
    Properties
    {
        _Color ("Base River Color", Color) = (1,1,1,1)
        [Header(Reflection and Fresnel)]
        _Smoothness ("Smoothness", Range(0,1)) = 1
        _Metallic ("Metallic", Range(0,1)) = 1
        _FresnelOffset("Fresnel Intensity", Range(0,10)) = 5
        [Header(Detail 1)]
        _Dt1Color("Detail 1 Tint", Color) = (1,1,1,1)
        _Dt1Dir("Detail 1 Direction", Vector) = (1,0,0,0)
        _Dt1Tx("Detail 1", 2D) = "white" {}
        // _NormalTx
        // _HeightTx
        // _Smoothness("Smoothness", Range(0,1)) = 0.5
        
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "ForceNoShadowCasting"="True"
        }
        // LOD 200
        Blend SrcAlpha OneMinusSrcAlpha // transparency stuff
        ZWrite Off

        CGPROGRAM
        // surface shader declation
        // alpha attribute added for a transparent shader
        // add custom vertex shader to get depth
        // #pragma surface surf vertex:vert fullforwardshadows alpha
        #pragma surface surf Standard alpha
        // Use 4.0 target to use more interpolators
        #pragma target 3.0

        // variables
        uniform fixed4 _Color;
        uniform half _Smoothness;
        uniform half _Metallic;
        uniform fixed _FresnelOffset;
        uniform fixed4 _Dt1Color;
        uniform fixed4 _Dt1Dir;
        uniform sampler2D _Dt1Tx;


        struct Input
        {
            float2 uv_Dt1Tx;
            //float3 normal;
            //float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // BASE COLOR
            float4 col = _Color;
            float2 dt1Coords = IN.uv_Dt1Tx + (_Dt1Dir.xy * _Time.y); // plus some stuff
            float4 dt1 = tex2D(_Dt1Tx, dt1Coords) * _Dt1Color;
            

            // DETAILS
            col.rgb = lerp(col.rgb, dt1.rgb, dt1.a);
            col.a = max(col.a, dt1.a);

            //fixed fresnel = saturate(dot(IN.viewDir, IN.normal));

            // ASSIGNMENTS
            o.Albedo = col.rgb;
            o.Alpha = col.a;
            o.Smoothness = _Smoothness * (1-col.a);
            o.Metallic = _Metallic * (1-col.a);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
