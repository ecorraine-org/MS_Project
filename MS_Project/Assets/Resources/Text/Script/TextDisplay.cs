using UnityEngine;
using UnityEngine.UI;  // Text (Legacy)���g�p���邽�߂ɕK�v
using System.Collections;  // IEnumerator���g�p���邽�߂ɕK�v

public class TextDisplay : MonoBehaviour
{
    [SerializeField, TextArea] private string message = "����ɂ��́AUnity�I";  // �C���X�y�N�^�[�ŕ����s�̓��͂��\�ɂ���
    [SerializeField] private Color textColor = Color.white;  // �C���X�y�N�^�[�ŐF��ݒ�\
    [SerializeField] private int fontSize = 30;  // �����̃T�C�Y��ݒ肷��t�B�[���h
    public Text uiText;  // Text�R���|�[�l���g�ւ̎Q��
    public float typingSpeed = 0.1f;  // �����̕\�����x

    void Start()
    {
        // Text�R���|�[�l���g�̐F�ƃt�H���g�T�C�Y��ݒ�
        uiText.color = textColor;
        uiText.fontSize = fontSize;

        // �����T�C�Y�����𖳌��ɂ���
        uiText.resizeTextForBestFit = false;
        uiText.resizeTextMinSize = fontSize;
        uiText.resizeTextMaxSize = fontSize;

        // �R���[�`�����Ăяo���ĕ������ꕶ�����\��
        StartCoroutine(TypeText());
    }

    // �������ꕶ�����\������R���[�`��
    private IEnumerator TypeText()
    {
        uiText.text = "";  // �ŏ��͋�ɂ���

        foreach (char letter in message)
        {
            uiText.text += letter;  // �ꕶ�����ǉ�
            yield return new WaitForSeconds(typingSpeed);  // �^�C�s���O���x�𐧌�
        }
    }
}