using UnityEngine;

public class MiniMapSwitching : MonoBehaviour
{
    [SerializeField] private GameObject[] group1; // �ŏ��̔z��
    [SerializeField] private GameObject[] group2; // 2�ڂ̔z��

    private bool isGroup1Active = true; // ���݂ǂ��炪�A�N�e�B�u�����L�^

    private void Update()
    {
        // M�L�[�������ꂽ��؂�ւ�
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleGroups();
        }
    }

    private void ToggleGroups()
    {
        // ���݂̏�Ԃɉ����Đ؂�ւ�
        isGroup1Active = !isGroup1Active;

        // �z��1�̃A�N�e�B�u��Ԃ�ݒ�
        foreach (GameObject obj in group1)
        {
            if (obj != null) obj.SetActive(isGroup1Active);
        }

        // �z��2�̃A�N�e�B�u��Ԃ�ݒ�
        foreach (GameObject obj in group2)
        {
            if (obj != null) obj.SetActive(!isGroup1Active);
        }
    }
}
