Shader "Hidden/CloudShadow"
{
    Properties
    {
        _ShadowTexture ("Shadow Texture", 2D) = "white" {}
        _ShadowStrength ("Shadow Strength", Range(0, 1)) = 0.5
        _ScrollOffset ("Scroll Offset", Vector) = (0, 0, 0, 0)
        _Scale ("Scale", Float) = 100
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend DstColor Zero
            ZWrite Off
            ZTest Always

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
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _ShadowTexture;
            float _ShadowStrength;
            float2 _ScrollOffset;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 worldUV = i.worldPos.xz / _Scale + _ScrollOffset;
                fixed shadow = tex2D(_ShadowTexture, worldUV).r;
                shadow = lerp(1, shadow, _ShadowStrength);
                return fixed4(shadow, shadow, shadow, 1);
            }
            ENDCG
        }
    }
}