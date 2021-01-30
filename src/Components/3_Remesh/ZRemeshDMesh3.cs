using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Components.Params;
using gh3sharp.Core.Goos;

using g3;

namespace gh3sharp.Components.Remesh
{
    public class ZombieRemeshDMesh3 : GH_Component
    {

        public ZombieRemeshDMesh3()
          : base("Remesh [Zombie]", "Nickname",
            "RemeshDMesh3 description",gh3sharpUtil.pluginName
            , "3_Remesh")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddNumberParameter("Target Edge Length", "len", "Target edge length for remeshing", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number of Iterations", "iter", "Number of Iterations for the remeshing process", GH_ParamAccess.item, 10);
            pManager.AddBooleanParameter("Constrain Edges", "c", "Option to constrain the edges during the remeshing procedure", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Project to Input", "p", "Project the remeshed result back to the input mesh", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            double targetL = 0;
            int numI = 0;
            bool fixB = false;
            bool projBack = false;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref targetL);
            DA.GetData(2, ref numI);
            DA.GetData(3, ref fixB);
            DA.GetData(4, ref projBack);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);

            Remesher r = new Remesher(dMsh_copy);
            r.PreventNormalFlips = true;
            r.SetTargetEdgeLength(targetL);
            r.SmoothSpeedT = 0.5;

            if (fixB)
                MeshConstraintUtil.PreserveBoundaryLoops(r);

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
                return Resource1.g3_gh_icons_02_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("862f6d4e-964c-495c-a1f1-3465879263b5"); }
        }
    }
}
