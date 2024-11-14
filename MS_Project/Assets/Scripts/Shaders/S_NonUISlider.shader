Shader "Custom/NonUISlider"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _Sides ("Sides", Int) = 6
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.5)) = 0.1
        _Value ("Value", Range(0.0, 1.0)) = 0.5
        _Opacity ("Opacity", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define PI 3.14159265358979323846

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            int _Sides;
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineThickness;
            float _Value;
            float _Opacity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy; // Pass the UV coordinates
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 absUV = abs(i.uv);

                // create shape
                float a = atan2(absUV.x, absUV.y) + PI;
                float r = (PI * 2) / float(_Sides);
                float d = cos(floor(0.5 + a / r) * r - a) * length(absUV);

                // fill shape using smoothstep
                float gradient = _Value - 0.5;
                float fillFactor = smoothstep(0.1, 0.0, i.uv.x - gradient);
                float s = smoothstep(0.41, 0.4, d) * fillFactor;
                float4 sColor = float4(s, s, s, s);

                // create outline
                float outlineFactor = smoothstep(0.41 + _OutlineThickness, 0.4, d);
                float4 outlineColor = _OutlineColor * (outlineFactor - smoothstep(0.41, 0.4, d));

                if (sColor.r < 0.1)
                {
                    sColor.a = 0;
                }

                fixed4 finalColor = _Color * sColor + outlineColor;
                finalColor.a = _Opacity;
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
