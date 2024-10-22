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
}
public sealed class CrossHatchRenderer : PostProcessEffectRenderer<PPCrossHatching>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/ProceduralCrossHatchingWithColor"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
