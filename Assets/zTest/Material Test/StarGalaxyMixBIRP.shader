Shader "Custom/StarGalaxyMixBIRP"
{
    Properties
    {
        _StarDensityDistant ("Distant Star Density", Range(0, 10)) = 1.0
        _StarBrightnessDistant ("Distant Star Brightness", Range(0, 10)) = 1.0
        _TwinkleSpeedDistant ("Distant Twinkle Speed", Range(0, 10)) = 1.0
        _StarColorDistant ("Distant Star Color", Color) = (1,1,1,1)

        _StarDensityClose ("Close Star Density", Range(0, 10)) = 0.5
        _StarBrightnessClose ("Close Star Brightness", Range(0, 10)) = 2.0
        _TwinkleSpeedClose ("Close Twinkle Speed", Range(0, 10)) = 2.0
        _StarColorClose ("Close Star Color", Color) = (1,1,1,1)
        _ParallaxScaleClose ("Close Star Parallax Scale", Range(0, 1)) = 0.1

        _GalaxyDensity ("Galaxy Density", Range(0, 1)) = 0.5
        _GalaxyBrightness ("Galaxy Brightness", Range(0, 5)) = 1.0
        _GalaxyColor1 ("Galaxy Color 1", Color) = (0.2,0.4,1,1) // Blue
        _GalaxyColor2 ("Galaxy Color 2", Color) = (1,0.5,0.2,1) // Orange
        _GalaxyArmTwist ("Galaxy Arm Twist", Range(0, 5)) = 1.0
        _GalaxyCoreSize ("Galaxy Core Size", Range(0, 1)) = 0.1
        _GalaxySpeed ("Galaxy Speed", Range(0, 1)) = 0.01

        _NebulaDensity ("Nebula Density", Range(0, 1)) = 0.5
        _NebulaBrightness ("Nebula Brightness", Range(0, 5)) = 0.5
        _NebulaColor1 ("Nebula Color 1", Color) = (0.5,0.2,0.8,1) // Purple
        _NebulaColor2 ("Nebula Color 2", Color) = (0.2,0.8,0.5,1) // Green
        _NebulaSpeed ("Nebula Speed", Range(0, 1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Fog { Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // For _Time, UnityObjectToClipPos, etc.
            #include "Lighting.cginc" // For other potential lighting needs, though less critical for skybox

            // --- Properties (declared as uniforms for access in shaders) ---
            float _StarDensityDistant;
            float _StarBrightnessDistant;
            float _TwinkleSpeedDistant;
            fixed4 _StarColorDistant;

            float _StarDensityClose;
            float _StarBrightnessClose;
            float _TwinkleSpeedClose;
            fixed4 _StarColorClose;
            float _ParallaxScaleClose;

            float _GalaxyDensity;
            float _GalaxyBrightness;
            fixed4 _GalaxyColor1;
            fixed4 _GalaxyColor2;
            float _GalaxyArmTwist;
            float _GalaxyCoreSize;
            float _GalaxySpeed;

            float _NebulaDensity;
            float _NebulaBrightness;
            fixed4 _NebulaColor1;
            fixed4 _NebulaColor2;
            float _NebulaSpeed;

            // --- 3D Value Noise Implementation ---
            // A simple noise function, suitable for diffuse nebula and some star distribution.
            // For true Simplex noise, you would copy a robust implementation from a known source.

            float hash(float n) { return frac(sin(n) * 43758.5453123); }

            float vnoise(float3 x)
            {
                float3 p = floor(x);
                float3 f = frac(x);
                f = f * f * (3.0 - 2.0 * f); // Smoother interpolation (quintic)

                float n = p.x + p.y * 57.0 + p.z * 113.0; // A simple hashing for integer coordinates

                // Interpolate 8 corners of the cube
                float res = lerp(
                    lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
                         lerp(hash(n + 57.0), hash(n + 57.0 + 1.0), f.x), f.y),
                    lerp(lerp(hash(n + 113.0), hash(n + 113.0 + 1.0), f.x),
                         lerp(hash(n + 170.0), hash(n + 170.0 + 1.0), f.x), f.y), f.z);
                return res;
            }


            // --- Helper Functions ---
            // Random number generator from a seed
            float rand(float3 seed)
            {
                return frac(sin(dot(seed.xyz, float3(12.9898, 78.233, 37.719))) * 43758.5453123);
            }

            // Remap function
            float remap(float value, float inMin, float inMax, float outMin, float outMax)
            {
                return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
            }

            // --- Vertex Shader Input/Output ---
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldDir : TEXCOORD0; // Direction from camera to skybox point
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // For a skybox, the worldDir should essentially be the object's position transformed
                // to world space (which is the camera's view direction when looking at the skybox).
                // Multiplying by unity_ObjectToWorld will give the world-space direction.
                o.worldDir = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
                return o;
            }

            // --- Fragment Shader ---
            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir = normalize(i.worldDir); // Normalized direction vector

                fixed3 finalColor = fixed3(0,0,0);

                // --- 1. Distant Stars ---
                // Use a seeded random grid to place stars.
                float3 cellPosDistant = floor(dir * 500.0); // Large scale for distant stars
                float starSeedDistant = rand(cellPosDistant);

                if (starSeedDistant < _StarDensityDistant * 0.0005) // Low probability, adjusted for range
                {
                    float3 starPosDistant = cellPosDistant + rand(cellPosDistant + 1.0); // Offset within cell
                    float distFromCenterDistant = length(dir * 500.0 - starPosDistant); // Compare at the same scale

                    // Twinkle effect
                    float twinkle = sin(_Time.y * _TwinkleSpeedDistant + starSeedDistant * 100.0) * 0.5 + 0.5;
                    float starSizeDistant = remap(starSeedDistant, 0, 1, 0.5, 2.5); // Vary size slightly
                    float starBrightnessDistant = _StarBrightnessDistant * twinkle;

                    finalColor += _StarColorDistant.rgb * starBrightnessDistant * (1.0 - smoothstep(0, starSizeDistant, distFromCenterDistant));
                }

                // --- 2. Close Stars (with parallax) ---
                float3 parallaxOffset = _Time.y * _ParallaxScaleClose * float3(0.01, 0.005, 0.002);
                float3 parallaxDir = dir + parallaxOffset;
                float3 cellPosClose = floor(parallaxDir * 50.0); // Smaller scale for closer stars
                float starSeedClose = rand(cellPosClose + 0.5);

                if (starSeedClose < _StarDensityClose * 0.005) // Higher probability for close stars
                {
                    float3 starPosClose = cellPosClose + rand(cellPosClose + 1.5);
                    float distFromCenterClose = length(parallaxDir * 50.0 - starPosClose);

                    float twinkle = sin(_Time.y * _TwinkleSpeedClose + starSeedClose * 50.0) * 0.5 + 0.5;
                    float starSizeClose = remap(starSeedClose, 0, 1, 0.5, 5.0); // Larger size
                    float starBrightnessClose = _StarBrightnessClose * twinkle;

                    finalColor += _StarColorClose.rgb * starBrightnessClose * (1.0 - smoothstep(0, starSizeClose, distFromCenterClose));
                }

                // --- 3. Galaxy Core/Arms ---
                float2 uvGalaxy = dir.xz; // Project to a 2D plane for galaxy shape
                float angle = atan2(uvGalaxy.y, uvGalaxy.x);
                float radius = length(uvGalaxy);

                // Add time-based rotation
                angle += _Time.y * _GalaxySpeed;

                // Simple spiral arm formula, adjusted for better shape
                float spiralFactor = fmod(angle + radius * _GalaxyArmTwist, UNITY_PI * 2.0) / (UNITY_PI * 2.0);
                float galaxyShape = pow(sin(spiralFactor * UNITY_PI), 4.0); // Creates an arm-like shape

                // Core density
                float coreDensity = 1.0 - smoothstep(0.0, _GalaxyCoreSize, radius);
                galaxyShape = max(galaxyShape, coreDensity); // Combine core and arms

                // Add noise for irregularity using the provided vnoise function
                float galaxyNoise = vnoise(dir * 5.0 + _Time.y * 0.01);
                galaxyShape *= remap(galaxyNoise, 0, 1, 0.5, 1.0); // Modulate shape with noise

                float galaxyDensity = galaxyShape * _GalaxyDensity;
                // Example: vertical color blend based on world Y-axis
                float galaxyColorBlend = saturate(remap(dir.y, -0.5, 0.5, 0.0, 1.0));
                fixed3 galaxyColor = lerp(_GalaxyColor1.rgb, _GalaxyColor2.rgb, galaxyColorBlend);
                finalColor += galaxyColor * galaxyDensity * _GalaxyBrightness;


                // --- 4. Nebulae/Gas Clouds ---
                // Using multiple octaves of vnoise for cloudy appearance.
                float3 noisePos = dir * 2.0 + _Time.y * _NebulaSpeed;
                float nebulaNoise1 = vnoise(noisePos);
                float nebulaNoise2 = vnoise(noisePos * 2.0 + 10.0);
                float nebulaNoise3 = vnoise(noisePos * 4.0 + 20.0);

                float nebulaDensity = (nebulaNoise1 * 0.6 + nebulaNoise2 * 0.3 + nebulaNoise3 * 0.1);
                nebulaDensity = saturate(remap(nebulaDensity, 0.3, 0.7, 0.0, 1.0)); // Remap and clamp for sharper edges
                nebulaDensity *= _NebulaDensity;

                fixed3 nebulaColor = lerp(_NebulaColor1.rgb, _NebulaColor2.rgb, saturate(vnoise(dir * 3.0 + _Time.y * 0.05)));
                finalColor += nebulaColor * nebulaDensity * _NebulaBrightness;

                // Final clamping
                finalColor = saturate(finalColor);

                return fixed4(finalColor, 1.0);
            }
            ENDCG // <--- THIS WAS MISSING
        }
    }
    Fallback "Skybox/Procedural" // Fallback to Unity's default procedural skybox if this shader fails.
}