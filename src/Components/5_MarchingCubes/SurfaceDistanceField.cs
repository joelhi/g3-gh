using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;

using g3;
using System.Threading.Tasks;

namespace g3gh.Components._MarchingCubes
{
    public class SurfaceDistanceField : GH_Component
    {

        public SurfaceDistanceField()
          : base("Surface Distance Field", "ptDist",
              "Assign a value to each point of a grid based on their shortest distance to a surface in a set.",
              g3ghUtil.pluginName, "5_MarchingCubes")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Target grid for values.", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surfaces", "srfs", "Surfaces to compute values for", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Resulting grid with values assigned.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo goo = null;
            List<Surface> srfs = new List<Surface>();

            DA.GetData(0, ref goo);
            DA.GetDataList(1, srfs);

            DenseGridTrilinearImplicit grid = new DenseGridTrilinearImplicit(new DenseGrid3f(goo.Value.Grid), new g3.Vector3f(goo.Value.GridOrigin), goo.Value.CellSize);

            var pts = grid.ToRhinoPts();
            int numCurves = srfs.Count;

            Parallel.For(0, pts.Length, i =>
             {
                 var pt = pts[i];
                 double[] distances = new double[numCurves];
                 for (int j = 0; j < srfs.Count; j++)
                 {
                     double u = 0;
                     double v = 0;
                     var srf = srfs[j];
                     bool res = srf.ClosestPoint(pt, out u, out v);

                     double tempDouble = srf.PointAt(u, v).DistanceTo(pt);
                     distances[j] = tempDouble;
                 }
                 grid.Grid.Buffer[i] = (float)distances.Min();
             });

            DA.SetData(0, grid);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
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
                return Resource1.g3_gh_icons_24;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("256a7eb5-cd95-4f1b-81d6-55d66ddf585e"); }
        }
    }
}
