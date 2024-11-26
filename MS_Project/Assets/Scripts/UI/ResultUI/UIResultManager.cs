using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �V�[����J�ڂ��邽�߂ɕK�v
using TMPro; // TextMeshPro�p�̖��O���

public class UIResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // TextMeshProUGUI�R���|�[�l���g
    public Button restartButton; // ���X�^�[�g�{�^��

    void Start()
    {
        // �X�R�A��\��
        scoreText.text = "�X�R�A: " + GameManager.playerScore;

        // �{�^���Ƀ��X�i�[��ǉ��i�N���b�N�����Ƃ��̏����j
        restartButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        // �V�[���������[�h�i�Q�[���ɖ߂�j
        SceneManager.LoadScene("Title"); // �Q�[���V�[���̖��O�ɕύX
    }
}
