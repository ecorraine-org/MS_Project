Shader "DepthTest"
{
    HLSLINCLUDE
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    struct v2f {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

    half4 frag(v2f i) : SV_Target {
        float2 uv = i.uv;
        
        // 深度値を取得
        float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,sampler_CameraDepthTexture, uv);
        return depth;
    }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

            #pragma vertex VertDefault
            #pragma fragment frag

            ENDHLSL
        }
    }
}