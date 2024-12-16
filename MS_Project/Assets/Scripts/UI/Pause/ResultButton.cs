using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultButton : MonoBehaviour
{
    // ボタンの参照
    [SerializeField, Header("次のステージへ行くボタン")]
    public Button NextStageButton;
    [SerializeField, Header("セレクトステージへ行くボタン")]
    public Button GoToSelectButton;

    // ボタンリスト
    private List<Button> buttons;
    private int selectedIndex = 0; // 現在選択されているボタンのインデックス

    void Start()
    {
        // ボタンをリストに追加
        buttons = new List<Button> { NextStageButton, GoToSelectButton };

        // 最初にボタンを選択
        UpdateButtonSelection();

        Time.timeScale = 0;
    }
    void Update()
    {
        // Wキーで次のボタンへ移動
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveSelection(-1); // 前のボタンに移動
        }

        // Sキーで前のボタンへ移動
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveSelection(1); // 次のボタンに移動
        }

        // Enterキーで選択されたボタンを実行
        if (Input.GetKeyDown(KeyCode.Return)) // Enterキーでボタンを選択
        {
            ExecuteSelectedButton();
        }

        // 選択されたボタンに視覚的な変更を適用
        UpdateButtonSelection();
    }

    // ボタン選択のインデックスを変更
    void MoveSelection(int direction)
    {
        selectedIndex += direction;

        // 範囲を超えないように調整
        if (selectedIndex < 0)
        {
            selectedIndex = buttons.Count - 1; // 最後のボタンに移動
        }
        else if (selectedIndex >= buttons.Count)
        {
            selectedIndex = 0; // 最初のボタンに移動
        }

        // 選択されたボタンの更新
        UpdateButtonSelection();
    }

    // 選択されたボタンのスタイルを更新
    void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == selectedIndex)
            {
                // 現在選択されたボタンにハイライト（色を変えるなどの処理）
                buttons[i].Select(); // ボタンを選択状態に
            }
            else
            {
                // 非選択状態の処理（必要なら色を戻す等）
            }
        }
    }

    // 選択されたボタンを実行する
    void ExecuteSelectedButton()
    {
        // 選択されたボタンを実行
        buttons[selectedIndex].onClick.Invoke();
        Time.timeScale = 1;
    }
}
