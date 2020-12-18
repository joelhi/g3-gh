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
            int numC = ms.VertexColors.Count;

            List<Vector3f> Vertices = new List<Vector3f>(numV);
            List<Vector3i> Triangles = new List<Vector3i>(numF);

            for (int i = 0; i < numV; i++)
                Vertices.Add(ms.Vertices[i].ToVec3f());

            for (int i = 0; i < numF; i++)
                Triangles.Add(ms.Faces[i].ToVec3i());

            DMesh3 dMs = DMesh3Builder.Build<Vector3f,Vector3i,Vector3f>(Vertices, Triangles);

            if(numV == numC)
            {
                dMs.EnableVertexColors(new Vector3f(0.5, 0.5, 0.5));

                for (int i = 0; i < numV; i++)
                    dMs.SetVertexColor(i, new Vector3f((float)ms.VertexColors[i].R / 255, (float)ms.VertexColors[i].G / 255, (float)ms.VertexColors[i].B / 255));
            }

            return dMs;
        }

        public static Rhino.Geometry.Mesh ToRhino(this DMesh3 dMesh3)
        {
            Rhino.Geometry.Mesh rhMs = new Rhino.Geometry.Mesh();
            List<Rhino.Geometry.MeshFace> rhFaces = new List<Rhino.Geometry.MeshFace>();
            List<Rhino.Geometry.Point3f> rhVertices = new List<Rhino.Geometry.Point3f>();

            DMesh3 copy;
            if (!dMesh3.IsCompact)
                copy = new DMesh3(dMesh3, true);
            else
                copy = dMesh3;

            foreach (var tri in copy.Triangles())
                rhMs.Faces.AddFace(new Rhino.Geometry.MeshFace(tri.a, tri.b, tri.c));
            foreach (var vert in copy.Vertices())
                rhMs.Vertices.Add((float)vert.x, (float)vert.y, (float)vert.z);
            foreach (int ind in copy.VertexIndices())
            {
                Vector3f col = copy.GetVertexColor(ind);
                rhMs.VertexColors.Add((int)(col.x * 255), (int)(col.y * 255), (int)(col.z * 255));
            }

            return rhMs;
        }

        public static Rhino.Geometry.Point3d[] ToRhinoPts(this DenseGridTrilinearImplicit Value)
        {
            int dimX = Value.Grid.ni;
            int dimY = Value.Grid.nj;
            int dimZ = Value.Grid.nk;

            int counter = 0;

            Rhino.Geometry.Point3d origin = Value.GridOrigin.ToRhinoPt();
            double size = Value.CellSize;

            Rhino.Geometry.Point3d[] Pts = new Rhino.Geometry.Point3d[dimX * dimY * dimZ];

            for (int k = 0; k < dimZ; k++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    for (int i = 0; i < dimX; i++)
                    {
                        Pts[counter] = origin + new Rhino.Geometry.Point3d(i * size, j * size, k * size);
                        counter++;
                    }
                }
            }

            return Pts;
        }

        public static Vector3f ToVec3f(this Rhino.Geometry.Point3f rhPt)
        {
            return new Vector3f(rhPt.X,rhPt.Y,rhPt.Z);
        }

        public static Vector3d ToVec3d(this Rhino.Geometry.Point3d pt3d)
        {
            return new Vector3d(pt3d.X, pt3d.Y, pt3d.Z);
        }

        public static Vector3d ToVec3d(this Rhino.Geometry.Vector3d vec3d)
        {
            return new Vector3d(vec3d.X, vec3d.Y, vec3d.Z);
        }

        public static Rhino.Geometry.Point3d ToRhinoPt(this Vector3d vec3d)
        {
            return new Rhino.Geometry.Point3d(vec3d.x, vec3d.y, vec3d.z);
        }

        public static Vector3i ToVec3i(this Rhino.Geometry.MeshFace rhMeshFace)
        {
            if (rhMeshFace.IsTriangle)
                return new Vector3i(rhMeshFace.A, rhMeshFace.B, rhMeshFace.C);
            else
                throw new Exception("The mesh face needs to be a triangle.");  
        }

        public static Rhino.Geometry.BoundingBox ToRhino(this AxisAlignedBox3d bx)
        {
            return new Rhino.Geometry.BoundingBox(bx.Min.x, bx.Min.y, bx.Min.z, bx.Max.x, bx.Max.y, bx.Max.z);
        }
    }
}
