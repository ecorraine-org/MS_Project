using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EatBoxSlider;

public class EatBoxSlider : MonoBehaviour
{
    [SerializeField, Header("�ʒmBOX")]
    public RectTransform box; // ���삷��Panel��RectTransform
    [SerializeField, Header("�J�n�n�_")]
    public Vector2 offScreenPosition; // Panel����ʊO�ɂ���Ƃ��̍��W
    [SerializeField, Header("�I���n�_")]
    public Vector2 onScreenPosition;  // Panel����ʓ��ɂ���Ƃ��̍��W
    [SerializeField, Header("�X���C�h����")]
    public float slideDuration; // �X���C�h�A�j���[�V�����̏��v���ԁi�b�j

    [SerializeField, Header("�\�����Text�R���|�[�l���g")]
    private TextMeshProUGUI uiText;
    [SerializeField, Header("�ϐ��Ɋ�Â��e�L�X�g�̃}�b�s���O")]
    private TextMapping[] textMappings; // ����̕ϐ��Ɋ�Â��e�L�X�g���}�b�s���O
    private int currentIndex = 0; // ���ݕ\�����Ă���e�L�X�g�̃C���f�b�N�X

    private bool isVisible = false; // box���\�������ǂ���
    private bool isSliding = false; // �X���C�h�����ǂ���

    [System.Serializable]
    public class TextMapping
    {
        [SerializeField, Header("�ϐ���")]
        public string variableName;
        [SerializeField, Header("�Ή�����e�L�X�g")]
        public string text;
    }

private void Start()
    {
        // �����ʒu��ݒ�
        if (box != null)
        {
            box.anchoredPosition = offScreenPosition;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TogglePanel();
            ShowTextByVariable();
        }
        else
        {
            Debug.LogError("UI Text�R���|�[�l���g���ݒ肳��Ă��܂���B");
        }
    }
    //�X���C�h������֐�
    public void TogglePanel()
    {
        //if (isSliding) return; //�X���C�h���͑��삵�Ȃ�

        isSliding = true; //�X���C�h�����ǂ���

        //isVisible = true �Ȃ� offScreenPosition�܂ŃX���C�h�Afalse �Ȃ� onScreenPosition �܂ŃX���C�h
        Vector2 targetPosition = isVisible ? offScreenPosition : onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }

    private IEnumerator SlidePanel(Vector2 targetPosition)
    {
        float elapsedTime = 0f; //�A�j���[�V�����̌o�ߎ��Ԃ�ǐՂ��邽�߂̕ϐ�
        Vector2 startPosition = box.anchoredPosition; //�X���C�h�J�n���̃p�l���̌��݈ʒu��ێ�

        //�A�j���[�V�������I�����Ԃ܂Ń��[�v�����
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime; //�O�t���[�����玞�Ԍo�߂����Z
            float t = elapsedTime / slideDuration;
            box.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;

            // ���荞�݃`�F�b�N�i�K�v�Ȃ������ǉ��j
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("���荞�ݒʒm");
                //���������ʒu����
                box.anchoredPosition = offScreenPosition;
                isVisible = true;
            }
        }
        box.anchoredPosition = targetPosition; //�Ō�ɖڕW�ʒu�Ƀs�b�^�����킹��
        isVisible = !isVisible; //�^�U�l�̓���ւ�
        isSliding = false; //�X���C�h�A�j���[�V�����̏�����
    }

    // �O�����璼�ڃe�L�X�g��ݒ肷�郁�\�b�h
    public void SetText(string newText)
    {
        if (uiText != null)
        {
            uiText.text = newText;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI�R���|�[�l���g���ݒ肳��Ă��܂���B");
        }
    }
    // ����̕ϐ��Ɋ�Â��ăe�L�X�g��\�����郁�\�b�h
    public void ShowTextByVariable(string variable)
    {
        foreach (var mapping in textMappings)
        {
            if (mapping.variableName == variable)
            {
                SetText(mapping.text);
                return;
            }
        }
        Debug.LogWarning($"�w�肳�ꂽ�ϐ� '{variable}' �ɑΉ�����e�L�X�g��������܂���B");
    }
}