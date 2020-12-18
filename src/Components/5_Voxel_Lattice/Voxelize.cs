using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;

namespace gh3sharp.Components.Voxel_Lattice
{
    public class Voxelize : GH_Component
    {

        public Voxelize()
          : base("Voxelize", "Voxl",
            "Voxelize a mesh using a bitmap. Returns a voxel surface mesh.",
            gh3sharpUtil.pluginName, "Voxel_Lattice")
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
            get { return new Guid("9bb0e2bd-7463-4150-a94a-1997f26095d3"); }
        }
    }
}
