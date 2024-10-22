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
}
public sealed class HalftoneRenderer : PostProcessEffectRenderer<PPHalftone>
{
   public override void Render(PostProcessRenderContext context)
  {
       var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/Halftone"));
       sheet.properties.SetFloat("_Blend", settings.blend);
       context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
  }
}
