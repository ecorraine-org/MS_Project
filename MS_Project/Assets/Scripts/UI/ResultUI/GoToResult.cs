using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToResult : MonoBehaviour
{
    //�G�̎w��
    [SerializeField, Header("�w�肷��Enemy")]
    public GameObject EnemyObjct;

    //�ڍs����V�[���̎w��
    [SerializeField,Header("�w�肷��Scene")]
    string sceneToLoad;

    //�G��HP�Ǘ��p
    float EnemyObjctHP;

    private void Start()
    {

    }

    public void KillMeEnemy()
    {
        //�G��HP��0�ȉ��ɂȂ�����
        if(EnemyObjctHP <= 0)
        {
            //ResultScene�Ɉڍs����
            SceneManager.LoadScene(sceneToLoad);                    // �V�[�������[�h���ă��j���[�V�[���ɑJ��

        }
    }
}
