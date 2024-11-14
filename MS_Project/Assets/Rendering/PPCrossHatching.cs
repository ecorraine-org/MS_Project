using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CrossHatchRenderer), PostProcessEvent.AfterStack, "Custom/PostProcessCrossHatching")]
public sealed class PPCrossHatching : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Cross Hatch effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    [Tooltip("Factor controlling how depth affects the cross-hatching size.")]
    public FloatParameter depthFactor = new FloatParameter { value = 1.0f };
}
public sealed class CrossHatchRenderer : PostProcessEffectRenderer<PPCrossHatching>
{
    public override void Render(PostProcessRenderContext context)
    {
        // Ensure depth texture mode is enabled on the camera
        context.camera.depthTextureMode |= DepthTextureMode.Depth;

        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/ProceduralCrossHatchingWithDepth"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_DepthFactor", settings.depthFactor);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
