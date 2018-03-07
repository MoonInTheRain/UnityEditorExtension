using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stage))]
public class StageInspector : Editor {

    private Stage stage;

    /// <summary>
    /// インスペクターが開かれるタイミングで実行
    /// </summary>
    private void OnEnable()
    {
        stage = (Stage)target;
    }

    /// <summary>
    /// インスペクターが開かれている時の、シーン描画中に実行。
    /// 必ず毎フレーム実行されるわけではなく、操作をしなければ実行されない。
    /// </summary>
    private void OnSceneGUI()
    {
        // この操作のIDを取得している（はず）
        int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

        Event current = Event.current;

        EventType eventType = current.GetTypeForControl(controlID);

        switch (eventType)
        {
            case EventType.MouseDown:
            case EventType.MouseDrag:
                if (current.button == 0)
                {
                    // hotControlを固定して、他の操作が起こらないようにする。
                    // キーボードの時はkeyboardControl
                    GUIUtility.hotControl = controlID;

                    OnClick(current.mousePosition);

                    // 通常の操作が行われない用に、イベントを使用済みにする。
                    current.Use();
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    // コントロールを開放する。
                    GUIUtility.hotControl = 0;

                    // 通常の操作が行われない用に、イベントを使用済みにする。
                    current.Use();
                }
                break;
        }
    }

    /// <summary>
    /// クリック時の動作
    /// </summary>
    /// <param name="mousePos"></param>
    private void OnClick(Vector3 mousePos)
    {
        // マウスのクリック地点から、Stageの表面の座標を算出します。
        var posTmp = RayToPoint(HandleUtility.GUIPointToWorldRay(mousePos));
        if (posTmp == null) { return; }

        var pos = posTmp ?? Vector3.zero;

        // 座標をグリッドに合わせるようにした。なんとなく。
        pos.x = Mathf.RoundToInt(pos.x - 0.5f) + 0.5f;
        pos.z = Mathf.RoundToInt(pos.z - 0.5f) + 0.5f;

        var prefab = stage.GetRandomPrefab();

        if (prefab == null) { return; }

        // オブジェクトを作成＆場所調整
        var cube = Instantiate(prefab);
        cube.transform.position = pos;

        // Undoでオブジェクトを削除出来るようにする。
        Undo.RegisterCreatedObjectUndo(cube, "キューブ作成");
    }

    /// <summary>
    /// マウス位置をシーンのワールド座標に変換
    /// </summary>
    /// <param name="ray">レイ</param>
    /// <param name="pos">マウス位置</param>
    public Vector3? RayToPoint(Ray ray)
    {
        var mesh = stage.GetComponent<MeshCollider>();
        if(mesh == null) { return null; }

        RaycastHit info;

        if (Physics.Raycast(ray, out info))
        {
            if(info.collider == stage.GetComponent<Collider>())
            {
                return info.point;
            }
        }

        return null;
    }
}
