Shader "Custom/SciFiBackground_SparseRandomStars"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Dummy texture, not used for procedural parts

        // Starfield Properties
        _StarDensity ("Star Density (Higher for fewer stars)", Range(0.0, 1.0)) = 0.9998 // VERY HIGH: Significantly fewer stars
        _StarBaseBrightness ("Star Base Brightness", Range(0.0, 1.0)) = 0.3 
        _StarSizeVariation ("Star Size Variation", Range(0.0, 0.8)) = 0.5 
        _StarTwinkleMinSpeed ("Star Twinkle Min Speed", Range(0.0, 5.0)) = 0.3 
        _StarTwinkleMaxSpeed ("Star Twinkle Max Speed", Range(0.0, 10.0)) = 8.0 
        _StarTwinkleOffsetStrength ("Twinkle Offset Strength", Range(0.0, 30.0)) = 20.0 
        _StarColor ("Star Color", Color) = (1.0, 1.0, 1.0, 1.0)

        // Nebula Properties
        _NebulaColor1 ("Nebula Color 1", Color) = (0.1, 0.2, 0.4, 1.0)
        _NebulaColor2 ("Nebula Color 2", Color) = (0.05, 0.0, 0.15, 1.0)
        _NebulaScale ("Nebula Scale", Range(0.1, 10.0)) = 2.0
        _NebulaSpeed ("Nebula Speed", Range(0.0, 1.0)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; 

            // Shader properties
            float _StarDensity;
            float _StarBaseBrightness;
            float _StarSizeVariation;
            float _StarTwinkleMinSpeed;
            float _StarTwinkleMaxSpeed;
            float _StarTwinkleOffsetStrength;
            fixed4 _StarColor;

            fixed4 _NebulaColor1;
            fixed4 _NebulaColor2;
            float _NebulaScale;
            float _NebulaSpeed;

            // --- Utility Functions ---
            // Pseudo-random function (seeded by position)
            float rand(float2 co) {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            // 2D Noise function (Simplex-like, adapted for GLSL/HLSL)
            // Source: https://www.shadertoy.com/view/XsXGRW (simplified version)
            float noise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);

                float2 d = fp * fp * (3.0 - 2.0 * fp);

                float a = rand(ip);
                float b = rand(ip + float2(1.0, 0.0));
                float c = rand(ip + float2(0.0, 1.0));
                float d_val = rand(ip + float2(1.0, 1.0));

                float result = lerp(lerp(a, b, d.x), lerp(c, d_val, d.x), d.y);
                return result;
            }

            // Octave noise for more complex patterns
            float octavesNoise(float2 p, int octaves, float persistence, float lacunarity) {
                float total = 0.0;
                float frequency = 1.0;
                float amplitude = 1.0;
                float maxValue = 0.0; 

                for (int i = 0; i < octaves; i++) {
                    total += noise(p * frequency) * amplitude;
                    maxValue += amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                return total / maxValue;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; 
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 finalColor = fixed4(0, 0, 0, 1); 

                // --- 1. Nebula/Cloud ---
                float2 nebulaCoord = uv * _NebulaScale;
                nebulaCoord.x += _Time.y * _NebulaSpeed; 
                nebulaCoord.y += _Time.y * _NebulaSpeed * 0.5; 

                float nebulaNoise = octavesNoise(nebulaCoord, 4, 0.5, 2.0);
                nebulaNoise = pow(nebulaNoise, 2.0); 

                fixed3 nebulaColor = lerp(_NebulaColor2.rgb, _NebulaColor1.rgb, nebulaNoise);
                finalColor.rgb = nebulaColor; 


                // --- 2. Starfield ---
                // Scale UV for apparent star density
                float2 starUVScale = uv * 1000.0; 

                // Add a small, high-frequency random offset to the UVs before flooring.
                // This helps break up any grid-like patterns when stars are very sparse.
                starUVScale += rand(starUVScale * 123.45) * 0.1; // Random shift

                // Primary random for star existence, based on the pixel's "cell"
                float starRand = rand(floor(starUVScale)); 
                
                // Determine star's size/brightness random value (different seed)
                float starSizeRand = rand(floor(starUVScale + float2(1.0, 2.0))); 

                // Determine if a star exists at this pixel
                // _StarDensity is set very high by default for very few stars
                float starMask = step(_StarDensity, starRand); 

                // Calculate initial star brightness based on its random 'size' and base brightness
                // This gives the illusion of varying distances (closer = brighter/larger)
                float starBrightness = lerp(_StarBaseBrightness, _StarBaseBrightness + _StarSizeVariation, starSizeRand);
                
                // Add a subtle "glow" around the star
                float glow = 0.0;
                float glow_radius = 0.002; 
                glow += smoothstep(0.0, glow_radius, starRand - _StarDensity); 
                glow += smoothstep(0.0, glow_radius * 0.5, rand(floor(starUVScale + float2(0.5, 0.5))) - _StarDensity); 

                starBrightness *= (starMask + glow * 0.5); 

                // Randomized twinkling effect
                float twinkleSpeed = lerp(_StarTwinkleMinSpeed, _StarTwinkleMaxSpeed, rand(floor(starUVScale + float2(3.0, 4.0))));
                float twinkleOffset = rand(floor(starUVScale + float2(5.0, 6.0))) * _StarTwinkleOffsetStrength;

                float twinkle = sin(_Time.y * twinkleSpeed + twinkleOffset) * 0.5 + 0.5; 
                twinkle = pow(twinkle, 1.5); 

                starBrightness *= twinkle; 

                // Blend stars onto the existing nebula color
                finalColor.rgb += _StarColor.rgb * starBrightness;


                // --- Fog (Unity's built-in fog) ---
                UNITY_APPLY_FOG(i.fogCoord, finalColor);

                return finalColor;
            }
            ENDCG
        }
    }
}