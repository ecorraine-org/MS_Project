using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionResultManager : MonoBehaviour
{
    private static MissionResultManager instance;
    public static MissionResultManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("MissionResultManager");
                instance = obj.AddComponent<MissionResultManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public event Action<MissionType> OnMissionComplete;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NotifyMissionComplete(MissionType missionType)
    {
        OnMissionComplete?.Invoke(missionType);
    }
}
