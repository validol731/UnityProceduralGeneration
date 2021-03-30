using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapPreview))]
public class MapPreviewEditor : Editor
{
    [System.Obsolete]
    public override void OnInspectorGUI()
    {
        MapPreview mapPreview = (MapPreview)target;


        if (DrawDefaultInspector())
        {
            if (mapPreview.autoUpdate)
            {
                mapPreview.DrawMapInEditor();
            }
        }
        if (GUILayout.Button("Generate"))
        {
            mapPreview.DrawMapInEditor();
        }
    }
}
