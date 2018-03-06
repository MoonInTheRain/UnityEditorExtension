using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabList))]
public class PrefabListInspector : Editor {

    private PrefabList list;

    /// <summary>
    /// インスペクターが開かれる度に実行
    /// </summary>
    private void OnEnable()
    {
        list = (PrefabList)target;
        if (list.List == null)
        {
            list.List = new List<GameObject>();
        }
    }

    /// <summary>
    /// インスペクターを描画するタイミングで実行。
    /// 必ず毎フレーム実行されるわけではなく、操作をしなければ実行されない。
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 変更があったかどうか
        var isDirty = false;

        // 横に並べる。
        EditorGUILayout.BeginHorizontal();
        {
            // ボタン描画。クリックした際にTrueが変える。
            if (GUILayout.Button("項目追加"))
            {
                isDirty = true;
                list.List.Add(null);
            }

            if (GUILayout.Button("項目削除"))
            {
                var index = list.List.Count - 1;
                if(index < 0) { return; }

                isDirty = true;
                list.List.RemoveAt(index);
            }
        }
        // 横に並べるのを終了。
        EditorGUILayout.EndHorizontal();

        if (isDirty)
        {
            // スクリプト上でScriptableObjectを変更した場合、下記の処理をしないと、Unity終了後に元に戻る。
            EditorUtility.SetDirty(list);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
