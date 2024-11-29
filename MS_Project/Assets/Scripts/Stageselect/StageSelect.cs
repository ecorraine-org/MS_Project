using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

namespace Stage.Utility
{
    public class StageSelect : MonoBehaviour
    {
        [SerializeField] private List<RingStage> stageList = new List<RingStage>();
        // リングの横幅
        [SerializeField] private float ringWidth;
        // リングの縦幅
        [SerializeField] private float ringHeight;
        // 移動のインターバル
        [SerializeField] private float magnetSpeed = 0.18f;
        // 要素が一番後ろに移動したときの縮小率(0.5 = 半分の大きさ)
        [SerializeField] private float backZoomScale = 0.5f;

        // 左右の回転量
        private float stepAmount;
        // 要素の間隔・角度
        private float oneAngle;
        // 目標位置 -> 回転させた回数(右+1, 左-1)
        private int count;
        // リングの前後関係整列用のバッファー
        private List<RingStage> stageListCache = new List<RingStage>();

        // 最前面の要素
        public RingStage frontStage;

        bool fIris = false; // アイリスアウトのアニメーションフラグ

        [SerializeField] RectTransform unmask;
        //readonly Vector2 IRIS_IN_SCALE = new Vector2(50, 50);
        readonly float SCALE_DURATION = 2;
        [Header("ステージ１の名前")]
        [SerializeField] string sceneToLoad1;
        [Header("ステージ２の名前")]
        [SerializeField] string sceneToLoad2;
        [Header("ステージ３の名前")]
        [SerializeField] string sceneToLoad3;
        [Header("ステージ４の名前")]
        [SerializeField] string sceneToLoad4;
        [Header("ステージ５の名前")]
        [SerializeField] string sceneToLoad5;
        [Header("ステージ６の名前")]
        [SerializeField] string sceneToLoad6;
        [Header("ステージ７の名前")]
        [SerializeField] string sceneToLoad7;
        [Header("ステージ８の名前")]
        [SerializeField] string sceneToLoad8;
        [Header("ステージ９の名前")]
        [SerializeField] string sceneToLoad9;

        void Start()
        {
            // 持ってる要素数に応じて初期位置を計算する
            this.oneAngle = 360.0f / this.stageList.Count;
            for (int i = 0; i < this.stageList.Count; i++)
            {
                RingStage item = this.stageList[i];

                // リストの先頭の要素が一番前に来るように調整
                item.InitDegree = (this.oneAngle * i) + 270.0f;
            }

            // 並び順用の整列用のキャッシュを作成
            this.stageListCache.AddRange(this.stageList);

            this.updateItemsPostion(); // 位置と大きさを決めるために1回だけ呼び出す
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) // Rotate left
            {
                this.count++;
                float endValue = this.count * this.oneAngle;

                this.enabled = true;

                // GCAlloc -> 1.2K
                var seq = DOTween.Sequence();
                seq.Append(DOTween.To(() => this.stepAmount, val => this.stepAmount = val, endValue, this.magnetSpeed));

            }
            if (Input.GetKeyDown(KeyCode.D)) // Rotate right
            {
                this.count--;
                float endValue = this.count * this.oneAngle;

                this.enabled = true;

                // GCAlloc -> 1.2K
                var seq = DOTween.Sequence();
                seq.Append(DOTween.To(() => this.stepAmount, val => this.stepAmount = val, endValue, this.magnetSpeed));
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                IrisOut();
            }

            if(fIris == true)
            {
                LoadStage(frontStage);
            }
            this.updateItemsPostion();
        }
        private void updateItemsPostion()
        {
            RingStage tempFrontStage = null;
            float closestDegree = 360f;

            foreach (RingStage item in this.stageList)
            {
                if (item == null)
                {
                    Debug.LogWarning("要素がnullです。");
                    continue;
                }

                float deg = (item.InitDegree + this.stepAmount) % 360.0f;
                float _z = Mathf.Abs(deg - 270.0f);
                if (_z > 180.0f)
                {
                    _z = Mathf.Abs(360.0f - _z); // 180が一番うしろ
                }
                item.Rect.SetAnchoredPosZ(_z);

                // 一番後ろが指定した大きさになるように大きさを変更
                item.Rect.SetLocalScaleXY(Mathf.Lerp(this.backZoomScale, 1.0f, 1.0f - Mathf.InverseLerp(0, 180.0f, _z)));

                var (x, y) = MathfUtil.GetPosDeg(deg);
                item.Rect.SetAnchoredPos(x * this.ringWidth, y * this.ringHeight);

                // 最前面の要素を判定
                if (_z < closestDegree)
                {
                    closestDegree = _z;
                    tempFrontStage = item;
                }
            }

            // 計算したZ位置からuGUIの前後関係を設定する
            this.stageListCache.Sort(this.sort);
            for (int i = 0; i < this.stageListCache.Count; i++)
            {
                this.stageListCache[i].Rect.SetSiblingIndex(i);
            }

            frontStage = tempFrontStage;
        }

        // 要素を整列するときに渡すラムダ用の処理
        private int sort(RingStage a, RingStage b)
        {
            float diff = b.Rect.GetAnchoredPosZ() - a.Rect.GetAnchoredPosZ();
            if (diff > 0)
            {
                return 1;
            }
            else if (diff < 0)
            {
                return -1;
            }
            return 0;
        }

        public void IrisOut()
        {
            // アイリスアウト（小さくして消える）とシーン遷移を開始
            StartCoroutine(IrisOutAndLoadScene());
        }

        private IEnumerator IrisOutAndLoadScene()
        {
            // アニメーションの完了を待つ
            yield return unmask.DOScale(Vector3.zero, SCALE_DURATION).SetEase(Ease.OutCubic).WaitForCompletion();

            fIris = true;
        }

        private void LoadStage(RingStage stage)
        {
            switch (stage.name)
            {
                case "Stage1":
                    Debug.Log("Stage1に遷移");
                    SceneManager.LoadScene(sceneToLoad1);
                    break;
                case "Stage2":
                    Debug.Log("Stage2に遷移");
                    SceneManager.LoadScene(sceneToLoad2);
                    break;
                case "Stage3":
                    Debug.Log("Stage3に遷移");
                    SceneManager.LoadScene(sceneToLoad3);
                    break;
                case "Stage4":
                    Debug.Log("Stage4に遷移");
                    SceneManager.LoadScene(sceneToLoad4);
                    break;
                case "Stage5":
                    Debug.Log("Stage5に遷移");
                    SceneManager.LoadScene(sceneToLoad5);
                    break;
                case "Stage6":
                    Debug.Log("Stage6に遷移");
                    SceneManager.LoadScene(sceneToLoad6);
                    break;
                case "Stage7":
                    Debug.Log("Stage7に遷移");
                    SceneManager.LoadScene(sceneToLoad7);
                    break;
                case "Stage8":
                    Debug.Log("Stage8に遷移");
                    SceneManager.LoadScene(sceneToLoad8);
                    break;
                case "Stage9":
                    Debug.Log("Stage9に遷移");
                    SceneManager.LoadScene(sceneToLoad9);
                    break;
            }
        }
    }

}
