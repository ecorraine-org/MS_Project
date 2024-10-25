Shader "Hidden/PostProcess/HalftoneWithDepth"
{
    HLSLINCLUDE
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

    // Uniforms
    float _Blend;        // Blending factor
    float _DepthFactor;  // Factor to control how depth affects halftone size

    // Function to generate halftone dots based on luminance and depth
    float halftonePattern(float2 uv, float luminance, float dotScale, float depthFactor)
    {
        // Create a grid pattern
        float2 grid = frac(uv * dotScale) - 0.5;

        // Calculate the distance from the center of the grid cell
        float dist = length(grid);

        // Determine the dot size based on luminance and depthFactor
        float dotSize = (1.0 - luminance) * 0.5 * depthFactor;

        // Return 1.0 (white) if outside the dot size, 0.0 (black) otherwise
        return smoothstep(dotSize, dotSize + 0.02, dist);
    }

    // Function to sample the depth texture and remap the depth to 0-1
    float GetDepth(float2 uv)
    {
        // Sample depth texture
        float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;

        // Convert depth to linear 0-1 range using Linear01Depth
        depth = Linear01Depth(depth);
        return depth;
    }

    // Fragment shader
    float4 Frag(VaryingsDefault i) : SV_Target
    {
        // Sample the original texture
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

        // Calculate the luminance for the current pixel
        float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));

        // Sample depth texture and calculate depth factor
        float depth = GetDepth(i.texcoord);
        float depthFactor = lerp(1.0, _DepthFactor, depth);

        // Create a halftone pattern using a scale for dot density modified by depth
        float halftone = halftonePattern(i.texcoord, luminance, 100.0, depthFactor);

        // Mix between original color and halftone effect based on the _Blend factor
        float4 halftoneColor = lerp(color, float4(halftone, halftone, halftone, 1.0), _Blend);

        // Return the final color
        return halftoneColor;
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }
    }
}
