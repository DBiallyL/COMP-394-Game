using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coded with help from: https://www.youtube.com/watch?v=rQG9aUWarwE&list=PLFt_AvWsXl0dohbtVgHDNmgZV_UY7xZv7 

public class EnemyView : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;

    public Vector3 DirecFromAngle(float angle) {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    // Start is called before the first frame update
    void Start()
    {
        // Mesh mesh = new Mesh();
        // GetComponent<MeshFilter>().mesh = mesh;

        // Vector3 origin = Vector3.zero;
        // float fov = 90f;
        // int rayCount = 2;
        // float angle = 0f;
        // float angleIncrease = fov / rayCount;
        // float viewDistance = 50f;

        // Vector3[] vertices = new Vector3[rayCount + 2];
        // Vector2[] uv = new Vector2[vertices.Length];
        // int[] triangles = new int[rayCount * 3];

        // vertices[0] = origin;

        // for (int i = 0; i <= rayCount; i++) {

        // }

        // mesh.vertices = vertices;
        // mesh.uv = uv;
        // mesh.triangles = triangles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
