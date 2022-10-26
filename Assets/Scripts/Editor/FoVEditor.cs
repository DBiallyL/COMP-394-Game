using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyView))]
public class FoVEditor : Editor
{
    void onSceneGUI() {
        EnemyView fow = (EnemyView) target;
        Handles.color = Color.white;
        // Handles.DrawWireArc(fow.transform.position);
    }
}
