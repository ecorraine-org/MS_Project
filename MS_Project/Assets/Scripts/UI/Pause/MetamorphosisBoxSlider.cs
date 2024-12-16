using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetamorphosisBoxSlider : MonoBehaviour
{
    [SerializeField, Header("�ʒmBOX")]
    public RectTransform box; // ���삷��Panel��RectTransform
    [SerializeField, Header("�J�n�n�_")]
    public Vector2 offScreenPosition; // Panel����ʊO�ɂ���Ƃ��̍��W
    [SerializeField, Header("�I���n�_")]
    public Vector2 onScreenPosition;  // Panel����ʓ��ɂ���Ƃ��̍��W
    [SerializeField, Header("�X���C�h����")]
    public float slideDuration; // �X���C�h�A�j���[�V�����̏��v���ԁi�b�j

    [SerializeField, Header("���̑f��")]
    public GameObject swordbox;
    [SerializeField, Header("�n���}�[�̑f��")]
    public GameObject hammerdbox;
    [SerializeField, Header("���̑f��")]
    public GameObject spearbox;
    
    private bool isVisible = false; // box���\�������ǂ���
    private bool isSliding = false; // �X���C�h�����ǂ���

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
        // ������ϐg�������̕ϐ��ɂ��Ă�������
        if (Input.GetKeyDown(KeyCode.K))
        {
            TogglePanel();
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
}