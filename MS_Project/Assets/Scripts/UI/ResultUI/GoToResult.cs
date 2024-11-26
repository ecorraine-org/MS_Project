using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToResult : MonoBehaviour
{
    //敵の指定
    [SerializeField, Header("指定するEnemy")]
    public GameObject EnemyObjct;

    //移行するシーンの指定
    [SerializeField,Header("指定するScene")]
    string sceneToLoad;

    //敵のHP管理用
    float EnemyObjctHP;

    private void Start()
    {

    }

    public void KillMeEnemy()
    {
        //敵のHPが0以下になったら
        if(EnemyObjctHP <= 0)
        {
            //ResultSceneに移行する
            SceneManager.LoadScene(sceneToLoad);                    // シーンをロードしてメニューシーンに遷移

        }
    }
}
