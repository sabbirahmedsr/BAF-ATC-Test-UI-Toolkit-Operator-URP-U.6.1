Shader "Custom/MatrixSmoothLinesAndDots"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0,1,0,1) // Green
        _GridSize ("Grid Size", Range(10.0, 200.0)) = 100.0 // Increased default for finer detail
        _ScrollSpeed ("Scroll Speed", Range(0.1, 5.0)) = 1.0
        _LineLength ("Line Segment Length", Range(0.01, 1.0)) = 0.5 // Relative to cell height
        _LineWidth ("Line Width", Range(0.001, 0.05)) = 0.005 // Default to thinner lines
        _DotSize ("Dot Size", Range(0.001, 0.05)) = 0.01
        _FlickerAmount ("Flicker Amount", Range(0.0, 0.5)) = 0.1 // Renamed and reduced range for subtlety
        _GlowIntensity ("Glow Intensity", Range(0.0, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" } // Or "Transparent" if you want transparency
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _MainColor;
            float _GridSize;
            float _ScrollSpeed;
            float _LineLength;
            float _LineWidth;
            float _DotSize;
            float _FlickerAmount;
            float _GlowIntensity;

            // --- Helper Functions (Hash functions for pseudo-randomness) ---
            // A simple hash function from a float2 to a float
            float hash_f2_f1(float2 p) {
                p  = 50.0 * frac(p * 0.3183099 + p.yx * 0.3679099);
                return frac(p.x * p.y * (p.x + p.y));
            }

            // Perlin noise (or a simple value noise for smoother randomness)
            // This will be used for smoother flicker
            float snoise(float2 p) {
                // Simplified 2D value noise for smooth transitions
                // This is a common pattern for value noise.
                float2 ip = floor(p);
                float2 fp = frac(p);
                fp = fp * fp * (3.0 - 2.0 * fp); // Smooth curve for interpolation

                float tl = hash_f2_f1(ip);
                float tr = hash_f2_f1(ip + float2(1.0, 0.0));
                float bl = hash_f2_f1(ip + float2(0.0, 1.0));
                float br = hash_f2_f1(ip + float2(1.0, 1.0));

                float mixX0 = lerp(tl, tr, fp.x);
                float mixX1 = lerp(bl, br, fp.x);

                return lerp(mixX0, mixX1, fp.y);
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 1. Map UVs to the grid space
                float2 gridCoord = uv * _GridSize;
                float2 cellID = floor(gridCoord);
                float2 cellUV = frac(gridCoord);

                // Re-calculate currentCellY for smooth continuous fall within cells
                float currentCellY = frac(uv.y * _GridSize + _Time.y * _ScrollSpeed);

                // 2. Generate random properties for the current cell
                float rnd = hash_f2_f1(cellID); // Random value for this specific grid cell

                // 3. Determine if this cell should have a line or a dot
                float lineProbability = 0.8; // Increased probability for lines
                float dotProbability = 0.1;  // Slightly reduced for dots
                float lineThreshold = lineProbability;
                float dotThreshold = lineProbability + dotProbability;

                float intensity = 0.0;

                // 4. Draw Lines (main falling characters)
                if (rnd < lineThreshold)
                {
                    float lineStart = 1.0 - _LineLength; // Lines start near the top of the cell
                    float lineEnd = 1.0;                 // And fall to the bottom of the cell

                    // Use currentCellY to check if it's within the line segment
                    float lineAlpha = smoothstep(lineStart - _LineWidth, lineStart + _LineWidth, currentCellY) -
                                      smoothstep(lineEnd - _LineWidth, lineEnd + _LineWidth, currentCellY);

                    // Fade the line as it falls within its own segment
                    // This creates the "trailing" effect
                    float fadeY = frac(uv.y * _GridSize + _Time.y * _ScrollSpeed); // Continuous Y for falloff
                    float falloffStart = 1.0 - _LineLength * 1.2; // Start fading a bit earlier for smoother trail
                    float falloffEnd = 1.0;
                    float falloff = smoothstep(falloffStart, falloffStart + 0.1, fadeY) * // Fade in
                                    (1.0 - smoothstep(falloffEnd - 0.1, falloffEnd, fadeY)); // Fade out

                    lineAlpha = saturate(lineAlpha); // Clamp between 0 and 1
                    lineAlpha *= falloff;            // Apply the falling trail fade

                    intensity += lineAlpha;
                }
                // 5. Draw Dots (random 'sparkle' points)
                else if (rnd < dotThreshold)
                {
                    float dotDistance = distance(cellUV, float2(0.5, 0.5)); // Dot in center of cell
                    float dotAlpha = smoothstep(_DotSize * 0.8, _DotSize * 0.2, dotDistance);
                    intensity += dotAlpha;
                }

                // 6. Add Smoothed Flicker Effect
                // Use snoise for a smoother, less abrupt flicker.
                // The time component is scaled down for slower transitions.
                float flickerNoise = snoise(cellID * 0.5 + _Time.y * 0.5); // Slower time scaling
                // Remap flickerNoise from (0,1) to a smaller range around 1.0 to dim/brighten slightly.
                float flickerMod = lerp(1.0 - _FlickerAmount, 1.0 + _FlickerAmount, flickerNoise);
                intensity *= flickerMod;


                // 7. Apply glow intensity and main color
                fixed4 col = _MainColor * intensity * _GlowIntensity;

                col.a = 1.0; // Ensure alpha is fully opaque if RenderType="Opaque"

                return col;
            }
            ENDCG
        }
    }
}