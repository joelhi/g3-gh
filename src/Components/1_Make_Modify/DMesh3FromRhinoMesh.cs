using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;
using g3;

namespace g3gh.Components.MakeModify
{
    public class DMesh3FromRhinoMesh : GH_Component
    {
        public DMesh3FromRhinoMesh()
          : base("DMesh3 from Rhino Mesh", "toDM3",
            "Convert a Rhino Mesh to a DMesh3 object.",
            Core.g3ghUtil.pluginName, "1_Make_Modify")
        {
        }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "m", "Mesh To Convert", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(),"Converted Mesh","dm3","Converted mesh of typ DMesh3.",GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh rhMesh = null;

            DA.GetData(0, ref rhMesh);

            DMesh3 dMesh3 = rhMesh.ToDMesh3();

            int edgeCount = dMesh3.EdgeCount;

            DA.SetData(0, new DMesh3_goo(dMesh3));
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.g3_gh_icons_05;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("4d4d319b-6cbb-4dee-bb48-a14780b7c6d1"); }
        }
    }
}
