using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossAreaLockCamera : MonoBehaviour
{
    //シネマシーンブレイン
    private CinemachineBrain _cinemachineBrain;
    //プレイヤー
    private Transform _player;

    private void Update()
    {
        //プレイヤーがいる場合
        if (_player != null)
        {
            //プレイヤーの位置を取得
            Vector3 playerPos = _player.position;
            //プレイヤーの位置をカメラの位置に設定
            transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z - 10);
        }
    }

    //プレイヤーがOnTriggerEnterでボスエリアに入ったときにカメラをロックする
    private void OnTriggerEnter(Collider other)
    {
        //Vcam_Bossオブジェクトを取得し、Priorityを100に設定
        if (other.CompareTag("Player"))
        {
            Debug.Log("BossAreaLockCamera");
            GameObject.Find("VCam_BossArea").GetComponent<CinemachineVirtualCamera>().Priority = 100;
            //VCamのTargetGroupにプレイヤーを追加
            //GameObject.Find("VCam_BossArea").GetComponent<CinemachineVirtualCamera>().Follow = other.transform;
            //ボスを追加
            GameObject.Find("VCam_BossArea").GetComponent<CinemachineVirtualCamera>().LookAt = other.transform;
            //GameObject.Find("VCam_BossArea").GetComponent<CinemachineVirtualCamera>().LookAt = GameObject.Find("Boss_Golem(Clone)").transform;

        }
    }

}
