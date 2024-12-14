using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス検出器
/// BossIDがないため、ボスを検出するためのコンポーネント
/// </summary>
public class BossDetector : MonoBehaviour
{
    [SerializeField] private CameraEffectManager _cameraManager;
    private GameObject _currentBoss;
    private bool _isMonitoring = false;
    private float _checkInterval = 0.5f;
    private float _nextCheckTime = 0f;
    private bool _waitingForDeath = false;
    private Vector3 _lastKnownPosition;

    private void FixedUpdate()
    {
        if (Time.time < _nextCheckTime) return;
        _nextCheckTime = Time.time + _checkInterval;

        // ボスを検出
        var possibleBoss = GameObject.Find("Boss_Golem");


        if (possibleBoss != null && !_isMonitoring)
        {
            _currentBoss = possibleBoss;
            _isMonitoring = true;
            _waitingForDeath = true;
            _lastKnownPosition = _currentBoss.transform.position;
            Debug.Log("Boss detected: " + _currentBoss.name);
        }
        else if (_waitingForDeath && possibleBoss == null)
        {
            Debug.Log("Boss death detected at position: " + _lastKnownPosition);
            HandleBossDeath();
            //タイムスケールを一時的に遅くする
            //StartCoroutine(SlowTimeForBossDeath());
            _waitingForDeath = false;
            _isMonitoring = false;
        }
    }

    private void HandleBossDeath()
    {
        if (CameraEffectManager.Instance == null)
        {
            var managerObject = new GameObject("CameraEffectManager");
            managerObject.AddComponent<CameraEffectManager>();
        }

        var deathEffect = new BossDeathCameraCommand(_lastKnownPosition);
        CameraEffectManager.Instance.AddEffect(deathEffect);
    }

    //ボス死亡時に一時的に時間を遅らせるコルーチン
    public IEnumerator SlowTimeForBossDeath()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
    }

}