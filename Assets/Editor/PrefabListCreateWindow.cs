using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabListCreateWindow : EditorWindow {

    private const string PATH = "Assets/Prefabs/List.asset";

    private PrefabList list;

    /// <summary>
    /// メニューに追加。
    /// スラッシュで区切ることで子メニューに出来る。
    /// 第三引数で順番を制御出来る。
    /// 第二引数はあまり使わない。
    /// </summary>
    [MenuItem("test/Prefab作成", false, 1)]
    public static void ShowThisWindow()
    {
        GetWindow<PrefabListCreateWindow>("Prefab作成");
    }

    /// <summary>
    /// ウィンドウが開かれたタイミングで実行。
    /// </summary>
    private void OnEnable()
    {
        list = AssetDatabase.LoadAssetAtPath<PrefabList>(PATH);
    }

    /// <summary>
    /// ウィンドウが選択されたタイミングで実行。
    /// </summary>
    private void OnFocus()
    {
        
    }

    /// <summary>
    /// ウィンドウの描画の度に実行。
    /// 必ず毎フレーム実行されるわけではなく、非アクティブだったり、操作がない場合は実行されない。
    /// </summary>
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("作成"))
            {
                Create();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void Create()
    {
        if (list != null) { return; }

        // scriptableObjectを作成。
        list = ScriptableObject.CreateInstance<PrefabList>();

        // 保存。
        // AssetDatabaseはUnityEditorのネームスペースなので、Editor以外で使用する場合には注意。
        AssetDatabase.CreateAsset(list, PATH);
    }
}
