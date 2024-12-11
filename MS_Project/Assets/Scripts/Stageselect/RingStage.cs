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

        [Header("ステージのロック")]
        public bool isLocked = true;

        [Header("ロック中に表示させるアイコン")]
        [SerializeField] private GameObject lockIcon;

        // ステージのロックを解除する関数
        public void UnlockNextStage(RingStage nextStage)
        {
            nextStage.isLocked = false;
        }

        /// <summary>
        /// ロック状態に応じてアイコンやUIを更新する。
        /// </summary>
        public void Update()
        {
            if (isLocked == false)
            {
                lockIcon.SetActive(false);
            }
            else if (isLocked == true)
            {
                lockIcon.SetActive(true);
            }
        }
    }
}


