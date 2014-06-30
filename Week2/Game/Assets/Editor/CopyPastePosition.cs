using UnityEditor;
using UnityEngine;

public class CopyPastePosition : EditorWindow
{
    Vector3 Position = new Vector3();
    Vector3 Rotation = new Vector3();
    Vector3 Scale = new Vector3();
    //string nameOfLastObject = "";

    bool pos, rot, scale;
    bool local;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/CopyPastePosition")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CopyPastePosition));
    }

    void OnGUI()
    {
        //GUILayout.Label(nameOfLastObject, EditorStyles.boldLabel);
        EditorGUILayout.Vector3Field("Position", Position);
        EditorGUILayout.Vector3Field("Rotation", Rotation);
        EditorGUILayout.Vector3Field("Scale", Scale);


        pos = EditorGUILayout.Toggle("Position", pos);
        rot = EditorGUILayout.Toggle("Rotation", rot);
        scale = EditorGUILayout.Toggle("Scale", scale);
        local = EditorGUILayout.Toggle("Use local coordinates", local);

        if (GUILayout.Button("Copy"))
            CopyPosition();

        if (GUILayout.Button("Paste"))
            PastePosition();

        /*if (GUILayout.Button("***Reset All***"))
            ResetAll();*/
    }

    void CopyPosition()
    {
        var obj = Selection.activeGameObject;
        
        if (obj == null)
            return;

        //nameOfLastObject = obj.name;

        if (local)
        {
            if (pos)
                Position = obj.transform.localPosition;
            if (rot)
                Rotation = obj.transform.localEulerAngles;
            if (scale)
                Scale = obj.transform.localScale;
        }
        else // global
        {
            if (pos)
                Position = obj.transform.position;
            if (rot)
                Rotation = obj.transform.eulerAngles;
            if (scale)
                Scale = obj.transform.localScale;
        }
    }

    void PastePosition()
    {
        var obj = Selection.activeGameObject;

        if (obj == null)
            return;

        if (local)
        {
            if (scale && Scale != null)
                obj.transform.localScale = Scale;
            if (rot && Rotation != null)
                obj.transform.localEulerAngles = Rotation;
            if (pos && Position != null)
                obj.transform.localPosition = Position;
        }
        else // global
        {
            if (scale && Scale != null)
                obj.transform.localScale = Scale;
            if (rot && Rotation != null)
                obj.transform.eulerAngles = Rotation;
            if (pos && Position != null)
                obj.transform.position = Position;
        }
    }

    void ResetAll()
    {
        Position = Vector3.zero;
        Rotation = Vector3.zero;
        Scale = Vector3.zero;
    }

    
}