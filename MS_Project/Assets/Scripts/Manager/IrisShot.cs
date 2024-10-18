using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IrisShot : MonoBehaviour
{
    [SerializeField] RectTransform unmask;
    readonly Vector2 IRIS_IN_SCALE = new Vector2(30, 30);
    readonly float SCALE_DURATION = 2;
    [SerializeField] string sceneToLoad; // �؂�ւ���V�[�������w��

    private void Start()
    {
        // �V�[���J�n���ɃA�C���X�C��
        IrisIn();
    }

    public void IrisIn()
    {
        // �A�C���X�C���i�傫�����ĕ\���j
        unmask.localScale = Vector3.zero;  // ������Ԃŏ��������Ă���
        unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION).SetEase(Ease.InCubic);
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

        // �V�[���J��
        SceneManager.LoadScene(sceneToLoad);
    }

    private void Update()
    {
        // Enter�L�[�ŃA�C���X�A�E�g
        if (Input.GetKeyDown(KeyCode.Return))
        {
            IrisOut();
        }
    }
}