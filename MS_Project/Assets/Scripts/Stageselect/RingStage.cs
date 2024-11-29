using UnityEngine;

namespace Stage.Utility
{
    public class RingStage : MonoBehaviour
    {
        /// <summary>
        /// �J�n���Ɍ��肳�ꂽ�����p�x�p�x��ݒ�܂��͎擾���܂��B
        /// </summary>
        public float InitDegree { get; set; }

        /// <summary>
        /// <see cref="RectTransform"/> ���擾���܂��B
        /// </summary>
        public RectTransform Rect => this.transform as RectTransform;
    }
}


