using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChangeBox : MonoBehaviour
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

    float cunt;
    PlayerMode playermode; //�v���C���[�̏�Ԃ�ۑ�(����)
    private void OnEnable()
    {
        //�C�x���g���o�C���h����
        OnomatoManager.OnModeChangeEvent += ModeChange;
    }

    private void OnDisable()
    {
        //�o�C���h����������
        OnomatoManager.OnModeChangeEvent -= ModeChange;
    }
    //���[�h���ς�������Ƀe�L�X�g�{�b�N�X��\������
    private void ModeChange(PlayerMode _mode, string _name)
    {
        playermode = _mode; //���[�h�ݒ�
        Debug.Log("���[�h�`�F���W������" + playermode);
        Debug.Log("�I�m�}�g�y��H�ׂ���" + _name);
        playermode = BattleManager.Instance.CurPlayerMode; //���݂̏�Ԃ�ۑ�
        OnOff(); //�ʒmBOX��\��
        SlideIn(); //�ʒmbox���X���C�h
    }
    //--------------------------------�X���C�h����--------------------------------
    public void SlideIn()
    {
        if (isSliding) return; //�X���C�h���͑��삵�Ȃ�

        isSliding = true; //�X���C�h�����ǂ���

        //offScreenPosition ���� onScreenPosition �܂ŃX���C�h
        Vector2 targetPosition = onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }
    public void SlideOut()
    {
        if (isSliding) return; //�X���C�h���͑��삵�Ȃ�

        isSliding = true; //�X���C�h�����ǂ���

        //onScreenPosition ���� offScreenPosition �܂ŃX���C�h
        Vector2 targetPosition = offScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }
    //�X���C�h���鏈��
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
        }
        box.anchoredPosition = targetPosition; //�Ō�ɖڕW�ʒu�Ƀs�b�^�����킹��
        isSliding = false; //�X���C�h�A�j���[�V�����̏�����
    }

    //----------------------------------�\������----------------------------------
    void Start()
    {
        AllOff();
    }
    //4�b��ɏ����鏈��
    void Update()
    {
        if(isVisible == true)
        {
            cunt += Time.deltaTime;

            if (cunt >= 4)
            {
                SlideOut();
            }
        }
    }
    private void OnOff()
    {
        AllOff();

        // ���A�n���}�[�A���̐؂�ւ��p
        if (playermode == PlayerMode.Sword)
        {
            SetWeapon("Sword");
            swordbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (playermode == PlayerMode.Hammer)
        {
            SetWeapon("Hammer");
            hammerbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (playermode == PlayerMode.Spear)
        {
            SetWeapon("Spear");
            spearbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
    }
    private void SetWeapon(string weaponType)
    {
        //�m�F�p
        switch (weaponType)
        {
            case "Sword":
                Debug.Log("����I�����܂���");
                break;
            case "Hammer":
                Debug.Log("�n���}�[��I�����܂���");
                break;
            case "Spear":
                Debug.Log("����I�����܂���");
                break;
            default:
                Debug.LogWarning("�����ȕ���^�C�v���w�肳��܂���: " + weaponType);
                break;
        }
    }

    // ���ׂĂ̕���f�ނ��\��
    private void AllOff()
    {
        swordbox.SetActive(false);
        hammerbox.SetActive(false);
        spearbox.SetActive(false);
    }
}