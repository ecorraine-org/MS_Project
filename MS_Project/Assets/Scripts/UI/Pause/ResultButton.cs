using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultButton : MonoBehaviour
{
    // �{�^���̎Q��
    [SerializeField, Header("���̃X�e�[�W�֍s���{�^��")]
    public Button NextStageButton;
    [SerializeField, Header("�Z���N�g�X�e�[�W�֍s���{�^��")]
    public Button GoToSelectButton;

    // �{�^�����X�g
    private List<Button> buttons;
    private int selectedIndex = 0; // ���ݑI������Ă���{�^���̃C���f�b�N�X

    void Start()
    {
        // �{�^�������X�g�ɒǉ�
        buttons = new List<Button> { NextStageButton, GoToSelectButton };

        // �ŏ��Ƀ{�^����I��
        UpdateButtonSelection();

        Time.timeScale = 0;
    }
    void Update()
    {
        // W�L�[�Ŏ��̃{�^���ֈړ�
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveSelection(-1); // �O�̃{�^���Ɉړ�
        }

        // S�L�[�őO�̃{�^���ֈړ�
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveSelection(1); // ���̃{�^���Ɉړ�
        }

        // Enter�L�[�őI�����ꂽ�{�^�������s
        if (Input.GetKeyDown(KeyCode.Return)) // Enter�L�[�Ń{�^����I��
        {
            ExecuteSelectedButton();
        }

        // �I�����ꂽ�{�^���Ɏ��o�I�ȕύX��K�p
        UpdateButtonSelection();
    }

    // �{�^���I���̃C���f�b�N�X��ύX
    void MoveSelection(int direction)
    {
        selectedIndex += direction;

        // �͈͂𒴂��Ȃ��悤�ɒ���
        if (selectedIndex < 0)
        {
            selectedIndex = buttons.Count - 1; // �Ō�̃{�^���Ɉړ�
        }
        else if (selectedIndex >= buttons.Count)
        {
            selectedIndex = 0; // �ŏ��̃{�^���Ɉړ�
        }

        // �I�����ꂽ�{�^���̍X�V
        UpdateButtonSelection();
    }

    // �I�����ꂽ�{�^���̃X�^�C�����X�V
    void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == selectedIndex)
            {
                // ���ݑI�����ꂽ�{�^���Ƀn�C���C�g�i�F��ς���Ȃǂ̏����j
                buttons[i].Select(); // �{�^����I����Ԃ�
            }
            else
            {
                // ��I����Ԃ̏����i�K�v�Ȃ�F��߂����j
            }
        }
    }

    // �I�����ꂽ�{�^�������s����
    void ExecuteSelectedButton()
    {
        // �I�����ꂽ�{�^�������s
        buttons[selectedIndex].onClick.Invoke();
        Time.timeScale = 1;
    }
}
