using System;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(HalftoneRenderer), PostProcessEvent.AfterStack, "Custom/Halftone")]
public sealed class PPHalftone : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Halftone effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    [Tooltip("Factor controlling how depth affects the halftone size.")]
    public FloatParameter depthFactor = new FloatParameter { value = 1.0f };
}
public sealed class HalftoneRenderer : PostProcessEffectRenderer<PPHalftone>
{
    public override void Render(PostProcessRenderContext context)
    {
        // Ensure depth texture mode is enabled on the camera
        context.camera.depthTextureMode |= DepthTextureMode.Depth;

        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/HalftoneWithDepth"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_DepthFactor", settings.depthFactor);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
