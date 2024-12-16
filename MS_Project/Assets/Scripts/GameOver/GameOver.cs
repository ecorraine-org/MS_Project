using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static InputController;

public class GameOver : MonoBehaviour
{
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��
    public Image titlePanel;
    public Image continuePanel;

    public Sprite pushTitle;
    public Sprite pushStage;

    public Sprite hoverTitle;
    public Sprite hoverStage;

    bool Title;
    bool Continue;

    [SerializeField] string LoadTitle; // �؂�ւ���V�[�������w��
    string LoadStage; // �v���C���[�����ꂽ���_�̃X�e�[�W

    private const string LastStageKey = "LastPlayedStage"; // �ۑ��p�̃L�[

    void Start()
    {
        fadePanel.enabled = false;       // �t�F�[�h�p�l���𖳌���
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // ������Ԃł͓���

        // �ۑ����ꂽ�X�e�[�W�����擾
        LoadStage = PlayerPrefs.GetString(LastStageKey, "DefaultStageName");
    }

    void Update()
    {
        if (UIInputManager.Instance.GetLeftTrigger())
        {
            Continue = false;
            Title = true;
        }
        if(UIInputManager.Instance.GetRightTrigger())
        {
            Title = false;
            Continue = true;
        }

        if(Title)
        {
            titlePanel.sprite = pushTitle;
        }
        if(Continue)
        {
            continuePanel.sprite = pushStage;
        }

        if ((!Title))
        {
            titlePanel.sprite = hoverTitle;
        }
        if(!Continue)
        {
            continuePanel.sprite = hoverStage;
        }

        if(Title && UIInputManager.Instance.GetEnterTrigger())
        {
            //���Ԍo�ߑ��x
            Time.timeScale = 1;
            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);
            StartCoroutine(FadeOutAndLoadTitle());
        }

        if(Continue && UIInputManager.Instance.GetEnterTrigger())
        {
            //���Ԍo�ߑ��x
            Time.timeScale = 1;
            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);
            StartCoroutine(FadeOutAndLoadStage());
        }
    }

    // �X�e�[�W�ւ̃��g���C
    public IEnumerator FadeOutAndLoadStage()
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
        SceneManager.LoadScene(LoadStage);                    // �V�[�������[�h���ă��j���[�V�[���ɑJ��
    }

    // �^�C�g���ւ̃t�F�[�h
     public IEnumerator FadeOutAndLoadTitle()
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
        SceneManager.LoadScene(LoadTitle);                    // �V�[�������[�h���ă��j���[�V�[���ɑJ��
    }
}
