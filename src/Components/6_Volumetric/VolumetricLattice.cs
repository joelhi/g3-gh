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
    public class VolumetricLattice : GH_Component
    {

        public VolumetricLattice()
          : base("VolumetricLattice", "Nickname",
            "VolumetricLattice description",
            g3ghUtil.pluginName, "6_Volumetric")
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

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_35_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8e74a4e3-fd43-4b40-87bf-f0b51a0ad676"); }
        }
    }
}
