using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Stage.Utility
{
    public class StageSelect : MonoBehaviour
    {
        [SerializeField] private List<RingStage> stageList = new List<RingStage>();

        [Header("リングの横、縦、移動時間、後ろの縮小率、縮小時の横幅")]
        // リングの横幅
        [SerializeField] private float ringWidth = 600;
        // リングの縦幅
        [SerializeField] private float ringHeight = 100;
        // 移動のインターバル
        [SerializeField] private float magnetSpeed = 0.18f;
        // 要素が一番後ろに移動したときの縮小率(0.5 = 半分の大きさ)
        [SerializeField] private float backZoomScale = 0.5f;

        [SerializeField] private float originWidth = 600;

        [Header("拡大後の横、アニメーション時間")]
        [SerializeField] private float expandedRingWidth = 3000; // 広げた後の横幅
        [SerializeField] private float expandDuration = 0.1f; // 広げるアニメーションの時間

        [Header("エンターキー入力後の拡大率")]
        [SerializeField] private float ZoomScaleX;
        [SerializeField] private float ZoomScaleY;
        [SerializeField] private float ZoomScaleZ;

        // 左右の回転量
        private float stepAmount;
        // 要素の間隔・角度
        private float oneAngle;
        // 目標位置 -> 回転させた回数(右+1, 左-1)
        private int count;
        // リングの前後関係整列用のバッファー
        private List<RingStage> stageListCache = new List<RingStage>();

        // アニメーション関係
        private Sequence rotation;
        private Tween zoomTween;

        // 最前面の要素
        public RingStage frontStage;

        bool fIris = false; // アイリスアウトのアニメーションフラグ
        bool fZoomIn = false;    // 拡大フラグ
        bool fZoomOut = false;   // 縮小フラグ
        bool flag = false;  // シーン遷移用のフラグ 

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
            // リストが空の場合のみ初期化
            if (this.stageListCache.Count == 0)
            {
                this.oneAngle = 360.0f / this.stageList.Count;
                for (int i = 0; i < this.stageList.Count; i++)
                {
                    RingStage item = this.stageList[i];

                    // ステージにロックをかける
                    item.isLocked = true;

                    // リストの先頭の要素が一番前に来るように調整
                    item.InitDegree = (this.oneAngle * i) + 270.0f;

                    item.Rect.localScale = new Vector3(1, 1, 1); // スケールをリセット
                }

                // 最初のステージのロックを解除する
                stageList[0].isLocked = false;

                // 並び順用の整列用のキャッシュを作成
                this.stageListCache.AddRange(this.stageList);
            }

            // 初期値のリセット
            this.stepAmount = 0;
            this.count = 0;

            this.updateItemsPostion();
        }

        void Update()
        {
            if (UIInputManager.Instance.GetLeftTrigger() && !fZoomIn) // Rotate left
            //    if (Input.GetKeyDown(KeyCode.A) && !fZoomIn) // Rotate left
                {
                this.count++;
                float endValue = this.count * this.oneAngle;

                if (rotation != null && rotation.IsActive())
                {
                    rotation.Kill();
                }

                rotation = DOTween.Sequence();
                rotation.Append(DOTween.To(() => this.stepAmount, val => this.stepAmount = val, endValue, this.magnetSpeed));

            }
            if (UIInputManager.Instance.GetRightTrigger() && !fZoomIn) // Rotate right
                //if (Input.GetKeyDown(KeyCode.D) && !fZoomIn) // Rotate right
            {
                this.count--;
                float endValue = this.count * this.oneAngle;

                if (rotation != null && rotation.IsActive())
                {
                    rotation.Kill();
                }

                rotation = DOTween.Sequence();
                rotation.Append(DOTween.To(() => this.stepAmount, val => this.stepAmount = val, endValue, this.magnetSpeed));
            }

            if (UIInputManager.Instance.GetEnterTrigger())
                //if (Input.GetKeyDown(KeyCode.Return))
            {
                fZoomIn = true;
            }

            if(fZoomIn == true)
            {
                DOTween.To(() => this.ringWidth, value => this.ringWidth = value, expandedRingWidth, expandDuration).SetEase(Ease.OutCubic);

                //// 最前面のステージを拡大し、その拡大状態を永続にする
                //frontStage.Rect.DOScale(new Vector3(ZoomScaleX, ZoomScaleY, ZoomScaleZ), expandDuration).OnComplete(() =>
                //{
                //    // アニメーションが完了した後に拡大したスケールを永続的に適用
                //    frontStage.Rect.localScale = new Vector3(ZoomScaleX, ZoomScaleY, ZoomScaleZ);

                //    flag = true;
                //});

                transform.DOScale(new Vector3(ZoomScaleX, ZoomScaleY, ZoomScaleZ), expandDuration).OnComplete(() =>
                {
                    flag = true;
                });
            }

            if (fIris == true)
            {
                LoadStage(frontStage);
            }

      
            if (flag == true && UIInputManager.Instance.GetEnterTrigger() && !frontStage.isLocked)
                //    if (flag == true && Input.GetKeyDown(KeyCode.Return))
            {
                IrisOut();
            }

            if (flag == true && UIInputManager.Instance.GetCancelTrigger())
             //   if (flag == true && Input.GetKeyDown(KeyCode.Escape))
            {
                fZoomOut = true;

                fZoomIn = false;
            }

            if (fZoomOut == true)
            {
                //// 最前面のステージを縮小し、その縮小状態を永続にする
                //frontStage.Rect.DOScale(new Vector3(1, 1, 1), expandDuration).OnComplete(() => {
                //    // アニメーションが完了した後に縮小したスケールを永続的に適用
                //    frontStage.Rect.localScale = new Vector3(1, 1, 1);

                //    // flag をリセット
                //    flag = false;
                //});

                transform.DOScale(new Vector3(1, 1, 1), expandDuration).OnComplete(() => {

                        // flag をリセット
                        flag = false;
                 });

                    // リングのサイズを元に戻す
                    DOTween.To(() => this.ringWidth, value => this.ringWidth = value, originWidth, expandDuration).SetEase(Ease.OutCubic);

                // fZoomOut フラグをリセット
                fZoomOut = false;
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

        void OnDestroy()
        {
            DOTween.KillAll();
        }
    }

}
