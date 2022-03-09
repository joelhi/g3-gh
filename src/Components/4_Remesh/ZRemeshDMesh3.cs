using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Components.Params;
using g3gh.Core.Goos;

using g3;
using gs;
using System.Linq;

namespace g3gh.Components.Remesh
{
    public class ZombieRemeshDMesh3 : GH_Component
    {

        public ZombieRemeshDMesh3()
          : base("Remesh [Zombie]", "Nickname",
            "Remesh a DMesh3 object. Same as other remesher, only this one does iterations internally.",g3ghUtil.pluginName
            , "4_Remesh")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to remesh", GH_ParamAccess.item);
            pManager.AddNumberParameter("Target Edge Length", "len", "Target edge length for remeshing", GH_ParamAccess.item);
            pManager.AddPointParameter("Point Constraints", "pts", "Points on mesh to constrain", GH_ParamAccess.list);
            
            pManager.AddIntegerParameter("Constrain Boundaries", "c", "Option to constrain the boundary edges during the remeshing procedure\n0 = No Constraint\n1 = Sliding\n2 = Fixed", GH_ParamAccess.item, 0);
            pManager.AddParameter(new EdgeConstraint_Param(), "Custom Edge Constraints", "eC", "Custom edge constrraints which may or may not be on the boundary.\nGenerate from Loops or Spans", GH_ParamAccess.list); ;
            pManager.AddBooleanParameter("Project to Input", "p", "Project the remeshed result back to the input mesh", GH_ParamAccess.item, false);

            pManager.AddIntegerParameter("Number of Iterations", "iter", "Number of Iterations for the remeshing process", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Smoothing Speed", "s", "Smooth speed between iterations", GH_ParamAccess.item, 0.5);

            pManager[2].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "New mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            List<Point3d> points = new List<Point3d>();
            double targetL = 0;
            int numI = 0;
            int fixB = 0;
            bool projBack = false;
            double smooth = 0;

            DA.GetData(0, ref dMsh_goo);
            DA.GetDataList(2, points);
            DA.GetData(1, ref targetL);
            DA.GetData(6, ref numI);
            DA.GetData(3, ref fixB);
            DA.GetData(5, ref projBack);
            DA.GetData(7, ref smooth);

            List<EdgeConstraint_goo> edgeC = new List<EdgeConstraint_goo>();

            DA.GetDataList(6, edgeC);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);

            Remesher r = new Remesher(dMsh_copy);
            r.PreventNormalFlips = true;
            r.SetTargetEdgeLength(targetL);
            r.SmoothSpeedT = smooth;


            if (fixB == 2)
                MeshConstraintUtil.FixAllBoundaryEdges(r);
            else if (fixB == 1)
                MeshConstraintUtil.PreserveBoundaryLoops(r);
            else
                r.SetExternalConstraints(new MeshConstraints());

            if (edgeC.Count > 0)
            {
                for (int i = 0; i < edgeC.Count; i++)
                {
                    var tempEC = edgeC[i];

                    for (int j = 0; j < tempEC.edges.Length; j++)
                    {
                        r.Constraints.SetOrUpdateEdgeConstraint(tempEC.edges[j], tempEC.constraint);

                        if (tempEC.PinVerts)
                        {
                            Index2i edgeV = dMsh_copy.GetEdgeV(i);
                            r.Constraints.SetOrUpdateVertexConstraint(edgeV.a, VertexConstraint.Pinned);
                            r.Constraints.SetOrUpdateVertexConstraint(edgeV.b, VertexConstraint.Pinned);
                        }
                    }
                }
            }

            if (points.Count > 0)
            {

                DMeshAABBTree3 mshAABB = new DMeshAABBTree3(dMsh_copy);


                var v3pts = points.Select(pt => pt.ToVec3d());

                foreach (var p in v3pts)
                {
                    int id = mshAABB.FindNearestVertex(p, 0.1);

                    if(id != -1)
                        r.Constraints.SetOrUpdateVertexConstraint(id, VertexConstraint.Pinned);
                }
            }

            if(projBack)
                r.SetProjectionTarget(MeshProjectionTarget.Auto(dMsh_goo.Value));
            

            for (int k = 0; k < numI; ++k)
                r.BasicRemeshPass();

            bool isValid = dMsh_copy.CheckValidity();

            if (!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during remeshing. Please check...");

            DA.SetData(0, dMsh_copy);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.g3_gh_icons_25_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("862f4d4e-914c-495e-a1f1-3465879263b5"); }
        }
    }
}
