using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public List<RectTransform> icons; // ステージアイコンのリスト
    public float radius = 600f;       // リングの半径
    public float depth = 100f;        // 奥行きの深さ
    public float rotationDuration = 0.3f; // 回転にかける時間
    public float backZoomScale = 0.5f; // 背面アイコンの縮小率
    private int currentIndex = 0;     // 現在選択中のアイコンのインデックス
    private bool isRotating = false;  // 回転中の状態管理

    void Start()
    {
        ArrangeIconsInCylinder();
    }

    void Update()
    {
        // 回転の入力検知
        if (Input.GetKeyDown(KeyCode.A) && !isRotating)
        {
            StartCoroutine(RotateRing(1)); // 右回転
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isRotating)
        {
            StartCoroutine(RotateRing(-1)); // 左回転
        }
    }

    // アイコンを円筒状に配置する
    void ArrangeIconsInCylinder()
    {
        int iconCount = icons.Count;

        for (int i = 0; i < iconCount; i++)
        {
            // 中央アイコンは currentIndex になるように、残りを順番に配置
            float angle = ((i - currentIndex) * Mathf.PI * 2 / iconCount);
            Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Sin(angle) * depth, Mathf.Cos(angle) * radius);
            icons[i].localPosition = position;

            // 位置に応じたスケール調整
            float scale = Mathf.Lerp(1f, backZoomScale, Mathf.Abs(Mathf.Cos(angle)));
            icons[i].localScale = Vector3.one * scale;
        }

        // 中央のアイコンのスケールを強調
        icons[currentIndex].localScale = Vector3.one;
    }

    // アイコンリングを回転させる
    IEnumerator RotateRing(int direction)
    {
        isRotating = true;
        float elapsedTime = 0f;
        int iconCount = icons.Count;

        // 回転後の新しいインデックスを計算
        int newCurrentIndex = (currentIndex + direction + iconCount) % iconCount;

        // 回転アニメーション
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);

            for (int i = 0; i < iconCount; i++)
            {
                // 新しい位置の計算
                float startAngle = ((i - currentIndex) * Mathf.PI * 2 / iconCount);
                float endAngle = ((i - newCurrentIndex) * Mathf.PI * 2 / iconCount);
                float angle = Mathf.Lerp(startAngle, endAngle, t);

                Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Sin(angle) * depth, Mathf.Cos(angle) * radius);
                icons[i].localPosition = position;

                // 各アイコンのスケールを調整
                float scale = Mathf.Lerp(1f, backZoomScale, Mathf.Abs(Mathf.Cos(angle)));
                icons[i].localScale = Vector3.one * scale;
            }

            yield return null;
        }

        // 回転が完了した後、インデックスを更新
        currentIndex = newCurrentIndex;
        ArrangeIconsInCylinder(); // アイコン位置の更新
        isRotating = false;
    }
}
