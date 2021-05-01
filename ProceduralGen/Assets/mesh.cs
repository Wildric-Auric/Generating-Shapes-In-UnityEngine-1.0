using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mesh : MonoBehaviour
{
    [SerializeField] bool update = true;
    public Material mat;
    public float Xsize = 1f;
    public float Ysize = 1f;
    public int precision = 1;

    [HideInInspector]public bool sphere;
    [Min(0), HideInInspector]public float radius = 1f;
    [HideInInspector] public Transform sphereCenter;

    public Vector3 initialPos;
    public Quaternion initialRotation;

    Vector2[] texture;
    private bool isSphere;

    void OnValidate()
    {
        if (update)
        {
            Create();
        }
    }
    public void Create()
    {
        transform.position = initialPos;
        transform.rotation = initialRotation;

        MeshFilter MF = GetComponent<MeshFilter>();
        MeshRenderer MR = GetComponent<MeshRenderer>();
        MR.material = mat;
        Mesh mesh = MF.mesh;
        mesh.Clear();

        float smallX = (Xsize / precision);
        float smallY = Ysize / precision;
        int Num = (precision * precision);
        //Set up vertices
        Vector3[] vertices = new Vector3[(precision + 1) * (precision + 1)];
        Vector3[] normals = new Vector3[(precision + 1) * (precision + 1)];
        for (int j = 0, temp = 0; j <= precision; j++)
        {
            for (int i = 0; i <= precision; i++)
            {
                vertices[temp] = new Vector3(i * smallX, 0, j * smallY);
                temp++;

            }
        }
        //Set up triangles
        int[] triangles = new int[6 * Num];
        for (int j = 0, count = 0, vert = 0; j < precision; j++)
        {
            for (int i = 0; i < precision; i++)
            {
                triangles[count] = vert;
                triangles[count + 1] = vert + precision + 1;
                triangles[count + 2] = vert + 1;
                triangles[count + 3] = vert + 1;
                triangles[count + 4] = vert + precision + 1;
                triangles[count + 5] = vert + precision + 2;
                count += 6;
                vert++;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    public void spherize()
    {
        if (sphereCenter != null)
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] sphereCoord = new Vector3[mesh.vertices.Length];
            for (int temp = 0; temp < mesh.vertices.Length; temp++)
            {
                //transform.TransformPoint(mesh.vertices[temp]
                sphereCoord[temp] = sphereCenter.position + radius * (transform.localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[temp]) - sphereCenter.position).normalized;
            }
            mesh.vertices = sphereCoord;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}