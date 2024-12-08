using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    [SerializeField, Header("フィニッシャーUI")]
    GameObject finishIcon;

    public GameObject FinishIcon
    {
        get => finishIcon;
    }
}
