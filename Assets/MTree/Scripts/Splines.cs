﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtree
{
    public class Splines
    {
        Stack<Queue<TreePoint>> splines;
        public Queue<Vector3> verts;
        public Queue<Vector2> uvs;
        public Queue<Vector3> normals;
        public Queue<int> triangles;
        public Queue<Color> colors;

        public Splines(Stack<Queue<TreePoint>> points)
        {
            splines = points;
            verts = new Queue<Vector3>();
            uvs = new Queue<Vector2>();
            normals = new Queue<Vector3>();
            triangles = new Queue<int>();
            colors = new Queue<Color>();
        }

        public void GenerateMeshData(float resolutionMultiplier, int minResolution, AnimationCurve rootShape, float radiusMultiplier
                                    , float rootRadius, float rootHeight, float RootResolutionMultiplier, int flaresNumber, float displacmentStrength
                                    , float displacmentSize, float spinAmount)
        {
            Queue<int> trianglesWithBadNormals = new Queue<int>(); // Triangles whose verts have been displaced, needing their normals to be recalculated
            Queue<Vector2Int> duplicatedVertexIndexes = new Queue<Vector2Int>(); // Used when recalculating normals to avoid shading seams
            while (splines.Count > 0) // Each spline is an entire branch
            {
                SimplexNoiseGenerator noiseGenerator = new SimplexNoiseGenerator(new int[] { 0x16, 0x38, 0x32, 0x2c, 0x0d, 0x13, 0x07, 0x2a });
                Queue<TreePoint> spline = splines.Pop();
                int lastResolution = -1;
                float uv_height = 0f;
                while (spline.Count > 0) // drawing each node inside the branch
                {
                    TreePoint point = spline.Dequeue();
                    point.radius = Mathf.Max(point.radius, 0.001f);
                    int resolution = getResolution(point, minResolution, rootRadius, rootHeight, resolutionMultiplier);

                    Vector3[] newVerts;

                    if (lastResolution == -1 && point.parentRadius != 0 && point.type == NodeType.FromTrunk)
                    {
                        newVerts = AddCircleWrapped(point.position, point.direction, point.radius, resolution, point.parentRadius, point.parentDirection, displacmentStrength);
                    }
                    else
                    {
                        newVerts = AddCircle(point.position, point.direction, point.radius, resolution, uv_height * spinAmount * point.radius);
                    }
                    int n = verts.Count;
                    duplicatedVertexIndexes.Enqueue(new Vector2Int(n, n + resolution - 1));
                    BridgeLoops(newVerts, point, lastResolution, uv_height, n, getFillingGapRate(lastResolution, resolution), flaresNumber, rootRadius, rootHeight, radiusMultiplier,
                                rootShape, noiseGenerator, displacmentSize, displacmentStrength, trianglesWithBadNormals);
                    if (spline.Count > 0)
                    {
                        float rad = point.radius;
                        if (point.type == NodeType.Flare)
                            rad += rootRadius * rootShape.Evaluate(point.position.y / rootHeight) * radiusMultiplier;
                        uv_height += (point.position - spline.Peek().position).magnitude / rad;
                    }

                    lastResolution = resolution;
                }
            }
            RecalculateNormals(duplicatedVertexIndexes, trianglesWithBadNormals);
        }

        void BridgeLoops(Vector3[] loop, TreePoint point, int lastResolution, float uvHeight, int n, int fillingGapRate, int flaresNumber,
                         float rootRadius, float rootHeight, float radiusMultiplier, AnimationCurve rootShape, SimplexNoiseGenerator noiseGenerator,
                         float displacementSize, float displacementStrength, Queue<int> trianglesWithBadNormals)
        {
            int resolution = loop.Length;
            int gaps = lastResolution - resolution; //difference between the two loops
            for (int i = 0; i < resolution; i++)
            {
                Vector3 vert = loop[i];
                Vector3 normal = (vert - point.position).normalized;

                Vector3 nrm = (vert - point.position).normalized;
                float uvOffset = 0f;
                if (lastResolution == -1)
                {
                    Vector3 orthoDir = Vector3.ProjectOnPlane(point.direction, point.parentDirection).normalized;
                    Vector3 center = (loop[0] + loop[resolution / 2]) / 2 - orthoDir * point.parentRadius;
                    Vector3 pos = Vector3.Project(vert - center, point.parentDirection) + center;
                    nrm = (vert - pos).normalized;
                    if (point.type == NodeType.FromTrunk)
                    {
                        uvOffset = point.parentRadius / Mathf.Max(3f, 1- Mathf.Abs(Vector3.Dot(point.direction, point.parentDirection)));
                        uvOffset = Vector3.Distance(point.position, vert);
                    }
                }
                bool badNormals = false;
                if (point.type == NodeType.Trunk || point.type == NodeType.Flare) //Trunk displacement
                {
                    vert += noiseGenerator.noiseGradient(vert * displacementSize / radiusMultiplier, flat: true) / 5 * point.radius * displacementStrength;
                    badNormals = true;
                }

                if (point.type == NodeType.Flare) // root flares displacement
                {
                    float angle = i * 1f / (resolution - 1) * 2 * Mathf.PI;
                    float displacement = Mathf.Abs(Mathf.Sin(angle * flaresNumber / 2f)) * rootRadius * rootShape.Evaluate(point.position.y / rootHeight) * radiusMultiplier;
                    vert += normal * displacement;
                }

                verts.Enqueue(vert);
                normals.Enqueue(nrm);
                uvs.Enqueue(new Vector2(i * 1f / (resolution - 1), uvHeight / 3.2f + uvOffset));
                if (i > 0 && lastResolution > 0)
                {
                    if (badNormals)
                    {
                        trianglesWithBadNormals.Enqueue(triangles.Count);
                        trianglesWithBadNormals.Enqueue(triangles.Count + 3);
                    }
                    AddTriangle(n - lastResolution + i - 1, n + i - 1, n - lastResolution + i);
                    AddTriangle(n + i - 1, n + i, n - lastResolution + i);
                    if (i % fillingGapRate == 0 && gaps > 0) // filling a gap
                    {
                        if (badNormals)
                            trianglesWithBadNormals.Enqueue(triangles.Count);
                        AddTriangle(n - lastResolution + i, n + i, n - lastResolution + i + 1);
                        gaps--;
                        lastResolution--;
                    }
                }
                Color col = new Color(point.distanceFromOrigin / 10, 0, 0);
                colors.Enqueue(col);
            }
            if (gaps > 0) // Fill eventual remaining gap
                AddTriangle(n - lastResolution + resolution - 1, n + resolution - 1, n - lastResolution + resolution);

        }

        int getFillingGapRate(int lastResolution, int resolution)
        {
            int gaps = lastResolution - resolution; //difference between the two loops
            int fillingGapRate = int.MaxValue; //rate at which an additional triangle must be created
            if (gaps > resolution)
                resolution = gaps; // increase resolution when there are too many gaps to fill
            if (gaps > 0)
                fillingGapRate = resolution / gaps;

            return fillingGapRate;
        }

        int getResolution(TreePoint point, int minResolution, float rootRadius, float rootHeight, float resolutionMultiplier)
        {
            int resolution = (int)((point.radius) * resolutionMultiplier * 7);

            if (point.type == NodeType.Flare)
                resolution += (int)(rootRadius * (Mathf.Pow(1 - Mathf.Max(0, point.position.y / rootHeight), 2)) * resolutionMultiplier * 3);

            if (resolution < minResolution)
                resolution = minResolution;
            resolution++;
            return resolution;
        }

        public void RecalculateNormals(Queue<Vector2Int> duplicatedVerts, Queue<int> selectedTriangles)
        {
            Vector3[] newNormals = normals.ToArray();
            HashSet<int> overidenIndexes = new HashSet<int>();
            Vector3[] Verts = verts.ToArray();
            int[] Tris = triangles.ToArray();
            int n = Tris.Length;
            foreach (int i in selectedTriangles)
            {
                int a = Tris[i];
                int b = Tris[i+1];
                int c = Tris[i+2];

                if (!overidenIndexes.Contains(a))
                {
                    newNormals[a] = Vector3.zero;
                    overidenIndexes.Add(a);
                }
                if (!overidenIndexes.Contains(b))
                {
                    newNormals[b] = Vector3.zero;
                    overidenIndexes.Add(b);
                }
                if (!overidenIndexes.Contains(c))
                {
                    newNormals[c] = Vector3.zero;
                    overidenIndexes.Add(c);
                }

                Vector3 nrm = Vector3.Cross(Verts[b] - Verts[a], Verts[c] - Verts[a]);

                newNormals[a] += nrm;
                newNormals[b] += nrm;
                newNormals[c] += nrm;
            }
            
            foreach(int i in overidenIndexes)
            {
                newNormals[i].Normalize();
            }

            foreach (Vector2Int indexes in duplicatedVerts)
            {
                int x = indexes.x;
                int y = indexes.y;
                Vector3 nrm = (newNormals[x] + newNormals[y]) / 2;
                newNormals[x] = newNormals[y] = nrm;
            }

            normals = new Queue<Vector3>(newNormals);
        }

        public static Vector3[] AddCircle(Vector3 position, Vector3 direction, float radius, int resolution, float spinAngle)
        {
            Vector3[] verts = new Vector3[resolution];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
            for (int i = 0; i < resolution; i++)
            {
                float angle = Mathf.PI * 2 * ((float)i / (resolution - 1)) + spinAngle;
                Vector3 vert = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                verts[i] = rotation * vert + position;
            }
            return verts;
        }


        public static Vector3[] AddCircleWrapped(Vector3 position, Vector3 direction, float radius, int resolution, float parentRadius, Vector3 parentDirection, float displcementStrength)
        {
            Vector3[] verts = new Vector3[resolution];
            Vector3 orthoDir = Vector3.ProjectOnPlane(direction, parentDirection).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, orthoDir);
            Vector3 tangent = Vector3.Cross(parentDirection, orthoDir);
            float scale = Mathf.Sqrt(Mathf.Pow(Mathf.Tan(90-Vector3.Angle(direction, parentDirection) * Mathf.PI / 180), 2) + 1);
            scale = Mathf.Min(scale, 3f) / 4;
            Matrix4x4 scaleMat = Matrix4x4.identity;
            scaleMat[0, 0] += parentDirection.x * scale;
            scaleMat[1, 1] += parentDirection.y * scale;
            scaleMat[2, 2] += parentDirection.z * scale;
            for (int i = 0; i < resolution; i++)
            {
                float angle = Mathf.PI * 2 * ((float)i / (resolution - 1));
                Vector3 vert = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                vert = (rotation * vert);
                float wrap = Mathf.Sin(Mathf.Acos(Mathf.Clamp01(Mathf.Abs(Vector3.Dot(vert, tangent)) / parentRadius))) * parentRadius * Mathf.Exp(-Mathf.Abs(displcementStrength/5) - 0.2f);
                vert = scaleMat.MultiplyPoint(vert);
                vert += orthoDir * wrap;
                vert += position + parentDirection * Vector3.Dot(direction, parentDirection) * parentRadius;
                verts[i] = vert;
            }
            return verts;
        }


        public void AddTriangle(int a, int b, int c)
        {
            triangles.Enqueue(a);
            triangles.Enqueue(b);
            triangles.Enqueue(c);
        }
    }

    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
