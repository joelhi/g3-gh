using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;

using g3;

namespace g3gh.Components.Volumetric
{
    public class ExtrudeMesh : GH_Component
    {

        public ExtrudeMesh()
          : base("ExtrudeMesh", "xtrudeMsh",
            "Extrude a DMesh3 object by a certain distance and stich boundaries.",
            g3ghUtil.pluginName, "6_Volumetric")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to extude", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Resulting volume mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_36_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("418b48da-485e-4af9-a78b-342f1d2a849c"); }
        }
    }
}
