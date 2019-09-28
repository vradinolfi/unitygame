using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtree
{
    class Ao
    {
        public static void BakeAo(ref Color[] colors, Vector3[] verts, Vector3[] normals, int[] branchTri, int[] leafTri, GameObject ob, int samples, float distance)
        {
            MeshCollider collider = ob.GetComponent<MeshCollider>();
            if (collider == null)
                collider = ob.AddComponent<MeshCollider>();

            Mesh obstacle = new Mesh(); //Tree mesh used for collision detection

            int n = branchTri.Length;
            int m = leafTri.Length;
            int[] triangles = new int[n + 2 * m]; //counting twice the leafs in order to make the collider double sided
            int k = triangles.Length;
            for (int i=0; i<k; i += 3)
            {
                if(i < n)
                {
                    triangles[i] = branchTri[i];
                    triangles[i+1] = branchTri[i+1];
                    triangles[i+2] = branchTri[i+2];
                }

                else if (i >= n && i < n+m)
                {
                    triangles[i] = leafTri[i - n];
                    triangles[i + 1] = leafTri[i - n + 1];
                    triangles[i + 2] = leafTri[i - n + 2];
                }

                else //inversing triangles to have normals pointing backawards
                {
                    triangles[i] = leafTri[i - (n + m)];
                    triangles[i + 1] = leafTri[i - (n + m) + 2];
                    triangles[i + 2] = leafTri[i - (n + m) + 1];
                }
            }


            obstacle.vertices = verts;
            obstacle.normals = normals;
            obstacle.triangles = triangles;
            obstacle.RecalculateBounds();
            collider.sharedMesh = obstacle;

            Transform transform = ob.transform;


            n = verts.Length;
            for(int i=0; i<n; i++)
            {
                Vector3 pos = transform.TransformPoint(verts[i]);
                float ao = 0;
                Vector3 nrm = transform.TransformVector(normals[i]);
                for (int sample=0; sample<samples; sample++)
                {
                    Vector3 dir = Random.onUnitSphere + nrm;
                    RaycastHit hit;
                    if(Physics.Linecast(pos + nrm*.1f, pos + dir.normalized * distance, out hit))
                    {
                        ao += 1 - Mathf.Pow(hit.distance / distance, 10f);
                    }
                }
                ao = 1 - ao/samples;
                colors[i].a = ao;
            }
        }
    }
}
