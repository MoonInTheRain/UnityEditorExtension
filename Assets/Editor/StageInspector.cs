using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stage))]
public class StageInspector : Editor {

    private Stage stage;

    private void OnEnable()
    {
        stage = (Stage)target;
    }

    private void OnSceneGUI()
    {
        int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

        Event current = Event.current;

        EventType eventType = current.GetTypeForControl(controlID);

        if (eventType == EventType.MouseUp)
        {
            if (GUIUtility.hotControl == controlID)
            {
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl = 0;

                current.Use();
                return;
            }
        }

        var posTmp = RayToPoint(HandleUtility.GUIPointToWorldRay(current.mousePosition));
        if (posTmp == null) { return; }

        var pos = posTmp ?? Vector3.zero;

        pos.x = Mathf.RoundToInt(pos.x - 0.5f) + 0.5f;
        pos.z = Mathf.RoundToInt(pos.z - 0.5f) + 0.5f;

        switch (eventType)
        {
            case EventType.MouseDown:
            case EventType.MouseDrag:
                if (current.button == 0)
                {
                    GUIUtility.hotControl = controlID;

                    var prefab = stage.GetRandomPrefab();

                    if(prefab == null) { return; }

                    var cube = Instantiate(prefab);
                    cube.transform.position = pos;

                    Undo.RegisterCreatedObjectUndo(cube, "キューブ作成");

                    current.Use();
                }
                break;
            case EventType.MouseMove:
                {
                    current.Use();
                }
                break;
        }
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
