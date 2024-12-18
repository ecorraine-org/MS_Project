using UnityEngine;

public class Key : MonoBehaviour
{
    // �f�o�b�N�p�L�[���͕ω�

    // �A�N�e�B�u�ɂ������I�u�W�F�N�g��Inspector����ݒ�
    public GameObject targetObject;

    // �g�p����L�[��public�ɂ���Inspector�ŕύX�ł���悤�ɂ���B�m�[�}����M�L�[
    public KeyCode activationKey = KeyCode.M;

    void Update()
    {
        // �w�肳�ꂽ�L�[�������ꂽ�Ƃ�
        if (Input.GetKeyDown(activationKey))
        {
            // �w�肳�ꂽ�I�u�W�F�N�g���A�N�e�B�u�ɂ���
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                //
                Debug.Log($"{activationKey}�L�[��������A{targetObject.name}���A�N�e�B�u��");
            }
            else
            {
                Debug.LogWarning("targetObject�����݂��Ȃ��A�������͐ݒ肵�Ă��Ȃ�");
            }
        }
    }
}
