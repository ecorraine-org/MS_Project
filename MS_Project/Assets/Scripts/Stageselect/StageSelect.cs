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
        // �����O�̉���
        [SerializeField] private float ringWidth;
        // �����O�̏c��
        [SerializeField] private float ringHeight;
        // �ړ��̃C���^�[�o��
        [SerializeField] private float magnetSpeed = 0.18f;
        // �v�f����Ԍ��Ɉړ������Ƃ��̏k����(0.5 = �����̑傫��)
        [SerializeField] private float backZoomScale = 0.5f;

        // ���E�̉�]��
        private float stepAmount;
        // �v�f�̊Ԋu�E�p�x
        private float oneAngle;
        // �ڕW�ʒu -> ��]��������(�E+1, ��-1)
        private int count;
        // �����O�̑O��֌W����p�̃o�b�t�@�[
        private List<RingStage> stageListCache = new List<RingStage>();

        // �őO�ʂ̗v�f
        public RingStage frontStage;

        bool fIris = false; // �A�C���X�A�E�g�̃A�j���[�V�����t���O

        [SerializeField] RectTransform unmask;
        //readonly Vector2 IRIS_IN_SCALE = new Vector2(50, 50);
        readonly float SCALE_DURATION = 2;
        [Header("�X�e�[�W�P�̖��O")]
        [SerializeField] string sceneToLoad1;
        [Header("�X�e�[�W�Q�̖��O")]
        [SerializeField] string sceneToLoad2;
        [Header("�X�e�[�W�R�̖��O")]
        [SerializeField] string sceneToLoad3;
        [Header("�X�e�[�W�S�̖��O")]
        [SerializeField] string sceneToLoad4;
        [Header("�X�e�[�W�T�̖��O")]
        [SerializeField] string sceneToLoad5;
        [Header("�X�e�[�W�U�̖��O")]
        [SerializeField] string sceneToLoad6;
        [Header("�X�e�[�W�V�̖��O")]
        [SerializeField] string sceneToLoad7;
        [Header("�X�e�[�W�W�̖��O")]
        [SerializeField] string sceneToLoad8;
        [Header("�X�e�[�W�X�̖��O")]
        [SerializeField] string sceneToLoad9;

        void Start()
        {
            // �����Ă�v�f���ɉ����ď����ʒu���v�Z����
            this.oneAngle = 360.0f / this.stageList.Count;
            for (int i = 0; i < this.stageList.Count; i++)
            {
                RingStage item = this.stageList[i];

                // ���X�g�̐擪�̗v�f����ԑO�ɗ���悤�ɒ���
                item.InitDegree = (this.oneAngle * i) + 270.0f;
            }

            // ���я��p�̐���p�̃L���b�V�����쐬
            this.stageListCache.AddRange(this.stageList);

            this.updateItemsPostion(); // �ʒu�Ƒ傫�������߂邽�߂�1�񂾂��Ăяo��
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
                    Debug.LogWarning("�v�f��null�ł��B");
                    continue;
                }

                float deg = (item.InitDegree + this.stepAmount) % 360.0f;
                float _z = Mathf.Abs(deg - 270.0f);
                if (_z > 180.0f)
                {
                    _z = Mathf.Abs(360.0f - _z); // 180����Ԃ�����
                }
                item.Rect.SetAnchoredPosZ(_z);

                // ��Ԍ�낪�w�肵���傫���ɂȂ�悤�ɑ傫����ύX
                item.Rect.SetLocalScaleXY(Mathf.Lerp(this.backZoomScale, 1.0f, 1.0f - Mathf.InverseLerp(0, 180.0f, _z)));

                var (x, y) = MathfUtil.GetPosDeg(deg);
                item.Rect.SetAnchoredPos(x * this.ringWidth, y * this.ringHeight);

                // �őO�ʂ̗v�f�𔻒�
                if (_z < closestDegree)
                {
                    closestDegree = _z;
                    tempFrontStage = item;
                }
            }

            // �v�Z����Z�ʒu����uGUI�̑O��֌W��ݒ肷��
            this.stageListCache.Sort(this.sort);
            for (int i = 0; i < this.stageListCache.Count; i++)
            {
                this.stageListCache[i].Rect.SetSiblingIndex(i);
            }

            frontStage = tempFrontStage;
        }

        // �v�f�𐮗񂷂�Ƃ��ɓn�������_�p�̏���
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
            // �A�C���X�A�E�g�i���������ď�����j�ƃV�[���J�ڂ��J�n
            StartCoroutine(IrisOutAndLoadScene());
        }

        private IEnumerator IrisOutAndLoadScene()
        {
            // �A�j���[�V�����̊�����҂�
            yield return unmask.DOScale(Vector3.zero, SCALE_DURATION).SetEase(Ease.OutCubic).WaitForCompletion();

            fIris = true;
        }

        private void LoadStage(RingStage stage)
        {
            switch (stage.name)
            {
                case "Stage1":
                    Debug.Log("Stage1�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad1);
                    break;
                case "Stage2":
                    Debug.Log("Stage2�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad2);
                    break;
                case "Stage3":
                    Debug.Log("Stage3�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad3);
                    break;
                case "Stage4":
                    Debug.Log("Stage4�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad4);
                    break;
                case "Stage5":
                    Debug.Log("Stage5�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad5);
                    break;
                case "Stage6":
                    Debug.Log("Stage6�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad6);
                    break;
                case "Stage7":
                    Debug.Log("Stage7�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad7);
                    break;
                case "Stage8":
                    Debug.Log("Stage8�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad8);
                    break;
                case "Stage9":
                    Debug.Log("Stage9�ɑJ��");
                    SceneManager.LoadScene(sceneToLoad9);
                    break;
            }
        }
    }

}
