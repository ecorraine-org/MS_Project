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
    public GameObject hammerbox;
    [SerializeField, Header("���̑f��")]
    public GameObject spearbox;
    
    private bool isVisible = false; // box���\�������ǂ���
    private bool isSliding = false; // �X���C�h�����ǂ���

    private void Start()
    {
        // �ʒmBOX�������ʒu�ɐݒ�
        if (box != null) 
        {
            box.anchoredPosition = offScreenPosition;
        }
    }
    private void Update()
    {
        // �ϐg�����Ƃ��Ɂ`����if���ɕύX
        if (Input.GetKeyDown(KeyCode.K))
        {
            //�ʒmBOX��\��
            TogglePanel();
        }

        // ����̐؂�ւ���������if���̒���ϐg�̕ϐ���
        if (Input.anyKeyDown)
        {
            // ���A�n���}�[�A���̐؂�ւ��p
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetWeapon("Sword");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetWeapon("Hammer");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetWeapon("Spear");
            }
        }
    }

    // �����؂�ւ���
    private void SetWeapon(string weaponType)
    {
        // ���ׂĂ̕���f�ނ��\��
        swordbox.SetActive(false);
        hammerbox.SetActive(false);
        spearbox.SetActive(false);

        //�\�����镐�������
        switch (weaponType)
        {
            case "Sword":
                swordbox.SetActive(true);
                Debug.Log("����I�����܂���");
                break;
            case "Hammer":
                hammerbox.SetActive(true);
                Debug.Log("�n���}�[��I�����܂���");
                break;
            case "Spear":
                spearbox.SetActive(true);
                Debug.Log("����I�����܂���");
                break;
            default:
                Debug.LogWarning("�����ȕ���^�C�v���w�肳��܂���: " + weaponType);
                break;
        }
    }

    //�X���C�h����֐�
    public void TogglePanel()
    {
        //if (isSliding) return; //�X���C�h���͑��삵�Ȃ�
        
        isSliding = true; //�X���C�h�����ǂ���
        
        //isVisible = true �Ȃ� offScreenPosition�܂ŃX���C�h�Afalse �Ȃ� onScreenPosition �܂ŃX���C�h
        Vector2 targetPosition = isVisible ? offScreenPosition : onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }
    //�X���C�h������֐�
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