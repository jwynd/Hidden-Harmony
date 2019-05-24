Shader "Custom/CreatureStone"
{
    Properties
    {
        
        // standard toon shading properties 
        _MainTex("Texture", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        [HDR] _EmissionColor ("Emission Color for Cracks", Color) = (0,0,0,0)

        // Crack stage controller -- range 0 - 3
        _CrackStage("Crack Stage", Int) = 0
        // stage 0 = normal stone
        // stage 1 = cracks
        _CrackS1Tn1("Crack Stage 1 - Tint 1", Color) = (1,1,1,1)
        _CrackS1Tx1("Crack Stage 1 - Texture 1", 2D) = "black" {}
        _CrackS1Tn2("Crack Stage 1 - Tint 2", Color) = (1,1,1,1)
        _CrackS1Tx2("Crack Stage 1 - Texture 2", 2D) = "black" {}
        // stage 2 = wider cracks with light
        _CrackS2Tn1("Crack Stage 2 - Tint 1", Color) = (1,1,1,1)
        _CrackS2Tx1("Crack Stage 2 - Texture 1", 2D) = "black" {}
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
        uniform float _CrackStage;
        // Stage 1
        uniform sampler2D _CrackS1Tx1;
        uniform fixed4 _CrackS1Tn1;
        uniform sampler2D _CrackS1Tx2;
        uniform fixed4 _CrackS1Tn2;
        // Stage 2
        uniform sampler2D _CrackS2Tx1;
        uniform fixed4 _CrackS2Tn1;

        // input for surface function
        struct Input {
            float2 uv_MainTex;
            float2 uv_CrackS1Tx1;
            float2 uv_CrackS1Tx2;
            float2 uv_CrackS2Tx1;
        };

        // Surface Shading
        void surf(Input IN, inout SurfaceOutput o) {
            // calculate base color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // return object
            o.Alpha = c.a;

            if(_CrackStage <= 0)
                return;

            // calculate texture colors
            fixed4 s1col1 = tex2D(_CrackS1Tx1, IN.uv_CrackS1Tx1) * _CrackS1Tn1;
            fixed4 s1col2 = tex2D(_CrackS1Tx2, IN.uv_CrackS1Tx2) * _CrackS1Tn2;

            // get emission color
            fixed4 emission = _EmissionColor;
            
            // if first stage, show cracks
            // if second sage, add cracks, make first cracks glow
            if(_CrackStage == 1){
                // if not transparent, subtract to darken Albedo
                if(s1col1.a > 0)
                    o.Albedo -= (s1col1.rgb * s1col1.a);
                if(s1col2.a > 0)
                    o.Albedo -= (s1col2.rgb * s1col2.a);

                // add some slight glowing wherever the Albedo is black
                if((o.Albedo.r + o.Albedo.g + o.Albedo.b) <= 0)
                    o.Albedo += emission.rgb;
                return;
            }

            if(_CrackStage == 2) {
                // get color
                fixed4 s2col1 = tex2D(_CrackS2Tx1, IN.uv_CrackS2Tx1) * _CrackS2Tn1;
                // if not transparent, subtract to darken Albedo
                if(s2col1.a > 0)
                    o.Albedo -= (s2col1.rgb * s2col1.a);

                // make first cracks emit light completely
                if(s1col1.a > 0 || s1col2.a > 0)
                    o.Albedo += emission.rgb;
                return;
            }

            
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
