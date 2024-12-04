using UnityEngine;

namespace Stage.Utility
{
    public class RingStage : MonoBehaviour
    {
        /// <summary>
        /// 開始時に決定された初期角度角度を設定または取得します。
        /// </summary>
        public float InitDegree { get; set; }

        /// <summary>
        /// <see cref="RectTransform"/> を取得します。
        /// </summary>
        public RectTransform Rect => this.transform as RectTransform;
    }
}


