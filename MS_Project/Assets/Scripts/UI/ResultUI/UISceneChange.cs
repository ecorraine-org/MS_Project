using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISceneChange : MonoBehaviour
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
        if (buttonObject == null)
        {
            Debug.LogError("buttonObject���ݒ肳��Ă��܂���B�C���X�y�N�^�[�Őݒ肵�Ă��������B");
            return;
        }

        button = buttonObject.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("buttonObject��Button�R���|�[�l���g���A�^�b�`����Ă��܂���B�������I�u�W�F�N�g��ݒ肵�Ă��������B");
            return;
        }

        originalSprite = button.image.sprite; // ���̃X�v���C�g��ۑ�
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
