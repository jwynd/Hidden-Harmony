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
        // stage 0 = normal stone
        // stage 1 = cracks
        // stage 2 = wider cracks with light
        // stage 3 = all light
        _CrackStage("Crack Stage", Int) = 0

        // the most solid texture -- will create cracks
        _CrkOverlayTint("Crack Overlay Tint", Color) = (1,1,1,1)
        _CrkOverlayTexture("Crack Overlay Texture", 2D) = "black" {}
        // the more basic texture -- will light up in stage 2
        _CrkBaseTint("Crack Base Tint", Color) = (1,1,1,1)
        _CrkBaseTexture("Crack Base Texture", 2D) = "black" {}
        
        
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
        uniform sampler2D _CrkOverlayTexture;
        uniform fixed4 _CrkOverlayTint;
        uniform sampler2D _CrkBaseTexture;
        uniform fixed4 _CrkBaseTint;
        // Stage 2
        uniform sampler2D _CrackS2Tx1;
        uniform fixed4 _CrackS2Tn1;

        // input for surface function
        struct Input {
            float2 uv_MainTex;
            float2 uv_CrkOverlayTexture;
            float2 uv_CrkBaseTexture;
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
            fixed4 colOverlay = tex2D(_CrkOverlayTexture, IN.uv_CrkOverlayTexture) * _CrkOverlayTint;
            fixed4 colBase = tex2D(_CrkBaseTexture, IN.uv_CrkBaseTexture) * _CrkBaseTint;
            // get emission color
            fixed4 emission = _EmissionColor;
            
            // if first stage, show cracks
            // if second sage, add cracks, make first cracks glow
            if(_CrackStage == 1){
                // if not transparent, subtract to darken Albedo
                if(colOverlay.a > 0)
                    o.Albedo -= (colOverlay.rgb * colOverlay.a);
                if(colBase.a > 0)
                    o.Albedo -= (colBase.rgb * colBase.a);

                // add some slight glowing wherever the Albedo is black
                if((o.Albedo.r + o.Albedo.g + o.Albedo.b) <= 0)
                    o.Albedo += emission.rgb;
                return;
            }

            if(_CrackStage == 2) {
                // get combined color
                fixed combinedAlpha = colOverlay.a + colBase.a;
                // if not transparent, emit light
                if(combinedAlpha > 0)
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
