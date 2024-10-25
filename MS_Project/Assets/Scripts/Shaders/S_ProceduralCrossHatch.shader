Shader "Custom/BlendedHatchingShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Base Texture", 2D) = "white" {}
        [HideInInspector] _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Opacity ("Opacity", Float) = 0.5
        _HatchColor ("Hatch Color", Color) = (0,0,0,1)
        _HatchDensity ("Hatch Density", Float) = 10.0
        _CurvatureEffect ("Curvature Effect", Float) = 0.5
        _AngleOffset ("Angle Offset", Float) = 45.0
        _BlendFactor ("Blend Factor", Range(0,1)) = 0.5
        _ShadowHatchFactor ("Shadow Hatch Factor", Range(0,1)) = 1.0 // Controls hatching strength in shadow
        _DepthInfluence ("Depth Influence", Range(0, 1)) = 0.5 // Optional depth-based effect
        _LightSteps ("Light Steps", Float) = 4.0 // Controls the number of steps for toon lighting
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Overlay"
            "Queue"="Overlay"
            "LightMode" = "ForwardBase"
        }
        LOD 200

        Pass
        {
            // Disable depth writes and only blend for the overlay effect
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
            };

            sampler2D _MainTex;
            sampler2D _ShadowMapTexture;
            float4 _BaseColor;
            float4 _HatchColor;
            float _Opacity;
            float _HatchDensity;
            float _CurvatureEffect;
            float _AngleOffset;
            float _BlendFactor;
            float _ShadowHatchFactor;
            float _DepthInfluence;
            float _LightSteps;

            float4x4 _WorldToShadow;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.shadowCoord = mul(UNITY_MATRIX_VP, v.vertex);
                return o;
            }

            // Procedural stylized hatching pattern based on UV and worldNormal
            float stylizedHatch(float2 uv, float3 worldNormal, float hatchDensity, float angleOffset, float curvature)
            {
                // Adjust the angle of the hatch lines based on surface curvature and angle offset
                float curvatureInfluence = dot(worldNormal, float3(0,1,0)) * curvature; 
                float angle = radians(angleOffset + curvatureInfluence * 90.0); // Stylize based on curvature

                // Rotate the UV space for directional lines
                float2 rotatedUV = float2(
                    uv.x * cos(angle) - uv.y * sin(angle),
                    uv.x * sin(angle) + uv.y * cos(angle)
                );

                // Create the line pattern based on the rotated UV coordinates
                float linePattern = abs(frac(rotatedUV.x * hatchDensity) - 0.5);
                return linePattern;
            }

            // Toon shading function
            float toonLighting(float lightIntensity, float steps)
            {
                return ceil(lightIntensity * steps) / steps;
            }

            // Calculate shadow attenuation
            float calculateShadow(float4 shadowCoord)
            {
                // Sample the shadow map using projected coordinates
                float shadowValue = tex2D(_ShadowMapTexture, shadowCoord.xy).r;
                return shadowValue;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the base texture
                fixed4 baseTexture = tex2D(_MainTex, i.uv);
                fixed4 texColor = _BaseColor * baseTexture;
                // texColor.a = _Opacity;

                // Generate the procedural stylized hatching
                float hatchValue = stylizedHatch(i.uv, i.worldNormal, _HatchDensity, _AngleOffset, _CurvatureEffect);

                // Calculate lighting and hatching effect
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float lightIntensity = saturate(dot(i.worldNormal, lightDir));
                lightIntensity = toonLighting(lightIntensity, _LightSteps); // Toon lighting with steps

                 // Incorporate shadow
                float shadowAttenuation = calculateShadow(i.shadowCoord);
                lightIntensity *= shadowAttenuation; // Modulate light intensity based on shadow

                // Apply shadow hatch factor
                float hatchingEffect = smoothstep(0.0, 0.5, lightIntensity - hatchValue);
                hatchingEffect = lerp(hatchingEffect, 0, _ShadowHatchFactor * (1.0 - shadowAttenuation)); // Reduce hatching in shadow

                // Blend base texture with hatching effect
                float4 hatchColor = lerp(_BaseColor, _HatchColor, hatchingEffect);
                hatchColor.a = _Opacity;

                // Depth-based hatching adjustment
                float depthFactor = saturate(i.pos.z * _DepthInfluence);
                hatchValue *= depthFactor;

                // Combine the hatching effect and texture using blend factor
                float4 finalColor = lerp(texColor, hatchColor, _BlendFactor);

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}