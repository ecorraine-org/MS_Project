using UnityEngine;

namespace Stage.Utility
{
    public static class MathfUtil
    {
        // 角度をラジアンに変換する
        public static float ToRad(float deg) => deg * Mathf.Deg2Rad;

        // 指定した角度からXとYの組み合わせを取得する
        public static (float x, float y) GetPosDeg(float deg, float rate = 1.0f)
        {
            return GetPosRad(ToRad(deg), rate);
        }

        // 指定したラジアン角度からXとYの組み合わせを取得する
        public static (float x, float y) GetPosRad(float rad, float rate = 1.0f)
        {
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);

            if (rate == 1.0f)
            {
                return (x, y);
            }
            return (x * rate, y * rate);
        }
    }
}