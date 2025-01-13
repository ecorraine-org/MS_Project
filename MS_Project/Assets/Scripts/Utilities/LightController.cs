using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private static LightController instance;
    public static LightController Instance => instance;

    private GameObject sceneLight;

    void Awake()
    {
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        */
        sceneLight = this.gameObject;

        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }

    public GameObject SceneLight
    {
        get => sceneLight;
        set { sceneLight = value; }
    }
}
