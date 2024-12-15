using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class OnomatoMissionManager : SingletonBaseBehavior<OnomatoMissionManager>
{
    //  Stage1Data
    [SerializeField, Header("Stage1オノマトペデータ")]
    StageOnomatoData stage1Data;

    //オノマトペ配列
    StageOnomatoData targetData;

    //集めたオノマトペを格納する配列
    [SerializeField, NonEditable, Header("集めたオノマトペを格納する配列")]
    List<OnomatopoeiaData> curData = new List<OnomatopoeiaData>();

    [SerializeField, NonEditable, Header("個数記録")]
    int count;

    [SerializeField, NonEditable, Header("総数")]
    int maxNum;

    [SerializeField, NonEditable, Header("ミッションコントローラー")]
    UIMissionController missionController;

    protected override void AwakeProcess()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "StartScene01")
        {
            //初期化
            if (targetData == null)
            {
                targetData = ScriptableObject.CreateInstance<StageOnomatoData>();
            }

            if (stage1Data.StageOnomatoList != null)
            {
                targetData.StageOnomatoList = new List<OnomatopoeiaData>(stage1Data.StageOnomatoList);
                maxNum = targetData.StageOnomatoList.Count();

                UpdateText();
            }
            else
            {
                Debug.LogWarning("stage1Data.StageOnomatoList is null");
            }

        }
    }

    private void UpdateText()
    {

        //UI取得
        if (missionController == null)
        {
            missionController = GameObject.FindGameObjectWithTag("Mission").gameObject.GetComponent<UIMissionController>();
        
        }

        if (missionController)
        {
            //表示更新
            missionController.OnomatoTxt.text = count.ToString() + "/" + maxNum.ToString();
        }


    }

    public void AddToCurData(OnomatopoeiaData _data)
    {
        //入ってないオノマトペ
        //ターゲットになる
        //を入れる
        if (!curData.Contains(_data) && targetData.StageOnomatoList.Contains(_data))
        {
            curData.Add(_data);
            count++;

            //表示更新
            UpdateText();




        }
    }

}
