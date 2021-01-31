using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;

using g3;

namespace g3gh.Components.MarchingCubes
{
    public class CreateGrid3f : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CreateGrid3f()
          : base("New Grid3f", "Nickname",
            "New Grid3f description",
            g3ghUtil.pluginName, "4_MarchingCubes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Origin", "o", "Origin of grid", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddIntegerParameter("X Cells", "nx", "Number of cells in x", GH_ParamAccess.item, 16);
            pManager.AddIntegerParameter("Y Cells", "ny", "Number of cells in y", GH_ParamAccess.item, 16);
            pManager.AddIntegerParameter("Z Cells", "nz", "Number of cells in z", GH_ParamAccess.item, 16);
            pManager.AddNumberParameter("Cell size", "s", "Size of cells", GH_ParamAccess.item, 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d pt = Point3d.Origin;

            int ni = 0;
            int nj = 0;
            int nk = 0;
            double cellSize = 0;

            DA.GetData(0, ref pt);
            DA.GetData(1, ref ni);
            DA.GetData(2, ref nj);
            DA.GetData(3, ref nk);
            DA.GetData(4, ref cellSize);

            DenseGrid3f grid = new DenseGrid3f(ni, nj, nk, 0);
            DenseGridTrilinearImplicit grd = new DenseGridTrilinearImplicit(grid, pt.ToVec3d(), cellSize);

            DA.SetData(0, grd);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
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
                return Resource1.g3_gh_icons_11_copy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d213850b-d433-43e5-8a89-ae429fbd6d3a"); }
        }
    }
}
