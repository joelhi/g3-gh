using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Evaluate
{
    public class Measurements : GH_Component
    {

        public Measurements()
          : base("Mesh Measurements", "msMsh",
                  "Measure volume and area etc. of a DMesh3 Object",
                  g3ghUtil.pluginName, "2_Evaluate")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to extract information about", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Area", "A", "Mesh surface area", GH_ParamAccess.item);
            pManager.AddNumberParameter("Volume", "V", "Mesh volume", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_30_copy;
            }
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("9a0e2ced-5d97-46ed-8005-00a898704fa9"); }
        }
    }
}