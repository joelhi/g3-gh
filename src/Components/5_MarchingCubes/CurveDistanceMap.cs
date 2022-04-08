using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;



using g3;
using System.Threading.Tasks;

namespace g3gh.Components.MarchingCubes
{
    public class CurveDistanceField : GH_Component
    {

        public CurveDistanceField()
          : base("Curve Distance Field", "crvDist",
              "Assign a value to each point of a grid based on their shortest distance to a curve in a set.",
              g3ghUtil.pluginName, "5_MarchingCubes")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Target grid for values.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curves", "crvs", "Curves to compute values for", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Grid with values assigned.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo goo = null;
            List<Curve> crvs = new List<Curve>();

            DA.GetData(0, ref goo);
            DA.GetDataList(1, crvs);

            DenseGridTrilinearImplicit grid = new DenseGridTrilinearImplicit(new DenseGrid3f(goo.Value.Grid), new g3.Vector3f(goo.Value.GridOrigin),goo.Value.CellSize);
            
            var pts = grid.ToRhinoPts();
            int numCurves = crvs.Count;

            Parallel.For(0, pts.Length, i =>
             {
                 var pt = pts[i];
                 double[] distances = new double[numCurves];
                 for (int j = 0; j < crvs.Count; j++)
                 {
                     double param = 0;
                     var crv = crvs[j];
                     bool res = crv.ClosestPoint(pt, out param);

                     double tempDouble = crv.PointAt(param).DistanceTo(pt);
                     distances[j] = tempDouble;
                 }
                 grid.Grid.Buffer[i] = (float)distances.Min();
             });

            DA.SetData(0, grid);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_23;
            }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("37eef048-756c-4761-ad2a-e97b9f17f175"); }
        }
    }
}