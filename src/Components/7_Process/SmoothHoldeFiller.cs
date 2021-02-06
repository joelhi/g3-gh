using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace g3gh.Components._Process
{
    public class SmoothHoldeFiller : GH_Component
    {

        public SmoothHoldeFiller()
          : base("SmoothHoldeFiller", "Nickname",
            "SmoothHoldeFiller description",
            "Category", "Subcategory")
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
            get { return new Guid("d39054ca-5f24-49c3-adcd-21d306fb411e"); }
        }
    }
}
