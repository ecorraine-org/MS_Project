using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelManager : MonoBehaviour
{
    //[SerializeField, Header("元のマテリアル")]
    // Material initialMat;

    [SerializeField, Header("ディゾルブマテリアル")]
    Material dissolveMat;

    //[SerializeField, Header("トゥーンマテリアル")]
    //  Material toonMat;

    [SerializeField, Header("ディゾルブ持続時間")]
    float dissolveDuration=2.0f;

    // ディゾルブの進行状況（0から1）を示す
    float dissolveValue  = 0f; 

    SkinnedMeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
   
    }


    /// <summary>
    /// 死亡時の処理
    /// </summary>
    public void ChangeDeadMat()
    {
        // マテリアル取得
        Material[] materials = meshRenderer.materials;

        // 元のマテリアル→ディゾルブ
        materials[0] = dissolveMat;

        //// トゥーンマテリアル→無くす
        //materials[1] = null;


        // トゥーンマテリアル→削除
  
        Material[] newMaterials = new Material[1];
        newMaterials[0] = materials[0];  

        // 設定
        meshRenderer.materials = newMaterials;


        //コルーチン
        TimerUtility.FrameBasedTimer(this, dissolveDuration, () => DissovleUpdate(), () => Dead());
    }

    private void Dead()
    {
        Destroy(gameObject);
    }

    private void DissovleUpdate()
    {
              // float chargePerFrame = (1f / chargeTime) * Time.deltaTime;

    //    dissolveProgress +=    Time.deltaTime /  dissolveDuration;
      //  dissolveProgress = Mathf.Clamp01(dissolveProgress);

    //    float dissolveValue;

        if (dissolveDuration != 0)
        {
            // dissolveValue = dissolveProgress / dissolveDuration;
            dissolveValue += (1f / dissolveDuration) * Time.deltaTime;
        }
        //else
        //{
        //    //  dissolveValue = 0;
        //    dissolveValue  = 0;
        //}

        //一つ目はパラメター名?
        meshRenderer.materials[0].SetFloat("_Amcount", dissolveValue );
    }
}
