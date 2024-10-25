Shader "Custom/HalftoneToonLambert"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Color", Color) = (1,1,1,1)
        _ToonSteps ("Toon Steps", Range(1, 8)) = 4
        _DotSize("Dot Size", Range(0.01, 0.5)) = 0.1 // Size of the halftone dots
        _DotSpacing("Dot Spacing", Range(0.001, 0.1)) = 0.05 // Spacing between the dots
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Overlay"
            "Queue" = "Overlay"
            "LightMode" = "ForwardBase"
        }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
            float _HalftoneSize;
            float _CrossHatchSize;
            float _DotSpacing;
            float _DotSize;

            // Halftone function to create dot pattern
            half4 Halftone(fixed4 texColor, float2 uv, float intensity)
            {
                // Adjust dot spacing based on light intensity
                float adjustedDotSpacing = _DotSpacing * (1.0 - intensity);
                // Calculate the grid position
                float2 gridPos = frac(uv / adjustedDotSpacing);
                // Create a circular dot pattern
                float dot = step(length(gridPos - 0.5), _DotSize); // Circle with radius _DotSize
                // Use the dot to mask the texture color
                return texColor * dot;
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

                // Apply toon shading by stepping the diffuse value
                half stepSize = 1.0 / _ToonSteps;
                half toonDiff = floor(nDotL / stepSize) * stepSize;

                // Halftone effect applied only to shadowed areas
                float4 halftoneColor = texColor;
                if (toonDiff < 0.1)
                {
                    // Scale the halftone based on light intensity
                    halftoneColor = Halftone(texColor, i.uv, toonDiff);
                }

                // Combine the colors, applying toon shading
                // fixed4 finalColor = (texColor * toonDiff - halftoneColor);
                fixed4 finalColor = halftoneColor / (halftoneColor - toonDiff);
                finalColor = texColor * (texColor - finalColor);

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
