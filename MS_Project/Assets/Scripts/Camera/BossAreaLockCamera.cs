using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossAreaLockCamera : MonoBehaviour
{
    //シネマシーンブレイン
    private CinemachineBrain _cinemachineBrain;

    private void Update()
    {
        //
    }

    //プレイヤーがOnTriggerEnterでボスエリアに入ったときにカメラをロックする
    private void OnTriggerEnter(Collider other)
    {
        //Vcam_Bossオブジェクトを取得し、Priorityを100に設定
        if (other.CompareTag("Player"))
        {
            Debug.Log("BossAreaLockCamera");
            GameObject.Find("VCam_BossArea").GetComponent<CinemachineVirtualCamera>().Priority = 100;
            //VCamのTargetGroupを設定する

        }
    }

}
