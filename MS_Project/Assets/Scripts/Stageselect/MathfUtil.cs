using UnityEngine;

namespace Stage.Utility
{
    public static class MathfUtil
    {
        // �p�x�����W�A���ɕϊ�����
        public static float ToRad(float deg) => deg * Mathf.Deg2Rad;

        // �w�肵���p�x����X��Y�̑g�ݍ��킹���擾����
        public static (float x, float y) GetPosDeg(float deg, float rate = 1.0f)
        {
            return GetPosRad(ToRad(deg), rate);
        }

        // �w�肵�����W�A���p�x����X��Y�̑g�ݍ��킹���擾����
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