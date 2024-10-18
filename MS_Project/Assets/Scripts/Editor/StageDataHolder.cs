using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace StageEditor
{
    //ステージデータを保持するクラス
    [CreateAssetMenu]
    public class StageDataHolder : ScriptableObject
    {
        //ステージデータ
        public List<StageData> stageDataList;
    }

    [Serializable]
    public class StageData
    {
        public int id;
        public Vector2Int size;
        public List<StageUnitType> blockDataList;
    }

    [Serializable]
    public enum StageUnitType
    {
        None,
        Block,
        Player,
        Goal,
    }
}
