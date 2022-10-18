using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FoVEditor : MonoBehaviour
{
    void onSceneGUI() {
        FieldOfView fow = (FieldOfView) target;
        Handles.color = Color.White;
        // Handles.DrawWireArc(fow.transform.position);
    }
}
