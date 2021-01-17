using System;
using System.Collections.Generic;
using System.Timers;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Components.Params;
using gh3sharp.Core.Goos;

using System.Threading;

using g3;

namespace gh3sharp.Components.Remesh
{
    public class RemeshDMesh3 : GH_Component
    {
        Remesher r;
        DMesh3 dMsh_copy;
        int passes = 0;

        public RemeshDMesh3()
          : base("Remesh", "Nickname",
            "RemeshDMesh3 description",gh3sharpUtil.pluginName
            , "3_Remesh")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddNumberParameter("Target Edge Length", "len", "Target edge length for remeshing", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number of iterations", "iter", "Maximum number of iterations", GH_ParamAccess.item, 10);
            pManager.AddBooleanParameter("Constrain Edges", "c", "Option to constrain the edges during the remeshing procedure", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Project to Input", "p", "Project the remeshed result back to the input mesh", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Run", "run", "Run remeshing?", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Reset", "reset", "Reset mesh?", GH_ParamAccess.item, false);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            double targetL = 0;
            bool fixB = false;
            bool projBack = false;
            bool run = false;
            bool reset = false;
            int maxIter = 0;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref targetL);
            DA.GetData(3, ref fixB);
            DA.GetData(4, ref projBack);
            DA.GetData(2, ref maxIter);
            DA.GetData(5, ref run);
            DA.GetData(6, ref reset);

            if (passes >= maxIter)
                run = false;


            if (r is null || reset)
            {
                dMsh_copy = new DMesh3(dMsh_goo.Value);

                r = new Remesher(dMsh_copy);
                r.PreventNormalFlips = true;
                r.SetTargetEdgeLength(targetL);
                r.SmoothSpeedT = 0.5;

                passes = 0;

                if (fixB)
                    MeshConstraintUtil.PreserveBoundaryLoops(r);

                if (projBack)
                {
                    
                    r.SetProjectionTarget(MeshProjectionTarget.Auto(dMsh_goo.Value));
                }

                
            }

            if (run && !reset)
            {
                r.BasicRemeshPass();
                passes++;
                this.ExpireSolution(true);
            }

            bool isValid = dMsh_copy.CheckValidity();

            if (!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during remeshing. Please check...");

            this.Message = "Pass: " + passes.ToString();

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
                return Resource1.remesh;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("063b9251-fcbc-4b7a-a305-94e301763952"); }
        }


    }
}
