using UnityEngine;

public class ResultOpen : MonoBehaviour
{
    public GameObject prefab; // �C���X�y�N�^�[�Őݒ肷��v���n�u
    private GameObject instantiatedPrefab; // �������ꂽ�v���n�u���Ǘ�

    void Start()
    {
        // ������ԂŃv���n�u���A�N�e�B�u�ɂ��Ă����i�K�v�ɉ����āj
        if (prefab != null)
        {
            prefab.SetActive(false);
        }
    }

    void Update()
    {
        // 1�L�[��3�L�[�������������ꂽ�ꍇ�Ƀv���n�u��\��
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha3))
        {
            // �܂��v���n�u���\������Ă��Ȃ��ꍇ�̂ݏ���
            if (instantiatedPrefab == null)
            {
                OpenPrefab();
            }
        }
    }

    void OpenPrefab()
    {
        // prefab���C���X�y�N�^�[�Őݒ肳��Ă��邩�m�F
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned in the inspector!");
            return;
        }

        // �v���n�u���C���X�^���X�����ĕ\��
        instantiatedPrefab = Instantiate(prefab, transform.position, Quaternion.identity); // �v���n�u���V�[�����ɕ\��
        instantiatedPrefab.SetActive(true); // �C���X�^���X���A�N�e�B�u�ɂ���
        Debug.Log("Prefab opened!");
    }
}
