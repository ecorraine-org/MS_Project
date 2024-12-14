using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��
    public Image titlePanel;
    public Image continuePanel;

    bool Title;
    bool Continue;

    [SerializeField] string sceneToLoad; // �؂�ւ���V�[�������w��

    void Start()
    {
        fadePanel.enabled = false;       // �t�F�[�h�p�l���𖳌���
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // ������Ԃł͓���
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Continue = false;
            Title = true;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            Title = false;
            Continue = true;
        }

        if(Title)
        {
            titlePanel.color = new Color(255, 0, 0, 255);
        }
        if(Continue)
        {
            continuePanel.color = new Color(255, 0, 0, 255);
        }

        if ((!Title))
        {
            titlePanel.color = new Color(255, 255, 255, 255);
        }
        if(!Continue)
        {
            continuePanel.color = new Color(255, 255, 255, 255);
        }

        if(Title && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;   // �t�F�[�h�p�l����L����s

        float elapsedTime = 0.0f;                        // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color;              // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        fadePanel.color = endColor;                                // �t�F�[�h������������ŏI�F�ɐݒ�
        SceneManager.LoadScene(sceneToLoad);                    // �V�[�������[�h���ă��j���[�V�[���ɑJ��
    }
}
