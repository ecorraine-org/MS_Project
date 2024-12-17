using UnityEngine;
using PixelCrushers.SceneStreamer;

/// <summary>
/// リザルトシーンへの遷移を制御するクラス
/// Strategy パターンでシーン遷移の実装を切り替え可能に
/// </summary>
public class ResultTransitionHandler : MonoBehaviour
{
    [SerializeField] private string resultSceneName = "StageSelect";

    private void Start()
    {
        // ミッション完了イベントの購読
        MissionResultManager.Instance.OnMissionComplete += HandleMissionComplete;
    }

    private void OnDestroy()
    {
        // イベント購読の解除
        if (MissionResultManager.Instance != null)
        {
            MissionResultManager.Instance.OnMissionComplete -= HandleMissionComplete;
        }
    }

    private void HandleMissionComplete(MissionType missionType)
    {
        if (missionType == MissionType.KillBoss)
        {
            StartCoroutine(TransitionToResult());
        }
    }

    private System.Collections.IEnumerator TransitionToResult()
    {
        // スクリーンショットの処理を待つ
        Debug.Log("Transition to Result scene");
        yield return new WaitForSeconds(1.0f);

        //CameraPivot(Clone)を探sして削除
        Destroy
        (
            GameObject.Find("CameraPivot(Clone)")
        );
        // リザルトシーンへ遷移
        SceneStreamerManager.TransitionScene(resultSceneName, false);
        Debug.Log("Transition to Result scene");
    }
}