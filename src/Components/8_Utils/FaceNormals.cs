using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Utils
{
    public class FaceNormals : GH_Component
    {

        public FaceNormals()
          : base("FaceNormals", "Nickname",
            "FaceNormals description",
            g3ghUtil.pluginName, "8_Utils")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
                return Resource1.g3_gh_icons_38_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0cf13727-ebe5-40a0-965f-230152dd7fd0"); }
        }
    }
}
