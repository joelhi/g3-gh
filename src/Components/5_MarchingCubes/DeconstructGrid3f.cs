using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.MarchingCubes
{
    public class DeconstructGrid3f : GH_Component
    {
        public DeconstructGrid3f()
          : base("Deconstruct Grid3f", "deconG3f",
            "Deconstruct a Grid3f object into its components.",
            g3ghUtil.pluginName, "5_MarchingCubes")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Grid object to deconstruct.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "pts", "Points in grid", GH_ParamAccess.list);
            pManager.AddNumberParameter("Values", "val", "Values for the points in the grid", GH_ParamAccess.list);
            pManager.AddIntegerParameter("X Dimension", "nx", "Size of grid in global x direction", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Y Dimension", "ny", "Size of grid in global y direction", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Z Dimension", "nz", "Size of grid in global z direction", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo goo = null;

            DA.GetData(0, ref goo);

            DA.SetDataList(0, goo.Value.ToRhinoPts());
            DA.SetDataList(1, goo.Value.Grid.Buffer);
            DA.SetData(2, goo.Value.Grid.ni);
            DA.SetData(3, goo.Value.Grid.nj);
            DA.SetData(4, goo.Value.Grid.nk);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_13_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("f7cadf56-59ad-4b50-9190-7ef0adc09bec"); }
        }
    }
}
