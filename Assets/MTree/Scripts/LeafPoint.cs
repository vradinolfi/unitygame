using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtree
{
    public struct LeafPoint
    {
        public Vector3 position;
        public Vector3 direction;
        public float size;
        public Mesh[] mesh;
        public bool overrideNormals;
        public float distanceFromOrigin; 


        public LeafPoint(Vector3 pos, Vector3 dir, float size, Mesh[] m, bool overrideNorm, float distanceFromOrigin)
        {
            position = pos;
            direction = dir;
            this.size = size;
            mesh = m;
            overrideNormals = overrideNorm;
            this.distanceFromOrigin = distanceFromOrigin;
        }
    }

    public struct LeafCandidate
    {
        public Vector3 position;
        public Vector3 direction;
        public float size;
        public float parentBranchLength;
        public float distanceFromOrigin;
        public bool isEnd;


        public LeafCandidate(Vector3 pos, Vector3 dir, float size, float branchLength, float distanceFromOrigin, bool isEnd)
        {
            position = pos;
            direction = dir;
            this.size = size;
            parentBranchLength = branchLength;
            this.distanceFromOrigin = distanceFromOrigin; ;
            this.isEnd = isEnd;
        }
    }
}
