Shader "Custom/TransparentMovingEffect"
{
    Properties
    {
        _Color ("Base Tint", Color) = (1,1,1,1)
        _MainTex ("Base Texture", 2D) = "white" {}
        [Header(Detail 1)]
        _Dt1Color("Detail 1 Tint", Color) = (1,1,1,1)
        _Dt1Dir("Detail 1 Direction", Vector) = (1,0,0,0)
        _Dt1Tx("Detail 1", 2D) = "white" {}
        
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
        //Cull Off // flip normals
        ZWrite Off

        CGPROGRAM
        // surface shader declation
        // alpha attribute added for a transparent shader
        // add custom vertex shader to get depth
        // #pragma surface surf vertex:vert fullforwardshadows alpha
        // #pragma surface surf Custom_Unlit alpha
        #pragma surface surf Standard alpha
        // Use 4.0 target to use more interpolators
        #pragma target 3.0

        // variables
        uniform fixed4 _Color;
        uniform sampler2D _MainTex;
        uniform fixed4 _Dt1Color;
        uniform fixed4 _Dt1Dir;
        uniform sampler2D _Dt1Tx;


        struct Input
        {
            float2 uv_Dt1Tx;
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) // Standard
        //void surf(Input IN, inout SurfaceOutput o) // Custom_Unlit
        {
            // BASE COLOR
            float4 col = tex2D(_MainTex, IN.uv_MainTex) * _Color;


            float2 dt1Coords = IN.uv_Dt1Tx + (_Dt1Dir.xy * _Time.y); // plus some stuff
            float4 dt1 = tex2D(_Dt1Tx, dt1Coords) * _Dt1Color;
            

            // DETAILS
            col.rgb = lerp(col.rgb, dt1.rgb, dt1.a);
            col.a = max(col.a, dt1.a);

            // ASSIGNMENTS
            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }

        fixed4 LightingCustom_Unlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {

            // half NdotL = max(0, dot(s.Normal, lightDir));
            half NdotL = abs(dot(s.Normal, lightDir));
            //half NdotL = max(0, dot(lightDir, s.Normal)); // reverse lighting direction?
            //NdotL = tex2D(_RampTex, fixed2(NdotL, 1));

            // shadows
            fixed4 c;
            c.rgb = s.Albedo * NdotL; // * atten;
            //c.rgb += _ShadowTint.rgb * max(0.0, (1.0 - (NdotL * atten)));
            c.a = s.Alpha;

            return c;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
