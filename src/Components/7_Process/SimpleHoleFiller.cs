using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3;

using g3gh.Core;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components._Process
{
    public class SimpleHoleFiller : GH_Component
    {

        public SimpleHoleFiller()
          : base("SimpleHoleFiller", "Nickname",
            "SimpleHoleFiller description",
            g3ghUtil.pluginName, "7_Process")
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
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("3f22f345-7b53-4c0b-b048-f5db2a4f4b61"); }
        }
    }
}
