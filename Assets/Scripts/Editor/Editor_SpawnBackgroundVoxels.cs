using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnBackgroundVoxels))]
public class Editor_SpawnBackgroundVoxels : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnBackgroundVoxels myScript = (SpawnBackgroundVoxels)target;

        if (GUILayout.Button("Create Background Voxels"))
        {
            //myScript.BuildPathingInBox();
            //List<Waypoint> way = new List<Waypoint>(FindObjectsOfType<Waypoint>());
            //foreach (var w in way) EditorUtility.SetDirty(w);
            //EditorUtility.SetDirty(this);
        }
    }
}
