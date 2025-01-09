using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTime : MonoBehaviour
{
    float Gametime;

    [SerializeField, Header("ŽžŠÔ‚ð•\Ž¦‚·‚étext")]
    Text Timetext;

    // Start is called before the first frame update
    void Start()
    {
        Gametime = GameStatsManager.Instance.gameTime;

        TimeSpan time = TimeSpan.FromSeconds(Gametime);

        string timeString = string.Format("{0:D2}/{1:D2}",
                                          (int)time.TotalMinutes,
                                          time.Seconds);

        Timetext.text = timeString;
    }

    // Update is called once per frame
    void Update()
    {
  
    }
}
