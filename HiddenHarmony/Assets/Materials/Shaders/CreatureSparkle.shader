Shader "Custom/CreatureSparkle"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _EffectTex0 ("Effect Texture 0", 2D) = "black" {}
        _EffectTex1 ("Effect Texture 1", 2D) = "black" {}
        _Smoothness ("Smoothness", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // textues
        sampler2D _MainTex;
        sampler2D _EffectTex0;
        sampler2D _EffectTex1;
        // _ST is extra parameters for the TRANSFORM_TEX function
        float4 _EffectTex0_ST;
        float4 _EffectTex1_ST;

        // other stuff
        fixed4 _Tint;
        half _Smoothness;


        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EffectTex0;
            float2 uv_EffectTex1;
            float4 screenPos;
        };

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            // get normal texture color
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            col.rgb *= _Tint.rgb;

            // get UV parameters from the screen
            float2 textureCoordinate = i.screenPos.xy / i.screenPos.w;
            float aspect = _ScreenParams.x / _ScreenParams.y;
            textureCoordinate.x = textureCoordinate.x * aspect;

            //
            textureCoordinate.y += _CosTime * unity_DeltaTime;
            float2 textureCoordinate0 = TRANSFORM_TEX(textureCoordinate, _EffectTex0);
            // animate texture coordinates with time
            textureCoordinate.x += _SinTime * unity_DeltaTime;
            float2 textureCoordinate1 = TRANSFORM_TEX(textureCoordinate, _EffectTex1);
            

            // if the effects texture has opacity
            //  add the effect color * opacity
            fixed4 effectCol0 = tex2D(_EffectTex0, textureCoordinate0);
            fixed4 effectCol1 = tex2D(_EffectTex1, textureCoordinate1);
            fixed4 sumEffect = fixed4(0,0,0,1);


            if(effectCol0.a > 0)
              sumEffect.rgb += (effectCol0.rgb * effectCol0.a);
            if(effectCol1.a > 0)
              sumEffect.rgb += (effectCol1.rgb * effectCol1.a);
            
            // set final values
            col.rgb += sumEffect.rgb;
            o.Albedo = col.rgb;
            o.Alpha = col.a;
            o.Smoothness = _Smoothness;
            // will later try to mess with emission once i figure out 
            //      how HDR is calculated
        }
        ENDCG
    }
    FallBack "Diffuse"
}
