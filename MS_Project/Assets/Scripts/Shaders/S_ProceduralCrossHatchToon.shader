Shader "Custom/ProceduralCrossHatchToonLambert"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Color", Color) = (1,1,1,1)
        _ToonSteps ("Toon Steps", Range(1, 8)) = 4
        _LineSpacing ("Line Spacing", Range(0.001, 0.5)) = 0.1 // Spacing between cross-hatch lines
        _LineWidth ("Line Width", Range(0.001, 1)) = 0.02 // Width of the cross-hatch lines
        _Angle ("Line Angle", Range(0, 360)) = 45 // Angle of the lines in degrees
        _Density ("Line Density", Range(1, 10)) = 5 // Number of lines per unit area
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "LightMode" = "ForwardBase"
        }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            int _ToonSteps;
            float _LineSpacing;
            float _LineWidth;
            float _Angle;
            float _Density;

            // Cross-hatch pattern function
            half4 CrossHatchPattern(float4 color, float2 uv, float intensity)
            {
                // Convert angle from degrees to radians
                float angleRad = _Angle * 3.14159265 / 180.0;
                float cosAngle = cos(angleRad);
                float sinAngle = sin(angleRad);

                // Adjust line spacing based on light intensity
                float adjustedLineSpacing = _LineSpacing * (1.0 - intensity) * (1.0 / _Density);
                float2 lines = frac(uv / adjustedLineSpacing);

                // Create the cross-hatch effect
                /*
                float line1 = step(lines.x, _LineWidth) * step(lines.y, _LineWidth);
                float line2 = step(lines.y, _LineWidth) * step(1.0 - lines.x, _LineWidth);
                */
                float line1 = step(lines.x * cosAngle + lines.y * sinAngle, _LineWidth);
                float line2 = step(lines.y * cosAngle - lines.x * sinAngle, _LineWidth);

                // Combine lines for a cross-hatch pattern
                float hatch = max(line1, line2);
                
                // Use the hatch to mask the texture color
                return color * hatch;
            }

            // Toon shading function
            float ToonLighting(float lightIntensity, float _StepSize)
            {
                float steps = 1.0 / _StepSize;
                return floor(lightIntensity / steps) * steps;
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                float3 normal = normalize(i.normal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float nDotL = max(dot(normal, lightDir), 0.0);

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float toonDiff = ToonLighting(nDotL, _ToonSteps);

                // Cross-hatch effect applied only to shadowed areas
                float4 crossHatchColor = texColor;
                if (toonDiff < 0.1)
                {
                    // Scale the cross-hatch based on light intensity
                    crossHatchColor = CrossHatchPattern(texColor, i.uv, toonDiff);
                }

                // Apply toon shading and cross-hatch effect on shadows
                fixed4 finalColor = crossHatchColor / (crossHatchColor - toonDiff);
                finalColor = (texColor - finalColor) * (crossHatchColor - toonDiff);
                finalColor.rgb += _LightColor0.rgb;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
