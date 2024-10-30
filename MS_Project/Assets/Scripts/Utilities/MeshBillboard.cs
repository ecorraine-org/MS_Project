using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBillboard : MonoBehaviour
{
    [SerializeField]
    private EnemyAction enemy;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = this.GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Camera.main.transform.rotation;

        float normalizedValue = (float)(enemy.EnemyStatus.Health / enemy.EnemyStatus.StatusData.maxHealth);
        mat.SetFloat("_Value", normalizedValue);

        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        float distance = Vector3.Distance(playerPos, this.transform.position);
        if (distance > 10f)
        {
            mat.SetFloat("_Opacity", 0);
        }
        else
        {
            mat.SetFloat("_Opacity", 1);
        }
    }
}
