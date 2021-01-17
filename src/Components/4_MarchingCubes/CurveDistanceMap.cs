using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using gh3sharp.Components.Params;



using g3;

namespace gh3sharp.Components.MarchingCubes
{
    public class CurveDistanceMap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateCurveDistanceField class.
        /// </summary>
        public CurveDistanceMap()
          : base("Curves Distance Map", "crvDist",
              "Assign a value to each point of a grid based on their shortest distance to a curve in a set.",
              gh3sharpUtil.pluginName, "4_MarchingCubes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
            pManager.AddCurveParameter("Curves", "crvs", "Curves to compute values for", GH_ParamAccess.list);
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
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo goo = null;
            List<Curve> crvs = new List<Curve>();

            DA.GetData(0, ref goo);
            DA.GetDataList(1, crvs);

            DenseGridTrilinearImplicit grid = new DenseGridTrilinearImplicit(new DenseGrid3f(goo.Value.Grid), new g3.Vector3f(goo.Value.GridOrigin),goo.Value.CellSize);
            
            var pts = grid.ToRhinoPts();
            int numCurves = crvs.Count;

            for (int i = 0; i < pts.Length; i++)
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
            }

            DA.SetData(0, grid);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("37eef048-756c-4761-ad2a-e97b9f17f175"); }
        }
    }
}