using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;

using g3gh.Components.Params;

using g3;


namespace g3gh.Components.Construction
{
    public class MeshBoundaryEdgeLoops : GH_Component
    {

        public MeshBoundaryEdgeLoops()
          : base("MeshBoundaryEdgeLoops", "Nickname",
            "MeshBoundaryEdgeLoops description",
            g3ghUtil.pluginName, "1_Construction")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new EdgeLoop_Param(),"Boundary Edge Loops","loops","Boundary edge loops of mesh",GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            DA.GetData(0, ref goo);

            DMesh3 mesh = new DMesh3(goo.Value);

            MeshBoundaryLoops bounds = new MeshBoundaryLoops(mesh, true);

            DA.SetDataList(0, bounds.Loops);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("28cc951c-b35c-4a27-bb7f-e77c635cc225"); }
        }
    }
}
