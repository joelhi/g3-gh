using System;
using System.Collections.Generic;
using System.Timers;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Components.Params;
using g3gh.Core.Goos;

using System.Threading;

using g3;
using System.Windows.Forms;
using Grasshopper.GUI;
using System.Linq;

namespace g3gh.Components.Remesh
{
    public class RemeshDMesh3 : GH_Component
    {
        Remesher r;
        DMesh3 dMsh_copy;
        int passes = 0;
        int timeStep = 100;
        public RemeshDMesh3()
          : base("Remesh", "rMsh",
            "Remesh a DMesh3 object based on a target edge length",g3ghUtil.pluginName
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
            pManager.AddBooleanParameter("Run", "run", "Run remeshing?", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Reset", "reset", "Reset mesh?", GH_ParamAccess.item, false);

            pManager[2].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "New mesh", GH_ParamAccess.item);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            var timeStepItem = GH_DocumentObject.Menu_AppendTextItem(menu, timeStep.ToString(), null, timeStepUpdate, false);
            timeStepItem.ToolTipText = "Time step for iterations";
            timeStepItem.Tag = "timestep";
        }

        private void timeStepUpdate(GH_MenuTextBox sender, string newText)
        {
            try
            {
                timeStep = int.Parse(newText);
            }
            catch (Exception)
            {
                timeStep = 100;
            }

            this.ExpireSolution(true);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            double targetL = 0;
            List<Point3d> points = new List<Point3d>();
            int fixB = 0;
            List<EdgeConstraint_goo> edgeC = new List<EdgeConstraint_goo>();
            bool projBack = false;
            double smoothing = 0;
            bool run = false;
            bool reset = false;
            int maxIter = 0;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref targetL);
            DA.GetDataList(2, points);
            DA.GetData(3, ref fixB);
            DA.GetDataList(4, edgeC);
            DA.GetData(5, ref projBack);
            DA.GetData(6, ref maxIter);
            DA.GetData(7, ref smoothing);
            DA.GetData(8, ref run);
            DA.GetData(9, ref reset);

            if (passes >= maxIter)
                run = false;


            if (r is null || reset)
            {
                dMsh_copy = new DMesh3(dMsh_goo.Value);

                r = new Remesher(dMsh_copy);
                r.PreventNormalFlips = true;
                r.SetTargetEdgeLength(targetL);
                r.SmoothSpeedT = smoothing;

                passes = 0;

                if (fixB == 2)
                    MeshConstraintUtil.FixAllBoundaryEdges(r);
                else if (fixB == 1)
                    MeshConstraintUtil.PreserveBoundaryLoops(r);
                else
                    r.SetExternalConstraints(new MeshConstraints());

                if (projBack)
                {
                    r.SetProjectionTarget(MeshProjectionTarget.Auto(dMsh_goo.Value));
                }

                if (edgeC.Count > 0)
                {
                    for (int i = 0; i < edgeC.Count; i++)
                    {
                        var tempEC = edgeC[i];

                        IProjectionTarget target = new DCurveProjectionTarget(tempEC.crv);

                        for (int j = 0; j < tempEC.edges.Length; j++)
                        {
                            tempEC.constraint.Target = target;
                            r.Constraints.SetOrUpdateEdgeConstraint(tempEC.edges[j], tempEC.constraint);
                        }

                        for (int j = 0; j < tempEC.vertices.Length; j++)
                        {
                            if (tempEC.PinVerts)
                            {
                                r.Constraints.SetOrUpdateVertexConstraint(tempEC.vertices[j], VertexConstraint.Pinned);
                            }
                            else
                            {
                                r.Constraints.SetOrUpdateVertexConstraint(tempEC.vertices[j], new VertexConstraint(target));
                            }
                        }
                    }
                }

                if (points.Count > 0)
                {
                    DMeshAABBTree3 mshAABB = new DMeshAABBTree3(dMsh_copy, true);

                    var v3pts = points.Select(pt => pt.ToVec3d());

                    foreach (var p in v3pts)
                    {
                        int id = mshAABB.FindNearestVertex(p, 0.1);

                        if (id != -1)
                            r.Constraints.SetOrUpdateVertexConstraint(id, VertexConstraint.Pinned);
                    }
                }
            }

            if (run && !reset)
            {
                r.BasicRemeshPass();
                passes++;
                Update();
            }

            bool isValid = dMsh_copy.CheckValidity();

            if (!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during remeshing. Please check...");

            this.Message = "Pass: " + passes.ToString();

            DA.SetData(0, dMsh_copy);
        }

        private void Update()
        {
            var doc = this.OnPingDocument();
            doc.ScheduleSolution(timeStep, callback);
        }

        private void callback(GH_Document doc)
        {
            this.ExpireSolution(false);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.g3_gh_icons_02;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("063b9251-fcbc-4b7a-a305-94e301763952"); }
        }


    }
}
