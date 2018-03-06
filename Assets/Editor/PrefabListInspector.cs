using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabList))]
public class PrefabListInspector : Editor {

    private PrefabList list;

    private void OnEnable()
    {
        list = (PrefabList)target;
        if (list.List == null)
        {
            list.List = new List<GameObject>();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var isDirty = false;

        EditorGUILayout.BeginHorizontal();
        {
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
        EditorGUILayout.EndHorizontal();

        if (isDirty)
        {
            EditorUtility.SetDirty(list);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
