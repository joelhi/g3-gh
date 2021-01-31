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
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public DeconstructGrid3f()
          : base("Deconstruct Grid3f", "Nickname",
            "DeconstructGrid3f description",
            g3ghUtil.pluginName, "4_MarchingCubes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "pts", "Points in grid", GH_ParamAccess.list);
            pManager.AddNumberParameter("Values", "val", "Values for the points in the grid", GH_ParamAccess.list);
            pManager.AddIntegerParameter("X Dimension", "nx", "Size of grid in global x direction", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Y Dimension", "ny", "Size of grid in global y direction", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Z Dimension", "nz", "Size of grid in global z direction", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
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

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_13_copy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f7cadf56-59ad-4b50-9190-7ef0adc09bec"); }
        }
    }
}
