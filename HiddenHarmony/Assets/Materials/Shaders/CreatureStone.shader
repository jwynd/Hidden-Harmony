Shader "Custom/CreatureStone"
{
    Properties
    {
        // standard toon shading properties 
        _MainTex("Texture", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        [HDR] _EmissionColor ("Emission Color for Cracks", Color) = (0,0,0,0)

        // stage 0 = normal stone
        // stage 1 = cracks
        [Toggle] _CrackStage1("Stage 1 Cracks Active", Float) = 0
        _CrackTint1("Stage 1 Cracks Tint", Color) = (1,1,1,1)
        _CrackTex1("Stage 1 Crack Texture", 2D) = "black" {}
        // stage 2 = wider cracks with light
        [Toggle] _CrackStage2("Stage 2 Cracks Active", Float) = 0
        _CrackTint2("Stage 2 Cracks Tint", Color) = (1,1,1,1)
        _CrackTex2("Stage 2 Crack Texture", 2D) = "black" {}
        // stage 3 = all light
        
    }
    SubShader
    {
        Name "CREATURE_STONE"
        Tags{"RenderType" = "Opaque"}
        CGPROGRAM
        #pragma surface surf Toon
        // Properties for toon shading
        uniform sampler2D _MainTex;
        uniform fixed4 _Color;
        uniform fixed4 _EmissionColor;
        uniform sampler2D _RampTex;
        // Properties for Cracks
        // Stage 1
        uniform float _CrackStage1;
        uniform sampler2D _CrackTex1;
        uniform fixed4 _CrackTint1;
        // Stage 2
        uniform float _CrackStage2;
        uniform sampler2D _CrackTex2;
        uniform fixed4 _CrackTint2;

        // input for surface function
        struct Input {
            float2 uv_MainTex;
            float2 uv_CrackTex1;
            float2 uv_CrackTex2;
        };

        // Surface Shading
        void surf(Input IN, inout SurfaceOutput o) {
            // calculate base color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            // get emission color
            fixed4 emission = _EmissionColor;
            fixed4 crackCol1 = tex2D(_CrackTex1, IN.uv_CrackTex1) * _CrackTint1;
            fixed4 crackCol2 = tex2D(_CrackTex2, IN.uv_CrackTex2) * _CrackTint2;
            
            // if first stage, show cracks
            // if second sage, add cracks, make first cracks glow
            if(_CrackStage1){
                o.Albedo *= (crackCol1.rgb * (1-crackCol1.a));
                if(o.Albedo.r == 0 && o.Albedo.g == 0 && o.Albedo.b == 0)
                    o.Albedo += emission.rgb;

            }

            if(_CrackStage2) {
                o.Albedo += (crackCol1.rgb * crackCol1.a);
                // o.Albedo += (crackCol2.rgb * crackCol2.a);
                // add emission to stage one cracks
                if(crackCol2.a > 0)
                    o.Albedo += emission.rgb;
            }

            // return object
            o.Alpha = c.a;
        }

        // LIGHTING PARADIGM
        // Toon Lighting Function (used to calculate lighting in surf())
        fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            // calculate how much light is hitting the object
            half NdotL = max(0, dot(s.Normal, lightDir));
            // adjust lighting value based on Ramp Texture
            NdotL = tex2D(_RampTex, fixed2(NdotL, 1));
            // apply lighting
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten; // basic color and lighting
            c.a = s.Alpha;
            return c;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
