using UnityEngine;

public class CameraBossEnemy : MonoBehaviour
{
    public Camera targetCamera;         // �Ώۂ̃J����
    public string targetLayerName = ""; // �ʂ��������C���[��

    void Start()
    {
        
        // ���C���[�}�X�N���擾
        int layerMask = LayerMask.GetMask(targetLayerName);

        if (layerMask == 0)
        {
            //
            return;
        }

        // �J������Culling Mask��ݒ�
        targetCamera.cullingMask = layerMask;
    }
}
