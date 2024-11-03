using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    [SerializeField, Header("スプライトレンダラー")]
    SpriteRenderer spriteRenderer;

    private float randomRotationZ;

    private void Awake()
    {
        //一番前に表示する
        Material material = spriteRenderer.material;
        material.SetFloat("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

        randomRotationZ = Random.Range(-30f, 30f);

        transform.rotation = Quaternion.Euler(0, 0, randomRotationZ);
    }

    private void Update()
    {
      //  transform.rotation = Camera.main.transform.rotation;

        transform.rotation = Quaternion.Euler(0, 0, randomRotationZ) * Camera.main.transform.rotation;



    }
}
