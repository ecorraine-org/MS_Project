using UnityEngine;
using UnityEngine.UI;

public class TitleButtonAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    private Button button;

    private void Start()
    {
        // �{�^���R���|�[�l���g�̎擾
        button = GetComponent<Button>();

        // AudioSource���ݒ肳��Ă��Ȃ��ꍇ�͎����I�ɒǉ�
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �{�^���N���b�N���̃C�x���g��SE�Đ�������ǉ�
        button.onClick.AddListener(PlayButtonSound);
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null && audioSource != null)
        {
            // �Đ������I�[�f�B�I�N���b�v�����f�o�b�O���O�ɕ\��
            Debug.Log($"�Đ����̉�: {buttonSound.name}");

            audioSource.PlayOneShot(buttonSound);
        }
        else
        {
            Debug.LogWarning("AudioSource �܂��� AudioClip ���ݒ肳��Ă��܂���B");
        }
    }
}
