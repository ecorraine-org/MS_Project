using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    [SerializeField, Header("スプライトレンダラー")]
    SpriteRenderer spriteRenderer;

    [SerializeField, Header("アニメーター")]
    Animator animator;

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


        //アニメーション終了、消滅
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
