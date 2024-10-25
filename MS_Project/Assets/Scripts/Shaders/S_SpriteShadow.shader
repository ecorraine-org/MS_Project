// Based off Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/SpriteShadow"
{
    Properties
    {
        [PerRendererData] _MainTex("Texture", 2D) = "white" {}
        [PerRendererData] _NormalMap("Normal Map", 2D) = "white" {}
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel Snap", Float) = 0
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _ZTest("ZTest", Float) = 4  // 4 = LEqual
        _DotSize("Dot Size", Range(0.01, 0.5)) = 0.1 // Size of the halftone dots
        _DotSpacing("Dot Spacing", Range(0.001, 0.1)) = 0.05 // Spacing between the dots
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Overlay"
            "LightMode" = "ForwardBase"
            "PreviewType" = "Plane"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "True"
        }

        Blend One OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest [_ZTest]
        Lighting On

        CGPROGRAM
        #pragma surface surf WrapLambert vertex:vert alphatest:_Cutoff addshadow nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile_local _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"

        sampler2D _NormalMap;
        float _DotSize;
        float _DotSpacing;

        // custom surface lighting
        half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            half diff = NdotL * 0.5 + 0.5;

            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
            c.a = s.Alpha;
            return c;
        }

        // Function to apply halftone pattern
        half4 Halftone(fixed4 texColor, float2 uv)
        {
            // Calculate the grid position
            float2 gridPos = frac(uv / (_DotSpacing));
            // Create a circular dot pattern
            float dot = step(length(gridPos - 0.5), _DotSize); // Circle with radius _DotSize
            // Use the dot to mask the texture color
            return texColor * dot;
        }

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;
            fixed4 color;
        };

        void vert(inout appdata_full v, out Input o)
        {
            v.vertex = UnityFlipSprite(v.vertex, _Flip);

        #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap(v.vertex);
        #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * IN.color;

            float3 normalTex = tex2D(_NormalMap, IN.uv_NormalMap).rgb * 2.0 - 1.0;
            float3 normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normalTex));

            float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
            float NdotL = max(dot(normal, lightDir), 0.0);

            texColor = texColor + Halftone(texColor, IN.uv_MainTex);
            
            if (NdotL < 0.0)
            {
                // Calculate shadow color with halftone
                fixed4 shadowColor = fixed4(0.0, 0.0, 0.0, 1.0); // Adjust as needed for shadow color
                shadowColor = Halftone(shadowColor, IN.uv_MainTex); // Apply halftone to shadow
                texColor *= shadowColor.a; // Combine shadow effect
            }
            
            o.Albedo = texColor.rgb /* * texColor.a */;
            o.Alpha = texColor.a;
        }
        ENDCG
    }

    Fallback "Diffuse"
}
