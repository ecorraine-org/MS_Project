Shader "Hidden/PostProcess/ProceduralCrossHatchingWithDepth"
{
    HLSLINCLUDE
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

    // Uniforms
    float _Blend;
    float _DepthFactor;  // Factor to control how depth affects hatching size

    // Helper function to generate cross-hatch patterns
    float hatchPattern(float2 uv, float scale, float angle)
    {
        // Rotate UV coordinates to create diagonal hatches
        float2 rotatedUV = float2(
            uv.x * cos(angle) - uv.y * sin(angle),
            uv.x * sin(angle) + uv.y * cos(angle)
        );

        // Generate a repeating line pattern with the sine function
        float hatch = abs(sin(rotatedUV.x * scale)) * 0.5 + 0.5;
        return hatch;
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

        // Calculate luminance for the current pixel
        float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));

        // Sample depth texture and calculate depth factor
        float depth = GetDepth(i.texcoord);
        float depthFactor = lerp(1.0, _DepthFactor, depth);

        // Create three cross-hatching patterns with different densities, affected by depth
        float hatch1 = hatchPattern(i.texcoord, 40.0 * depthFactor, 0.0);     // Sparse cross-hatch (light)
        float hatch2 = hatchPattern(i.texcoord, 80.0 * depthFactor, 1.5708);  // Medium cross-hatch (diagonal)
        float hatch3 = hatchPattern(i.texcoord, 160.0 * depthFactor, 3.1416); // Dense cross-hatch (dark)

        // Blend between different hatch levels based on the luminance value
        float finalHatch = hatch1;
        if (luminance < 0.66) finalHatch = lerp(hatch1, hatch2, smoothstep(0.33, 0.66, luminance));
        if (luminance < 0.33) finalHatch = lerp(hatch2, hatch3, smoothstep(0.0, 0.33, luminance));

        // Blend the hatch pattern with the original color
        float4 hatchColor = lerp(color, float4(finalHatch, finalHatch, finalHatch, 1.0), _Blend);
        hatchColor.a = color.a;

        // Return the final cross-hatched color
        return hatchColor;
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
