using System;
using System.Collections.Generic;
using g3;

namespace gh3sharp.Core
{
    public static class Conversions
    {
        public static DMesh3 ToDMesh3(this Rhino.Geometry.Mesh ms)
        {

            int numV = ms.Vertices.Count;
            int numF = ms.Faces.Count;

            List<Vector3f> Vertices = new List<Vector3f>(numV);
            List<Vector3i> Triangles = new List<Vector3i>(numF);

            for (int i = 0; i < numV; i++)
                Vertices.Add(ms.Vertices[i].ToVec3f());

            for (int i = 0; i < numF; i++)
                Triangles.Add(ms.Faces[i].ToVec3i());

            DMesh3 dMs = DMesh3Builder.Build<Vector3f,Vector3i,Vector3f>(Vertices, Triangles);
            return dMs;
        }

        public static Rhino.Geometry.Mesh ToRhino(this DMesh3 dMesh3)
        {
            Rhino.Geometry.Mesh rhMs = new Rhino.Geometry.Mesh();
            List<Rhino.Geometry.MeshFace> rhFaces = new List<Rhino.Geometry.MeshFace>();
            List<Rhino.Geometry.Point3f> rhVertices = new List<Rhino.Geometry.Point3f>();

            foreach (var tri in dMesh3.Triangles())
                rhMs.Faces.AddFace(new Rhino.Geometry.MeshFace(tri.a, tri.b, tri.c));
            foreach (var vert in dMesh3.Vertices())
                rhMs.Vertices.Add((float)vert.x, (float)vert.y, (float)vert.z);

            return rhMs;

        }
        public static Vector3f ToVec3f(this Rhino.Geometry.Point3f rhPt)
        {
            return new Vector3f(rhPt.X,rhPt.Y,rhPt.Z);
        }

        public static Vector3i ToVec3i(this Rhino.Geometry.MeshFace rhMeshFace)
        {
            if (rhMeshFace.IsTriangle)
                return new Vector3i(rhMeshFace.A, rhMeshFace.B, rhMeshFace.C);
            else
                throw new Exception("The mesh face needs to be a triangle.");
                    
        }
    }
}
