using System.Runtime.CompilerServices;
using UnityEngine;

namespace Stage.Utility
{
    public static class TransformExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXY(this Transform self, float xy)
        {
            Vector3 scale = self.localScale;
            scale.x = xy;
            scale.y = xy;
            self.localScale = scale;
        }
    }
}