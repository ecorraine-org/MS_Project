using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクル管理ビヘイビア
/// </summary>
public class ParticleManager : MonoBehaviour
{
    ParticleSystem particle;
   
    ParticleCompStartSize[] startSizeComps;


    private void Awake()
    {
        this.particle = this.GetComponent<ParticleSystem>();

        startSizeComps = GetComponentsInChildren<ParticleCompStartSize>();

    }

    private void Update()
    {
        //  if (this.particle.isPlaying) return;
        //  Destroy(this.gameObject);
    }

    public void SetLoop(bool isLooping)
    {
        // 現在のオブジェクトのParticleSystemコンポーネントを取得
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        // ParticleSystemが存在する場合、ループ設定を変更
        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.loop = isLooping;
        }
        else
        {
            // ParticleSystemが存在しない場合のエラーログ（必要に応じて有効化）
            // Debug.LogError("ParticleSystemコンポーネントが見つかりません。");
        }

        // 子オブジェクトの全てのParticleSystemコンポーネントを取得してループ設定を変更
        ParticleSystem[] childParticleSystems = GetComponentsInChildren<ParticleSystem>(true);
        foreach (var childParticleSystem in childParticleSystems)
        {
            // 現在のオブジェクトのParticleSystemはスキップ
            if (childParticleSystem != particleSystem)
            {
                var mainModule = childParticleSystem.main;
                mainModule.loop = isLooping;
            }
        }
    }


    public void SetStartSize(Vector2 size)
    {
        if (startSizeComps.Length == 0) return;

        foreach (var comp in startSizeComps)
        {    
            comp.SetStartSize(size);  
        }
    }

    /// <summary>
    /// 子オブジェクト含めて、サイズ変更
    /// </summary>
    public void ChangeScale(float _size)
    {
        transform.localScale *= _size;

        ChangeAllChildrenScale(transform, _size);
    }


    /// <summary>
    /// 再帰で子オブジェクトサイズ変更
    /// </summary>
    private void ChangeAllChildrenScale(Transform _parentTransform, float _size)
    {
        foreach (Transform childTransform in _parentTransform)
        {
            childTransform.localScale *= _size;

            ChangeAllChildrenScale(childTransform, _size);
        }
    }

    /// <summary>
    /// 子オブジェクト含めて、サイズ変更
    /// </summary>
    public void ChangeScale(Vector3 _size)
    {
        transform.localScale = Vector3.Scale(transform.localScale, _size);

        ChangeAllChildrenScale(transform, _size);
    }


    /// <summary>
    /// 再帰で子オブジェクトサイズ変更
    /// </summary>
    private void ChangeAllChildrenScale(Transform _parentTransform, Vector3 _size)
    {
        foreach (Transform childTransform in _parentTransform)
        {
            //childTransform.localScale = Vector3.Scale(transform.localScale, _size);
            childTransform.localScale = Vector3.Scale(childTransform.localScale, _size);


            ChangeAllChildrenScale(childTransform, _size);
        }
    }

    public void ChangePlaybackSpeed(float speedFactor)
    {
        // 現在のオブジェクトのParticleSystemコンポーネントを取得
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        // ParticleSystemが存在する場合、再生速度を変更
        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.simulationSpeed *= speedFactor;
        }
        else
        {
            // ParticleSystemが存在しない場合のエラーログ（必要に応じて有効化）
            // Debug.LogError("ParticleSystemコンポーネントが見つかりません。");
        }

        // 子オブジェクトの全てのParticleSystemコンポーネントを取得して再生速度を変更
        ParticleSystem[] childParticleSystems = GetComponentsInChildren<ParticleSystem>(true);
        foreach (var childParticleSystem in childParticleSystems)
        {
            // 現在のオブジェクトのParticleSystemはスキップ
            if (childParticleSystem != particleSystem)
            {
                var mainModule = childParticleSystem.main;
                mainModule.simulationSpeed *= speedFactor;
            }
        }
    }





    public ParticleCompStartSize[] StartSizeComps
    {
        get =>startSizeComps; 
    }

}

//public void ChangePlaybackSpeed(float speedFactor)
//{
//    // 粒子システムコンポーネントを取得
//    ParticleSystem particleSystem = GetComponent<ParticleSystem>();

//    // 粒子システムが存在するか確認
//    if (particleSystem != null)
//    {
//        // メインモジュールを取得
//        var mainModule = particleSystem.main;
//        mainModule.simulationSpeed *= speedFactor;  // 速度を変更
//    }
//    else
//    {
//        //Debug.LogError("ParticleSystemコンポーネントが見つかりません。");
//    }

//    // 子オブジェクトの粒子システムの再生速度も変更
//    ChangeAllChildrenPlaybackSpeed(transform, speedFactor);
//}

///// <summary>
///// 子オブジェクトの粒子システムの再生速度を変更する（再帰的に処理）
///// </summary>
//private void ChangeAllChildrenPlaybackSpeed(Transform parentTransform, float speedFactor)
//{
//    foreach (Transform childTransform in parentTransform)
//    {
//        // 子オブジェクトの粒子システムを取得
//        ParticleSystem childParticleSystem = childTransform.GetComponent<ParticleSystem>();

//        // 子オブジェクトに粒子システムが存在する場合
//        if (childParticleSystem != null)
//        {
//            var mainModule = childParticleSystem.main;
//            mainModule.simulationSpeed *= speedFactor;  // 速度を変更
//        }

//        // 子オブジェクトがさらに子オブジェクトを持っている場合、再帰的に処理
//        ChangeAllChildrenPlaybackSpeed(childTransform, speedFactor);
//    }
//}


// ParticleSystemRenderer particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
// if (particleRenderer == null) Debug.Log("particleRenderer NUll");
// else Debug.Log("particleRenderer exist " + particleRenderer);
//// 一番前に表示する
// Material material = particleRenderer.material;
// material.SetFloat("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
// if (material == null) Debug.Log("material NUll");
// else Debug.Log("material exist " + material);

//if (particle == null) return;
//ParticleSystemRenderer[] renderers = particle.GetComponentsInChildren<ParticleSystemRenderer>();  // true 允许获取所有子物体（包括非激活物体）
//if (renderers == null) Debug.Log("renderers NUll");
//else Debug.Log("renderers exist " + renderers);

//foreach (Renderer particleRenderer in renderers)
//{
//    Material material = particleRenderer.material;
//    if (material == null) Debug.Log("material NUll");
//    else Debug.Log("material exist " + material);

//    if (material != null)
//    {

//        material.SetFloat("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

//        material.renderQueue += 4000;

//        particleRenderer.sortingOrder = 100;
//        particleRenderer.sortingLayerID = SortingLayer.NameToID("Foreground");
//        Debug.Log($"Material modified for: {particleRenderer.name}");
//    }
//    else
//    {
//        Debug.LogWarning($"No material found on: {particleRenderer.name}");
//    }
//}