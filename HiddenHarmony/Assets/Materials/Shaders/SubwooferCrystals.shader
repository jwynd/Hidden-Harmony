Shader "Custom/SubwooferCrystals"
{
    Properties
    {
        // rainbow parameters
        _CycleOffset("Cycle Offset", float) = 0
        _CycleSpeed("Cycle Speed", float) = 1
        _EmissionIntensity("Emission Intensity -- Hardcoded (Power of Two)", float) = 1
        _PulseTex("Pulse Shape", 2D) = "black" {}
        _PulseSpeed("Pulse Speed (BPM)", float) = 1
        // lighting parameters
        _RampTex("Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Name "SUBWOOFER_CRYSTALS"
        Tags{"RenderType" = "Opaque"}
        CGPROGRAM
        #pragma surface surf Toon
        // Properties for toon shading
        //uniform sampler2D _MainTex;
        uniform fixed _CycleOffset;
        uniform fixed _CycleSpeed;
        uniform fixed _EmissionIntensity;
        uniform sampler2D _PulseTex;
        uniform fixed _PulseSpeed;

        uniform sampler2D _RampTex;

        // gonna be doin some Cosines
        static const float PI = 3.14159265;

        // input for surface function
        struct Input {
            float2 uv_MainTex;
        };

        // FUNCTIONS
        // calculateColor()
        // Returns a value from [0,1] for a color channel based on an offset value
        // To cycle through the color wheel CCW, the RGB values generally cycle
        //      in this pattern:
        // R:100 Y:110 G:010 C:011 B:001 V:101 --> R:100
        // At each point in the cycle, ONE of the RGB values must be at 1.
        // if we divide the cycle into a phase of 6 blocks, the pattern is (generally)
        //      as follows:
        // up 1 1 down 0 0
        // To loosely simulate this, we use a 3cos(Time) + rgboffset 
        fixed calculateColor(in fixed channelOffset)
        {
            // use the offset to determine which part in the cycle
            //      this RGB channel is
            fixed cValue = 3 * cos(_Time.y * _CycleSpeed + _CycleOffset + channelOffset);

            // return based on 
            if(cValue >= 1){
                return 1;
            } 
            else if(cValue <= -1){
                return 0;
            } 
            else{
                return (cValue + 1)/2; // force value between 0 and 1
            }
        }

        // Surface Shading
        void surf(Input IN, inout SurfaceOutput o) {
            // default colorShift to black
            fixed4 colorShift = fixed4(0,0,0,0);

            // each offset is 1/3 of circumfrance from each other
            fixed rOffset = 0;
            fixed gOffset = 2*PI / 3;
            fixed bOffset = gOffset * 2;
            // Assign colorShift.r based on color cycle function
            colorShift.r = calculateColor(rOffset);
            colorShift.g = calculateColor(gOffset);
            colorShift.b = calculateColor(bOffset);

            // assign color
            o.Albedo += colorShift.rgb;
            o.Alpha = 1;

            // get emission color
            // fixed3 emission = c.rgb * (pow(2,_EmissionIntensity));
            fixed3 emission = colorShift.rgb * _EmissionIntensity;
            // fluctuate emission Intensity
            // emission *= max(cos(_Time.y * _PulseSpeed), 0);
            // cyling through the texture provides the pulse "shape"
            fixed pulseIntensity = tex2D(_PulseTex, float2(_Time.y * _PulseSpeed, 0.5)).r;
            o.Albedo += emission * pulseIntensity;

            
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
