using System.Runtime.CompilerServices;
using UnityEngine;

namespace Stage.Utility
{
    public static class RectTransformExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAnchoredPosZ(this RectTransform self)
        {
            return self.anchoredPosition3D.z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPos(this RectTransform self, float x, float y)
        {
            var ancPos = self.anchoredPosition;
            ancPos.Set(x, y);
            self.anchoredPosition = ancPos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPosZ(this RectTransform self, float z)
        {
            var ancPos = self.anchoredPosition3D;
            ancPos.z = z;
            self.anchoredPosition3D = ancPos;
        }
    }
}
