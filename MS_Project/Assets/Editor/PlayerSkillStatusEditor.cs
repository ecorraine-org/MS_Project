using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// PlayerSkillData用表示編集
/// </summary>
[CustomEditor(typeof(PlayerSkillData))]
public class PlayerSkillDataEditor : Editor
{
    // SkillStatus配列のSerializedPropertyを取得
    SerializedProperty skillArray;

    // 各スキルの折りたたみ状態を保存するリスト
    private List<bool> foldoutStates = new List<bool>();

    private void OnEnable()
    {
        // skill配列のSerializedPropertyを取得
        skillArray = serializedObject.FindProperty("skill");

        // foldoutStatesの初期化（スキル数に合わせる）
        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
        {
            foldoutStates.Add(true); // デフォルトではすべて展開
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // SerializedObjectを更新

        // スキルリストを表示
        for (int i = 0; i < skillArray.arraySize; i++)
        {
            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

            // 折りたたみUI
            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細表示
            {
                EditorGUI.indentLevel++; // インデントを追加

                // 属性をカテゴリー別に表示

                // 基本属性
                EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
                SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
                EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));

                // Dash属性
                EditorGUILayout.LabelField("Dash Properties", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));

                // HPとDamage属性
                EditorGUILayout.LabelField("HP and Damage", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));

                // SlowとStop属性
                EditorGUILayout.LabelField("Slow and Stop", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));

                // Charge属性
                EditorGUILayout.LabelField("Charge Properties", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

                // canChargeがtrueの場合、SizeRateを表示
                SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
                if (canCharge.boolValue)
                {
                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
                }

                // Cancel属性
                EditorGUILayout.LabelField("Cancel Properties", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));

                // 調整ボタン
                GUILayout.BeginHorizontal();
                if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30))) // 上に移動ボタン
                {
                    skillArray.MoveArrayElement(i, i - 1);
                    foldoutStates[i] = foldoutStates[i - 1];
                    foldoutStates[i - 1] = true; // 折りたたみ状態を維持
                }
                if (i < skillArray.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(30))) // 下に移動ボタン
                {
                    skillArray.MoveArrayElement(i, i + 1);
                    foldoutStates[i] = foldoutStates[i + 1];
                    foldoutStates[i + 1] = true; // 折りたたみ状態を維持
                }
                GUILayout.EndHorizontal();

                // スキル削除ボタン
                if (GUILayout.Button("Remove Skill"))
                {
                    skillArray.DeleteArrayElementAtIndex(i);
                    foldoutStates.RemoveAt(i); // 折りたたみ状態も削除
                    serializedObject.ApplyModifiedProperties();
                    break;
                }

                EditorGUI.indentLevel--; // インデントを戻す
            }

            EditorGUILayout.Space(); // スキル間にスペースを追加
        }

        // スキル追加ボタン
        if (GUILayout.Button("Add Skill"))
        {
            skillArray.arraySize++;
            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // デフォルトのスキル名

            foldoutStates.Add(true); // 新しいスキルの折りたたみ状態を追加
        }

        serializedObject.ApplyModifiedProperties(); // 変更を適用

        // 変更があった場合、変更を保存
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}



#region 折り畳み状態保存1.0
//1.0日本語
//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    // 各スキルの折りたたみ状態を保存するリスト
//    private List<bool> foldoutStates = new List<bool>();
//    // 属性タイプの折りたたみ状態
//    private bool foldoutBasicProperties = true;
//    private bool foldoutDashProperties = true;
//    private bool foldoutHpDamageProperties = true;
//    private bool foldoutSlowStopProperties = true;
//    private bool foldoutChargeProperties = true;
//    private bool foldoutCancelProperties = true;

//    private const string foldoutKeyPrefix = "PlayerSkillDataEditor_FoldoutState_";

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");

//        // foldoutStatesの初期化（スキル数に合わせる）
//        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
//        {
//            foldoutStates.Add(true); // デフォルトではすべて展開
//        }

//        // EditorPrefsから折りたたみ状態を読み込む
//        LoadFoldoutStates();
//    }

//    private void OnDisable()
//    {
//        // 折りたたみ状態を保存
//        SaveFoldoutStates();
//    }

//    private void LoadFoldoutStates()
//    {
//        // 各折りたたみ状態を読み込む
//        foldoutBasicProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "BasicProperties", true);
//        foldoutDashProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "DashProperties", true);
//        foldoutHpDamageProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "HpDamageProperties", true);
//        foldoutSlowStopProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "SlowStopProperties", true);
//        foldoutChargeProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "ChargeProperties", true);
//        foldoutCancelProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "CancelProperties", true);

//        // スキルごとの折りたたみ状態を更新
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            foldoutStates[i] = EditorPrefs.GetBool(foldoutKeyPrefix + $"Skill_{i}", true);
//        }
//    }

//    private void SaveFoldoutStates()
//    {
//        // 各折りたたみ状態を保存
//        EditorPrefs.SetBool(foldoutKeyPrefix + "BasicProperties", foldoutBasicProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "DashProperties", foldoutDashProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "HpDamageProperties", foldoutHpDamageProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "SlowStopProperties", foldoutSlowStopProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "ChargeProperties", foldoutChargeProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "CancelProperties", foldoutCancelProperties);

//        // スキルごとの折りたたみ状態を保存
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            EditorPrefs.SetBool(foldoutKeyPrefix + $"Skill_{i}", foldoutStates[i]);
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // スキルリストを表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // 折りたたみUI
//            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

//            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細を表示
//            {
//                EditorGUI.indentLevel++; // インデントを追加

//                // 属性タイプ部分
//                // 基本属性
//                foldoutBasicProperties = EditorGUILayout.Foldout(foldoutBasicProperties, "Basic Properties");
//                if (foldoutBasicProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//                    EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));
//                    EditorGUI.indentLevel--;
//                }

//                // Dash属性
//                foldoutDashProperties = EditorGUILayout.Foldout(foldoutDashProperties, "Dash Properties");
//                if (foldoutDashProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));
//                    EditorGUI.indentLevel--;
//                }

//                // HPとDamage属性
//                foldoutHpDamageProperties = EditorGUILayout.Foldout(foldoutHpDamageProperties, "HP and Damage");
//                if (foldoutHpDamageProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));
//                    EditorGUI.indentLevel--;
//                }

//                // SlowとStop属性
//                foldoutSlowStopProperties = EditorGUILayout.Foldout(foldoutSlowStopProperties, "Slow and Stop");
//                if (foldoutSlowStopProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));
//                    EditorGUI.indentLevel--;
//                }

//                // Charge属性
//                foldoutChargeProperties = EditorGUILayout.Foldout(foldoutChargeProperties, "Charge Properties");
//                if (foldoutChargeProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

//                    // canChargeがtrueの場合、SizeRateを表示
//                    SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//                    if (canCharge.boolValue)
//                    {
//                        EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
//                    }
//                    EditorGUI.indentLevel--;
//                }

//                // Cancel属性
//                foldoutCancelProperties = EditorGUILayout.Foldout(foldoutCancelProperties, "Cancel Properties");
//                if (foldoutCancelProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));
//                    EditorGUI.indentLevel--;
//                }

//                // 順番変更ボタン
//                GUILayout.BeginHorizontal();
//                if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30))) // 上に移動
//                {
//                    skillArray.MoveArrayElement(i, i - 1);
//                    foldoutStates[i] = foldoutStates[i - 1];
//                    foldoutStates[i - 1] = true; // 展開状態を保持
//                }
//                if (i < skillArray.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(30))) // 下に移動
//                {
//                    skillArray.MoveArrayElement(i, i + 1);
//                    foldoutStates[i] = foldoutStates[i + 1];
//                    foldoutStates[i + 1] = true; // 展開状態を保持
//                }
//                GUILayout.EndHorizontal();

//                // スキル削除ボタン
//                if (GUILayout.Button("Remove Skill"))
//                {
//                    skillArray.DeleteArrayElementAtIndex(i);
//                    foldoutStates.RemoveAt(i); // 折りたたみ状態を削除
//                    serializedObject.ApplyModifiedProperties();
//                    break;
//                }

//                EditorGUI.indentLevel--; // インデントを戻す
//            }

//            EditorGUILayout.Space(); // スキル間のスペース
//        }

//        // スキル追加ボタン
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // デフォルトのスキル名

//            foldoutStates.Add(true); // 新しいスキルの折りたたみ状態を追加
//        }

//        serializedObject.ApplyModifiedProperties(); // 変更を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}


//1.0
//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    // 各スキルの折りたたみ状態を保存するリスト
//    private List<bool> foldoutStates = new List<bool>();
//    // 属性类型折叠状态
//    private bool foldoutBasicProperties = true;
//    private bool foldoutDashProperties = true;
//    private bool foldoutHpDamageProperties = true;
//    private bool foldoutSlowStopProperties = true;
//    private bool foldoutChargeProperties = true;
//    private bool foldoutCancelProperties = true;

//    private const string foldoutKeyPrefix = "PlayerSkillDataEditor_FoldoutState_";

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");

//        // foldoutStatesの初期化（スキル数に合わせる）
//        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
//        {
//            foldoutStates.Add(true); // デフォルトではすべて展開
//        }

//        // 从EditorPrefs加载折叠状态
//        LoadFoldoutStates();
//    }

//    private void OnDisable()
//    {
//        // 保存折叠状态
//        SaveFoldoutStates();
//    }

//    private void LoadFoldoutStates()
//    {
//        // 读取每个折叠状态
//        foldoutBasicProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "BasicProperties", true);
//        foldoutDashProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "DashProperties", true);
//        foldoutHpDamageProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "HpDamageProperties", true);
//        foldoutSlowStopProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "SlowStopProperties", true);
//        foldoutChargeProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "ChargeProperties", true);
//        foldoutCancelProperties = EditorPrefs.GetBool(foldoutKeyPrefix + "CancelProperties", true);

//        // 更新折叠状态
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            foldoutStates[i] = EditorPrefs.GetBool(foldoutKeyPrefix + $"Skill_{i}", true);
//        }
//    }

//    private void SaveFoldoutStates()
//    {
//        // 保存每个折叠状态
//        EditorPrefs.SetBool(foldoutKeyPrefix + "BasicProperties", foldoutBasicProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "DashProperties", foldoutDashProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "HpDamageProperties", foldoutHpDamageProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "SlowStopProperties", foldoutSlowStopProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "ChargeProperties", foldoutChargeProperties);
//        EditorPrefs.SetBool(foldoutKeyPrefix + "CancelProperties", foldoutCancelProperties);

//        // 保存技能折叠状态
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            EditorPrefs.SetBool(foldoutKeyPrefix + $"Skill_{i}", foldoutStates[i]);
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // スキルリスト表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // 折りたたみUI
//            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

//            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細表示
//            {
//                EditorGUI.indentLevel++; // インデントを追加

//                // 属性归类部分
//                // 基本属性
//                foldoutBasicProperties = EditorGUILayout.Foldout(foldoutBasicProperties, "Basic Properties");
//                if (foldoutBasicProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//                    EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));
//                    EditorGUI.indentLevel--;
//                }

//                // Dash属性
//                foldoutDashProperties = EditorGUILayout.Foldout(foldoutDashProperties, "Dash Properties");
//                if (foldoutDashProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));
//                    EditorGUI.indentLevel--;
//                }

//                // HP和Damage属性
//                foldoutHpDamageProperties = EditorGUILayout.Foldout(foldoutHpDamageProperties, "HP and Damage");
//                if (foldoutHpDamageProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));
//                    EditorGUI.indentLevel--;
//                }

//                // Slow和Stop属性
//                foldoutSlowStopProperties = EditorGUILayout.Foldout(foldoutSlowStopProperties, "Slow and Stop");
//                if (foldoutSlowStopProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));
//                    EditorGUI.indentLevel--;
//                }

//                // Charge属性
//                foldoutChargeProperties = EditorGUILayout.Foldout(foldoutChargeProperties, "Charge Properties");
//                if (foldoutChargeProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

//                    // canChargeがtrueの場合、SizeRateを表示
//                    SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//                    if (canCharge.boolValue)
//                    {
//                        EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
//                    }
//                    EditorGUI.indentLevel--;
//                }

//                // Cancel属性
//                foldoutCancelProperties = EditorGUILayout.Foldout(foldoutCancelProperties, "Cancel Properties");
//                if (foldoutCancelProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));
//                    EditorGUI.indentLevel--;
//                }

//                // 调序按钮
//                GUILayout.BeginHorizontal();
//                if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30))) // 向上按钮
//                {
//                    skillArray.MoveArrayElement(i, i - 1);
//                    foldoutStates[i] = foldoutStates[i - 1];
//                    foldoutStates[i - 1] = true; // 保持展开状态
//                }
//                if (i < skillArray.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(30))) // 向下按钮
//                {
//                    skillArray.MoveArrayElement(i, i + 1);
//                    foldoutStates[i] = foldoutStates[i + 1];
//                    foldoutStates[i + 1] = true; // 保持展开状态
//                }
//                GUILayout.EndHorizontal();

//                // スキル削除ボタン
//                if (GUILayout.Button("Remove Skill"))
//                {
//                    skillArray.DeleteArrayElementAtIndex(i);
//                    foldoutStates.RemoveAt(i); // 删除折叠状态
//                    serializedObject.ApplyModifiedProperties();
//                    break;
//                }

//                EditorGUI.indentLevel--; // 恢复缩进
//            }

//            EditorGUILayout.Space(); // 添加技能之间的空间
//        }

//        // 添加技能按钮
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // 默认的技能名

//            foldoutStates.Add(true); // 为新技能添加折叠状态
//        }

//        serializedObject.ApplyModifiedProperties(); // 应用更改

//        // 如果有更改，保存这些更改
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}
#endregion


#region 属性分けた PullDownMenu
//属性分けた PullDownMenu
//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    // 各スキルの折りたたみ状態を保存するリスト
//    private List<bool> foldoutStates = new List<bool>();
//    // 属性类型折叠状态
//    private bool foldoutBasicProperties = true;
//    private bool foldoutDashProperties = true;
//    private bool foldoutHpDamageProperties = true;
//    private bool foldoutSlowStopProperties = true;
//    private bool foldoutChargeProperties = true;
//    private bool foldoutCancelProperties = true;

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");

//        // foldoutStatesの初期化（スキル数に合わせる）
//        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
//        {
//            foldoutStates.Add(true); // デフォルトではすべて展開
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // スキルリスト表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // 折りたたみUI
//            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

//            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細表示
//            {
//                EditorGUI.indentLevel++; // インデントを追加

//                // 属性归类部分
//                // 基本属性
//                foldoutBasicProperties = EditorGUILayout.Foldout(foldoutBasicProperties, "Basic Properties");
//                if (foldoutBasicProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//                    EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));
//                    EditorGUI.indentLevel--;
//                }

//                // Dash属性
//                foldoutDashProperties = EditorGUILayout.Foldout(foldoutDashProperties, "Dash Properties");
//                if (foldoutDashProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));
//                    EditorGUI.indentLevel--;
//                }

//                // HP和Damage属性
//                foldoutHpDamageProperties = EditorGUILayout.Foldout(foldoutHpDamageProperties, "HP and Damage");
//                if (foldoutHpDamageProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));
//                    EditorGUI.indentLevel--;
//                }

//                // Slow和Stop属性
//                foldoutSlowStopProperties = EditorGUILayout.Foldout(foldoutSlowStopProperties, "Slow and Stop");
//                if (foldoutSlowStopProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));
//                    EditorGUI.indentLevel--;
//                }

//                // Charge属性
//                foldoutChargeProperties = EditorGUILayout.Foldout(foldoutChargeProperties, "Charge Properties");
//                if (foldoutChargeProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

//                    // canChargeがtrueの場合、SizeRateを表示
//                    SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//                    if (canCharge.boolValue)
//                    {
//                        EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
//                    }
//                    EditorGUI.indentLevel--;
//                }

//                // Cancel属性
//                foldoutCancelProperties = EditorGUILayout.Foldout(foldoutCancelProperties, "Cancel Properties");
//                if (foldoutCancelProperties)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));
//                    EditorGUI.indentLevel--;
//                }

//                // 调序按钮
//                GUILayout.BeginHorizontal();
//                if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30))) // 向上按钮
//                {
//                    skillArray.MoveArrayElement(i, i - 1);
//                    foldoutStates[i] = foldoutStates[i - 1];
//                    foldoutStates[i - 1] = true; // 保持展开状态
//                }
//                if (i < skillArray.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(30))) // 向下按钮
//                {
//                    skillArray.MoveArrayElement(i, i + 1);
//                    foldoutStates[i] = foldoutStates[i + 1];
//                    foldoutStates[i + 1] = true; // 保持展开状态
//                }
//                GUILayout.EndHorizontal();

//                // スキル削除ボタン
//                if (GUILayout.Button("Remove Skill"))
//                {
//                    skillArray.DeleteArrayElementAtIndex(i);
//                    foldoutStates.RemoveAt(i); // 删除折叠状态
//                    serializedObject.ApplyModifiedProperties();
//                    break;
//                }

//                EditorGUI.indentLevel--; // 恢复缩进
//            }

//            EditorGUILayout.Space(); // 添加技能之间的空间
//        }

//        // 添加技能按钮
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // 默认的技能名

//            foldoutStates.Add(true); // 为新技能添加折叠状态
//        }

//        serializedObject.ApplyModifiedProperties(); // 应用更改

//        // 如果有更改，保存这些更改
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}
#endregion




////分けた
//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    // 各スキルの折りたたみ状態を保存するリスト
//    private List<bool> foldoutStates = new List<bool>();

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");

//        // foldoutStatesの初期化（スキル数に合わせる）
//        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
//        {
//            foldoutStates.Add(true); // デフォルトではすべて展開
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // スキルリスト表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // 折りたたみUI
//            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

//            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細表示
//            {
//                EditorGUI.indentLevel++; // インデントを追加

//                // 属性归类部分

//                // 基本属性
//                EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
//                SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//                EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));

//                // Dash属性
//                EditorGUILayout.LabelField("Dash Properties", EditorStyles.boldLabel);
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));

//                // HP和Damage属性
//                EditorGUILayout.LabelField("HP and Damage", EditorStyles.boldLabel);
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));

//                // Slow and Stop属性
//                EditorGUILayout.LabelField("Slow and Stop", EditorStyles.boldLabel);
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));

//                // Charge属性
//                EditorGUILayout.LabelField("Charge Properties", EditorStyles.boldLabel);
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

//                // canChargeがtrueの場合、SizeRateを表示
//                SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//                if (canCharge.boolValue)
//                {
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
//                }

//                // Cancel属性
//                EditorGUILayout.LabelField("Cancel Properties", EditorStyles.boldLabel);
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));

//                // 调序按钮
//                GUILayout.BeginHorizontal();
//                if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30))) // 向上按钮
//                {
//                    skillArray.MoveArrayElement(i, i - 1);
//                    foldoutStates[i] = foldoutStates[i - 1];
//                    foldoutStates[i - 1] = true; // 保持展开状态
//                }
//                if (i < skillArray.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(30))) // 向下按钮
//                {
//                    skillArray.MoveArrayElement(i, i + 1);
//                    foldoutStates[i] = foldoutStates[i + 1];
//                    foldoutStates[i + 1] = true; // 保持展开状态
//                }
//                GUILayout.EndHorizontal();

//                // スキル削除ボタン
//                if (GUILayout.Button("Remove Skill"))
//                {
//                    skillArray.DeleteArrayElementAtIndex(i);
//                    foldoutStates.RemoveAt(i); // 删除折叠状态
//                    serializedObject.ApplyModifiedProperties();
//                    break;
//                }

//                EditorGUI.indentLevel--; // 恢复缩进
//            }

//            EditorGUILayout.Space(); // 添加技能之间的空间
//        }

//        // 添加技能按钮
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // 默认的技能名

//            foldoutStates.Add(true); // 为新技能添加折叠状态
//        }

//        serializedObject.ApplyModifiedProperties(); // 应用更改

//        // 如果有更改，保存这些更改
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}



//順番変更
//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    // 各スキルの折りたたみ状態を保存するリスト
//    private List<bool> foldoutStates = new List<bool>();

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");

//        // foldoutStatesの初期化（スキル数に合わせる）
//        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
//        {
//            foldoutStates.Add(true); // デフォルトではすべて展開
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // スキルリスト表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // 折りたたみUI
//            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

//            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細表示
//            {
//                EditorGUI.indentLevel++; // インデントを追加

//                // スキル名（skillName）を表示
//                SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//                EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));

//                // スキルの各プロパティを表示
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

//                // canChargeがtrueの場合、SizeRateを表示
//                SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//                if (canCharge.boolValue)
//                {
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
//                }

//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));

//                // 调序按钮
//                GUILayout.BeginHorizontal();
//                if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30))) // 向上按钮
//                {
//                    skillArray.MoveArrayElement(i, i - 1);
//                    foldoutStates[i] = foldoutStates[i - 1];
//                    foldoutStates[i - 1] = true; // 保持展开状态
//                }
//                if (i < skillArray.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(30))) // 向下按钮
//                {
//                    skillArray.MoveArrayElement(i, i + 1);
//                    foldoutStates[i] = foldoutStates[i + 1];
//                    foldoutStates[i + 1] = true; // 保持展开状态
//                }
//                GUILayout.EndHorizontal();

//                // スキル削除ボタン
//                if (GUILayout.Button("Remove Skill"))
//                {
//                    skillArray.DeleteArrayElementAtIndex(i);
//                    foldoutStates.RemoveAt(i); // 折りたたみ状態も削除
//                    serializedObject.ApplyModifiedProperties();
//                    break;
//                }

//                EditorGUI.indentLevel--; // インデントを戻す
//            }

//            EditorGUILayout.Space(); // スキル間のスペース
//        }

//        // スキル追加ボタン
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // デフォルトのスキル名

//            foldoutStates.Add(true); // 新しいスキルの折りたたみ状態を追加
//        }

//        serializedObject.ApplyModifiedProperties(); // 修正を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}





//////Pull down menu
//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    // 各スキルの折りたたみ状態を保存するリスト
//    private List<bool> foldoutStates = new List<bool>();

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");

//        // foldoutStatesの初期化（スキル数に合わせる）
//        for (int i = foldoutStates.Count; i < skillArray.arraySize; i++)
//        {
//            foldoutStates.Add(true); // デフォルトではすべて展開
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // スキルリスト表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // 折りたたみUI
//            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Skill {i + 1}", true, EditorStyles.foldoutHeader);

//            if (foldoutStates[i]) // 折りたたみが展開されている場合のみ詳細表示
//            {
//                EditorGUI.indentLevel++; // インデントを追加

//                // スキル名（skillName）を表示
//                SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//                EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name"));

//                // スキルの各プロパティを表示
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"), new GUIContent("Cool Time"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"), new GUIContent("Dash Speed"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"), new GUIContent("Dash Duration"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"), new GUIContent("HP Cost"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"), new GUIContent("Damage"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"), new GUIContent("Slow Speed"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"), new GUIContent("Stop Duration"));
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"), new GUIContent("Can Charge"));

//                // canChargeがtrueの場合、SizeRateを表示
//                SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//                if (canCharge.boolValue)
//                {
//                    EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"), new GUIContent("Size Rate"));
//                }

//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"), new GUIContent("Can Cancel"));

//                // スキル削除ボタン
//                if (GUILayout.Button("Remove Skill"))
//                {
//                    skillArray.DeleteArrayElementAtIndex(i);
//                    foldoutStates.RemoveAt(i); // 折りたたみ状態も削除
//                    serializedObject.ApplyModifiedProperties();
//                    break;
//                }

//                EditorGUI.indentLevel--; // インデントを戻す
//            }

//            EditorGUILayout.Space(); // スキル間のスペース
//        }

//        // スキル追加ボタン
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // デフォルトのスキル名

//            foldoutStates.Add(true); // 新しいスキルの折りたたみ状態を追加
//        }

//        serializedObject.ApplyModifiedProperties(); // 修正を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}



////追加削除ある方
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // skill配列の要素を一つずつ表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // スキル名（skillName）を表示（もしPlayerSkillがenum型の場合、ドロップダウンとして表示される）
//            SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//            EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name")); // スキル名を表示

//            // スキルの各プロパティを表示
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime")); // クールタイム
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed")); // 突進速度
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration")); // 突進持続時間
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost")); // HP消費量
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage")); // 攻撃力
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed")); // ヒットストップによる減速
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration")); // ヒットストップ持続時間
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge")); // 長押し(チャージ)可能かどうか

//            // canChargeがtrueの場合、SizeRateを表示
//            SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//            if (canCharge.boolValue)
//            {
//                // SizeRateの表示
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"));
//            }

//            // canCancelの表示
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel")); // 回避でキャンセル可能かどうか

//            // スキル削除ボタン
//            if (GUILayout.Button("Remove Skill"))
//            {
//                skillArray.DeleteArrayElementAtIndex(i);
//                serializedObject.ApplyModifiedProperties();
//                break; // 削除後にインデックスが変わるのでループを終了
//            }

//            EditorGUILayout.Space(); // 各スキル間にスペースを追加
//        }

//        // スキル追加ボタン
//        if (GUILayout.Button("Add Skill"))
//        {
//            skillArray.arraySize++;
//            SerializedProperty newSkill = skillArray.GetArrayElementAtIndex(skillArray.arraySize - 1);
//            newSkill.FindPropertyRelative("skillName").stringValue = "New Skill"; // デフォルトのスキル名を設定
//            // 必要なら他のプロパティも初期化
//        }

//        serializedObject.ApplyModifiedProperties(); // 修正を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}


//追加足りない方
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    // SkillStatus配列のSerializedPropertyを取得
//    SerializedProperty skillArray;

//    private void OnEnable()
//    {
//        // skill配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        // skill配列の要素を一つずつ表示
//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            // スキル名（skillName）を表示（もしPlayerSkillがenum型の場合、ドロップダウンとして表示される）
//            SerializedProperty skillNameProperty = skill.FindPropertyRelative("skillName");
//            EditorGUILayout.PropertyField(skillNameProperty, new GUIContent("Skill Name")); // スキル名を表示

//            // スキルの各プロパティを表示
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime")); // クールタイム
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed")); // 突進速度
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration")); // 突進持続時間
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost")); // HP消費量
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage")); // 攻撃力
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed")); // ヒットストップによる減速
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration")); // ヒットストップ持続時間
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge")); // 長押し(チャージ)可能かどうか

//            // canChargeがtrueの場合、SizeRateを表示
//            SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//            if (canCharge.boolValue)
//            {
//                // SizeRateの表示
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"));
//            }

//            // canCancelの表示
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel")); // 回避でキャンセル可能かどうか

//            EditorGUILayout.Space(); // 各スキル間にスペースを追加
//        }

//        serializedObject.ApplyModifiedProperties(); // 修正を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}





// インデントを追加
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    SerializedProperty skillArray;

//    private void OnEnable()
//    {
//        // SkillStatus配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            EditorGUILayout.LabelField("Skill: " + skill.FindPropertyRelative("skillName").stringValue); // スキル名を表示

//            // スキルの各プロパティを表示
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"));

//            // canChargeがtrueの場合、SizeRateを表示
//            SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//            if (canCharge.boolValue)
//            {
//                EditorGUILayout.BeginVertical(); // SizeRateの前に新しいレイアウトを開始
//                EditorGUI.indentLevel++; // インデントを追加
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"));
//                EditorGUI.indentLevel--; // インデントを戻す
//                EditorGUILayout.EndVertical(); // レイアウトを終了
//            }

//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"));

//            EditorGUILayout.Space(); // 各スキルの間にスペースを追加
//        }

//        serializedObject.ApplyModifiedProperties(); // 修正を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}










//ノーマル
//using UnityEditor;
//using UnityEngine;

///// <summary>
///// PlayerSkillDataのパラメーターの表示制御
///// </summary>
//[CustomEditor(typeof(PlayerSkillData))]
//public class PlayerSkillDataEditor : Editor
//{
//    SerializedProperty skillArray;

//    private void OnEnable()
//    {
//        // SkillStatus配列のSerializedPropertyを取得
//        skillArray = serializedObject.FindProperty("skill");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update(); // SerializedObjectを更新

//        for (int i = 0; i < skillArray.arraySize; i++)
//        {
//            SerializedProperty skill = skillArray.GetArrayElementAtIndex(i); // 配列の各要素を取得

//            EditorGUILayout.LabelField("Skill: " + skill.FindPropertyRelative("skillName").stringValue); // スキル名を表示

//            // スキルの各プロパティを表示
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("coolTime"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashSpeed"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("dashDuration"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("hpCost"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("damage"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("slowSpeed"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("stopDuration"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCharge"));
//            EditorGUILayout.PropertyField(skill.FindPropertyRelative("canCancel"));

//            // canChargeがtrueの場合、SizeRateを表示
//            SerializedProperty canCharge = skill.FindPropertyRelative("canCharge");
//            if (canCharge.boolValue)
//            {
//                EditorGUILayout.PropertyField(skill.FindPropertyRelative("SizeRate"));
//            }

//            EditorGUILayout.Space(); // 各スキルの間にスペースを追加
//        }

//        serializedObject.ApplyModifiedProperties(); // 修正を適用

//        // 変更があった場合、変更を保存
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}
