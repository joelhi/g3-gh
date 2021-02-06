using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;


using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Process
{
    public class CleanMesh : GH_Component
    {
        public CleanMesh()
          : base("Clean Mesh", "cleanMsh",
              "Clean a DMesh3, ridding it of unused vertices and faces etc.",
              g3ghUtil.pluginName, "7_Process")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to Clean", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Remove Fins", "fins", "Remove fin triangles (slim / narrow ones)", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Clean Mesh", "dm3", "Cleaned mesh", GH_ParamAccess.item);
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
                return Resource1.g3_gh_icons_29_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("e0410764-d4f7-4d94-a51c-e832d6f58ab0"); }
        }
    }
}
