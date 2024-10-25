using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneChange : MonoBehaviour
{
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��
    private bool isFading = false;      // �t�F�[�h�����ǂ����𔻒�
    public GameObject buttonObject;     // �{�^����GameObject���Q�Ƃ���ϐ�
    private Button button;              // Button�R���|�[�l���g�̎Q��
    public Sprite pushButton;           // �{�^���������ꂽ�Ƃ��̃X�v���C�g
    private Sprite originalSprite;      // ���̃{�^���̃X�v���C�g

    [SerializeField] string sceneToLoad; // �؂�ւ���V�[�������w��

    private void Start()
    {
        fadePanel.enabled = false;       // �t�F�[�h�p�l���𖳌���
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // ������Ԃł͓���

        // Button�R���|�[�l���g���擾
        button = buttonObject.GetComponent<Button>();
        originalSprite = button.image.sprite; // ���̃X�v���C�g��ۑ�
    }

    private void Update()
    {
        // Enter�L�[��������Ă���ԁA�{�^���̃X�v���C�g��ύX
        if (Input.GetKey(KeyCode.Return) && !isFading)
        {
            button.image.sprite = pushButton; // �{�^���̃X�v���C�g��ύX

            // Enter�L�[�������ꂽ��t�F�[�h�A�E�g���J�n
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(FadeOutAndLoadScene());
            }
        }
        else if (Input.GetKeyUp(KeyCode.Return)) // Enter�L�[�������ꂽ�Ƃ�
        {
            button.image.sprite = originalSprite; // ���̃X�v���C�g�ɖ߂�
        }
    }

    // �{�^���p�̃��\�b�h
    public void OnButtonClick()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;   // �t�F�[�h�p�l����L����

        isFading = true;                                 // �t�F�[�h���̃t���O�𗧂Ă�

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