Shader "Custom/CreatureSparkle_MeshPos"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0,1)) = 0.2
        _Overlay ("Overlay Blend Mode", float) = 1
        // Effect 0: circular movement
        _EffectSpeed0 ("Effect 0 - Circle - Speed", float) = 1
        _EffectTint0 ("Effect 0 - Tint", Color) = (1,1,1,1)
        _EffectTex0 ("Effect 0 - Texture", 2D) = "black" {}
        // Effect 1: diagonal movement
        _EffectSpeed1 ("Effect 1 - Diagonal - Speed", float) = 1
        _EffectTint1 ("Effect 1 - Tint", Color) = (1,1,1,1)
        _EffectTex1 ("Effect 1 - Texture", 2D) = "black" {}
        // Effect 2: horizontal movement
        _EffectSpeed2 ("Effect 2 - Horizontal - Speed", float) = 1
        _EffectTint2 ("Effect 2 - Tint", Color) = (1,1,1,1)
        _EffectTex2 ("Effect 2 - Texture", 2D) = "black" {}
        // Effect 3: vertical movement
        _EffectSpeed3 ("Effect 3 - Vertical - Speed", float) = 1
        _EffectTint3 ("Effect 3 - Tint", Color) = (1,1,1,1)
        _EffectTex3 ("Effect 3 - Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // textures
        uniform sampler2D _MainTex;
        uniform sampler2D _EffectTex0;
        uniform sampler2D _EffectTex1;
        uniform sampler2D _EffectTex2;
        uniform sampler2D _EffectTex3;
        // _ST is extra parameters for the TRANSFORM_TEX function
        uniform float4 _EffectTex0_ST;
        uniform float4 _EffectTex1_ST;
        uniform float4 _EffectTex2_ST;
        uniform float4 _EffectTex3_ST;
        // speed parameters
        uniform float _EffectSpeed0;
        uniform float _EffectSpeed1;
        uniform float _EffectSpeed2;
        uniform float _EffectSpeed3;
        // color parameters
        uniform fixed4 _Tint;
        uniform fixed4 _EffectTint0;
        uniform fixed4 _EffectTint1;
        uniform fixed4 _EffectTint2;
        uniform fixed4 _EffectTint3;
        // etc.
        uniform half _Smoothness;
        uniform float4 _Overlay;


        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EffectTex0;
            float2 uv_EffectTex1;
            float2 uv_EffectTex2;
            float2 uv_EffectTex3;
            float4 screenPos;
        };

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            // get base texture color for the model
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            col.rgb *= _Tint.rgb;

            // reuse mesh UV parameters
            float2 txtCoordBase = i.uv_MainTex;

            // animate over time
            float2 textureCoordinate0 = TRANSFORM_TEX(txtCoordBase, _EffectTex0);
            float2 textureCoordinate1 = TRANSFORM_TEX(txtCoordBase, _EffectTex1);
            float2 textureCoordinate2 = TRANSFORM_TEX(txtCoordBase, _EffectTex2);
            float2 textureCoordinate3 = TRANSFORM_TEX(txtCoordBase, _EffectTex3);
            
            // ANIMATE TEXTURES OVER TIME
            // Circular Movement
            textureCoordinate0.x += cos(_Time.y) * _EffectSpeed0;
            textureCoordinate0.y += sin(_Time.y) * _EffectSpeed0;
            // Diagonal Movement
            textureCoordinate1.x += _Time.x * _EffectSpeed1;
            textureCoordinate1.y += _Time.x * _EffectSpeed1;
            // Horizontal Movement
            textureCoordinate2.x += _Time.x * _EffectSpeed2;
            // Vertical Movement
            textureCoordinate3.y += _Time.x * _EffectSpeed3;

            // EFFECT COLORATION
            // Get base color from texture
            fixed4 sumEffect = fixed4(0,0,0,1);
            fixed4 effectCol0 = tex2D(_EffectTex0, textureCoordinate0);
            fixed4 effectCol1 = tex2D(_EffectTex1, textureCoordinate1);
            fixed4 effectCol2 = tex2D(_EffectTex2, textureCoordinate2);
            fixed4 effectCol3 = tex2D(_EffectTex3, textureCoordinate3);

            // Adjust Effect color by tint
            //  (Note: turning down the alpha in the tint can be used to 
            //  dilute the effect.)
            effectCol0 *= _EffectTint0;
            effectCol1 *= _EffectTint1;
            effectCol2 *= _EffectTint2;
            effectCol3 *= _EffectTint3;

            // Multiply the effect's color by its alpha so as to dilute
            //      the color appropirately without affecting the total
            //      alpha of the material
                sumEffect.rgb += effectCol0.rgb * effectCol0.a;
                sumEffect.rgb += effectCol1.rgb * effectCol1.a;
                sumEffect.rgb += effectCol2.rgb * effectCol2.a;
                sumEffect.rgb += effectCol3.rgb * effectCol3.a;
            
            // SET FINAL VALUES FOR THE MATERIAL
            col.rgb += sumEffect.rgb;
            o.Albedo = col.rgb;
            o.Alpha = col.a;
            o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
