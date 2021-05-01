using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class sphere : MonoBehaviour
{
    [SerializeField] bool spherize;
    [SerializeField] float radius =1f;
    [Min(1)][SerializeField] int precision =1;
    [SerializeField] Material mat;
    Vector4[] facePos = { new Vector4(-0.5F, -0.5f, -0.5f, 90), new Vector4(0.5f, 0.5f, -0.5f, -90), new Vector4(-0.5F, 0.5F, -0.5F, 0f), new Vector4(0.5f, -0.5f, -0.5f, -180), new Vector4(-0.5f, -0.5f, -0.5f, -90), new Vector4( -0.5F,0.5F, 0.5F, 90) };
    private void OnValidate()
    {
        if (transform.position !=Vector3.zero)
        {
            transform.position = Vector3.zero;
        }
        int i = 0;
        while (transform.childCount < 6)
        {
            
            GameObject face = new GameObject("Face"+transform.childCount.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(mesh));
            mesh mesh= face.GetComponent<mesh>();
            mesh.mat = this.mat;
            mesh.precision = this.precision;
            // MF = face.GetComponent<MeshFilter>();
            face.transform.SetParent(this.transform);
            var c = facePos[i];
            face.transform.position = transform.position + new Vector3(c.x, c.y, c.z);
            face.transform.rotation = Quaternion.Euler(c.w * (Convert.ToInt16(i > 3)), 0, c.w * (Convert.ToInt16(i <= 3)));
            mesh.initialPos = face.transform.position;
            mesh.initialRotation = face.transform.rotation;
            i++;
        }
     
        foreach (Transform face in transform)
        {
            mesh mesh = face.GetComponent<mesh>();

            mesh.precision = this.precision;
            mesh.radius = this.radius;
            mesh.mat = this.mat;

            mesh.Create();
            if (spherize)
            {
                mesh.sphere = true;
                mesh.sphereCenter = this.transform;
                mesh.spherize();
            }
            else mesh.sphere = false;
        }

    }
}
