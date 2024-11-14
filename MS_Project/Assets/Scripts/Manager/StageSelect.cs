using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public List<RectTransform> icons; // UIアイコンのリスト
    public float radius = 200f;       // リングの半径
    public float depth = 50f;         // 奥行きの深さ
    public float rotationSpeed = 100f; // 回転のスピード
    private int currentIndex = 0;     // 中央に表示されるアイコンのインデックス

    void Start()
    {
        ArrangeIconsInCylinder();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateLeft();
        }
        HighlightCenterIcon();
    }

    // アイコンを円筒形に配置する関数
    void ArrangeIconsInCylinder()
    {
        int iconCount = icons.Count;
        for (int i = 0; i < iconCount; i++)
        {
            float angle = i * Mathf.PI * 2 / iconCount;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * depth, Mathf.Sin(angle) * radius);
            icons[i].localPosition = position;
            icons[i].localScale = Vector3.one; // 全アイコンを初期サイズに
        }
    }

    // アイコンを右に回転させる
    void RotateRight()
    {
        currentIndex = (currentIndex + 1) % icons.Count;
        StartCoroutine(RotateRing(-1));
    }

    // アイコンを左に回転させる
    void RotateLeft()
    {
        currentIndex = (currentIndex - 1 + icons.Count) % icons.Count;
        StartCoroutine(RotateRing(1));
    }

    // リングを回転させるコルーチン
    IEnumerator RotateRing(int direction)
    {
        float elapsedTime = 0f;
        float duration = 0.3f; // 回転にかける時間

        while (elapsedTime < duration)
        {
            float angleStep = rotationSpeed * direction * Time.deltaTime;
            transform.Rotate(0, angleStep, 0); // Y軸で回転させて奥行きの効果を出す
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ArrangeIconsInCylinder(); // 位置を更新
    }

    // 中央のアイコンをハイライト表示する関数
    void HighlightCenterIcon()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if (i == currentIndex)
            {
                icons[i].localScale = Vector3.one * 1.5f; // 中央アイコンを拡大
            }
            else
            {
                icons[i].localScale = Vector3.one; // その他のアイコンを元のサイズに
            }
        }
    }
}