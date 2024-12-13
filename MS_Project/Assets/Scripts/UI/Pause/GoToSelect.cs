using Stage.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static InputController;

public class GoToSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            SceneManager.LoadScene("StageSelect");

            Destroy(GameObject.Find("CameraPivot(Clone)"));
            InputController.Instance.SetInputContext(InputContext.UI);

        }
    }
}
