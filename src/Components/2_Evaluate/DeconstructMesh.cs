using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Evaluate
{
    public class DeconstructMesh : GH_Component
    {
        public DeconstructMesh()
          : base("Deconstruct Mesh", "deconMsh",
              "Deconstruct a DMesh3 Object",
              g3ghUtil.pluginName, "2_Evaluate")
        {
        }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to Deconstruct", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Vertices", "v", "Vertices of the mesh",GH_ParamAccess.list);
            pManager.AddMeshFaceParameter("Faces", "f", "Faces of the mesh", GH_ParamAccess.list);
            pManager.AddColourParameter("Colours", "col", "Vertiex Colours of the mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            DA.GetData(0, ref goo);

            DMesh3 mesh = new DMesh3(goo.Value);


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
                return Resource1.g3_gh_icons_27_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("dbc08591-c039-4015-ae6d-69adc963d8d7"); }
        }
    }
}
