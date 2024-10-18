using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public partial class StageEditorWindow : EditorWindow
{
    //マップの大きさ
    private int mapSize = 0;
    private int oldMapSize = 0;

    //グリッドのサイズ
    private int gridSize = 40;

    //テクスチャ系
    private Texture damageItemTexture;
    private Texture offensivePowerItemTexture;
    private Texture generator1Texture;
    private Texture generator2Texture;

    //選ばれているチップ（アイテムやジェネレータなど）
    private int selectedChipType = -1;

    //マップの情報
    private int[,] mapData = new int[0, 0];

    //出力するcsvファイル
    private string outputFilePath = "/Users/yuta/Documents/Unity/programing!/Assets/Resources/CodeFightMap1.csv";

    //おまじない　これをつけないとマップエディタを開けない
    [MenuItem("Map/Open StageEditorWindow")]
    private static void Open()
    {
        StageEditorWindow window = GetWindow<StageEditorWindow>();
        window.titleContent = new GUIContent("Map Editor");

        //スタティック関数なので初期化はこのようにしないといけない
        window.damageItemTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/damageItem.png");
        window.offensivePowerItemTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/offensivePowerItem.png");
        window.generator1Texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/generator1.png");
        window.generator2Texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/generator2.png");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        {
            GUILayout.Label($"Map Size : {mapSize}x{mapSize}");

            bool isInteger = true;
            isInteger = int.TryParse(GUILayout.TextField(mapSize.ToString()), out mapSize);

            if (mapSize != oldMapSize)
            {
                mapData = new int[mapSize, mapSize];
                //initialize
                for (int y = 0; y < mapSize; y++)
                    for (int x = 0; x < mapSize; x++)
                        mapData[y, x] = -1;
            }

            oldMapSize = mapSize;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUI.skin.box);
        {
            //as number of csv
            if (GUILayout.Button(offensivePowerItemTexture, GUILayout.Height(100), GUILayout.Width(100)))
            {
                selectedChipType = 0;
            }
            if (GUILayout.Button(damageItemTexture, GUILayout.Height(100), GUILayout.Width(100)))
            {
                selectedChipType = 1;
            }
            if (GUILayout.Button(generator1Texture, GUILayout.Height(100), GUILayout.Width(100)))
            {
                selectedChipType = -2;
            }
            if (GUILayout.Button(generator2Texture, GUILayout.Height(100), GUILayout.Width(100)))
            {
                selectedChipType = -3;
            }
            if (GUILayout.Button("Clear", GUILayout.Height(100), GUILayout.Width(100)))
            {
                selectedChipType = -1;
            }
        }
        GUILayout.EndHorizontal();

        //display selected chip
        GUILayout.Label("Selected Chip");
        switch (selectedChipType)
        {
            case 0:
                GUILayout.Box(offensivePowerItemTexture, GUILayout.Height(100), GUILayout.Width(100));
                break;
            case 1:
                GUILayout.Box(damageItemTexture, GUILayout.Height(100), GUILayout.Width(100));
                break;
            case -2:
                GUILayout.Box(generator1Texture, GUILayout.Height(100), GUILayout.Width(100));
                break;
            case -3:
                GUILayout.Box(generator2Texture, GUILayout.Height(100), GUILayout.Width(100));
                break;
            case -1:
                GUILayout.Box("Clear", GUILayout.Height(100), GUILayout.Width(100));
                break;
            default:
                GUILayout.Box("None", GUILayout.Height(100), GUILayout.Width(100));
                break;
        }

        Vector3 additionalVector = new Vector3(10, 280, 0);
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Rect rectangle = new Rect(x * gridSize + additionalVector.x, y * gridSize + additionalVector.y, gridSize, gridSize);

                Handles.color = Color.white;
                Handles.DrawSolidRectangleWithOutline(rectangle, Color.white, Color.white);

                //draw item or generator
                if (mapData[y, x] == 0)
                {
                    GUI.DrawTextureWithTexCoords(rectangle, offensivePowerItemTexture, new Rect(0f, 0f, 1f, 1f));
                }
                else if (mapData[y, x] == 1)
                {
                    GUI.DrawTextureWithTexCoords(rectangle, damageItemTexture, new Rect(0f, 0f, 1f, 1f));
                }
                else if (mapData[y, x] == -2)
                {
                    GUI.DrawTextureWithTexCoords(rectangle, generator1Texture, new Rect(0f, 0f, 1f, 1f));
                }
                else if (mapData[y, x] == -3)
                {
                    GUI.DrawTextureWithTexCoords(rectangle, generator2Texture, new Rect(0f, 0f, 1f, 1f));
                }

                //draw grid
                Handles.color = Color.gray;
                Handles.DrawLine(new Vector3(x * gridSize, y * gridSize, 0) + additionalVector,
                                 new Vector3((x + 1) * gridSize, y * gridSize, 0) + additionalVector);
                Handles.DrawLine(new Vector3(x * gridSize, (y + 1) * gridSize, 0) + additionalVector,
                                 new Vector3((x + 1) * gridSize, (y + 1) * gridSize, 0) + additionalVector);
                Handles.DrawLine(new Vector3(x * gridSize, y * gridSize, 0) + additionalVector,
                                 new Vector3(x * gridSize, (y + 1) * gridSize, 0) + additionalVector);
                Handles.DrawLine(new Vector3((x + 1) * gridSize, y * gridSize, 0) + additionalVector,
                                 new Vector3((x + 1) * gridSize, (y + 1) * gridSize, 0) + additionalVector);
            }
        }

        this.minSize = new Vector2(mapSize * gridSize + additionalVector.x * 2, mapSize * gridSize + additionalVector.y + 60);
        float minXOfWindowSize = 570f;
        this.maxSize = minSize.x > minXOfWindowSize ? minSize : new Vector2(minXOfWindowSize, minSize.y);

        //When user clicks on grid, Update map data.
        Vector2 cursorPosition = Event.current.mousePosition;
        Vector2 cursorPositionOnGrid = cursorPosition - new Vector2(additionalVector.x, additionalVector.y);
        Vector2 cellWithCursorInGrid = cursorPositionOnGrid / gridSize;

        if (Event.current.type == EventType.MouseDown)
        {
            try
            {
                mapData[(int)cellWithCursorInGrid.y, (int)cellWithCursorInGrid.x] = selectedChipType;
            }
            catch (Exception) { }
        }

        Rect rect = new Rect(0, this.minSize.y - 50, 300, 50);
        GUILayout.BeginArea(rect);

        if (GUILayout.Button("Output File", GUILayout.MinWidth(300), GUILayout.MinHeight(50), GUILayout.MaxWidth(300), GUILayout.MaxWidth(300)))
        {
            OutputFile();
        }

        GUILayout.EndArea();

        //画面を書き直す
        //repaint
        Repaint();
    }

    private void OutputFile()
    {
        string csvData = "";
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            string csvDataLine = "";
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                csvDataLine += mapData[y, x];
                csvDataLine += ",";
            }
            //一番最後の","を取り除く これをつけないとCSVの形式にならない
            csvDataLine = csvDataLine.Substring(0, csvDataLine.Length - 1);

            csvData += csvDataLine + "\r";
        }

        //ファイルに出力!!
        File.WriteAllText(outputFilePath, csvData);
    }
}
