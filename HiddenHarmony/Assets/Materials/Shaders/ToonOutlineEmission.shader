Shader "Custom/ToonOutlineEmission"
{
    Properties
    {
        // Basics -- color, texture, shadows
        _MainTex("Texture", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _ShadowTint("Shadow Tint", Color) = (0,0,0,0)
        // Emission settings
        [Toggle] _UseEmission("Use Emission", Float) = 0
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,0)
        // Outline Settings
        [Toggle] _UseOutline("Use outlines", Float) = 0
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0,0.1)) = 0.03
    }

    Subshader
    {
        Name "TOON_WITH_EMISSION"
        Tags{"RenderType" = "Opaque"}
        CGPROGRAM
        #pragma surface surf Toon
        // Variables for toon shading
        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _ShadowTint;
        fixed4 _EmissionColor;
        float _UseEmission;
        sampler2D _RampTex;

        // input for surface function
        struct Input {
            float2 uv_MainTex;
        };

        // Surface Shading
        void surf(Input IN, inout SurfaceOutput o) {
            // calculate base color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            // get emission color
            fixed4 emission = _EmissionColor;
            // add emission to Albedo if enabled
            if(_UseEmission)
                o.Albedo += emission.rgb;

            // return object
            o.Alpha = c.a;
        }

        // Toon Lighting Function (used to calculate lighting in surf())
        fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            // calculate how much light is hitting the object
            half NdotL = max(0, dot(s.Normal, lightDir));
            // adjust lighting value based on Ramp Texture
            NdotL = tex2D(_RampTex, fixed2(NdotL, 1));
            // apply lighting
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten; // basic color and lighting
            c.rgb += _ShadowTint.rgb * max(0.0, (1.0 - (NdotL * atten)));   // apply shadow tint
            c.a = s.Alpha;

            return c;
        }
        ENDCG

        // Outline Pass
        Pass{
            Name "HULL_OUTLINE"
            // Outlines are done by expanding the original outlines
            //  (creating a "hull") then culling the front hull
            Cull Front

            CGPROGRAM
            // include useful shader functions
            #include "UnityCG.cginc"
            // define vert and frag shader
            #pragma vertex vert
            #pragma fragment frag
            
            // Variables
            fixed4 _OutlineColor;
            float _OutlineThickness;
            float _UseOutline;

            sampler2D _MainTex;

            // struct for vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;

            };

            // struct for fragment shader
            struct v2f{
                float4 position : SV_POSITION;
            };

            // Vertex Shader
            v2f vert(appdata v){
                v2f o;
                if(_UseOutline){
                    // calculate position of expanded object along normals
                    float3 normal = normalize(v.normal);
                    float3 outlineOffset = normal * _OutlineThickness;
                    float3 position = v.vertex + outlineOffset;

                    // convert vertex positions from object to clip space
                    o.position = UnityObjectToClipPos(position);
                } else {
                    // pass on regular vertex information
                    o.position = UnityObjectToClipPos(v.vertex);
                }
                return o;
            }

            // Fragment Shader
            fixed4 frag(v2f i) : SV_TARGET{
                return _OutlineColor;
            }

            ENDCG
        }
        
    }
    Fallback "Diffuse"
}
