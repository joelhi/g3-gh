using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3;

using g3gh.Core;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Process
{
    public class RepairMesh : GH_Component
    {

        public RepairMesh()
          : base("Repair Mesh", "repairMsh",
            "Run a general mesh repair using the auto repair tool from the geometry3sharp library",
            g3ghUtil.pluginName, "7_Process")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(),"Mesh","dm3","Mesh to repair",GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(),"Repaired Mesh","dm3","Resulting mesh from the reparation process.",GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;

            DA.GetData(0, ref goo);

            DMesh3 mesh = new DMesh3(goo.Value);


            gs.MeshAutoRepair repair = new gs.MeshAutoRepair(mesh);
            repair.Apply();


            DA.SetData(0, repair.Mesh);

        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

  
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_33_copy;
            }
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("daf88f8a-35d9-44b3-af1f-9f653c0d5f74"); }
        }
    }
}
