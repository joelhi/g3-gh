﻿using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;

using g3;

namespace g3gh.Components._MarchingCubes
{
    public class SurfaceDistanceField : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SurfaceDistanceField()
          : base("Surface Distance Field", "ptDist",
              "Assign a value to each point of a grid based on their shortest distance to a surface in a set.",
              g3ghUtil.pluginName, "5_MarchingCubes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Target grid for values.", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surfaces", "srfs", "Surfaces to compute values for", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param(), "Grid", "g3f", "Resulting grid with values assigned.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo goo = null;
            List<Surface> srfs = new List<Surface>();

            DA.GetData(0, ref goo);
            DA.GetDataList(1, srfs);

            DenseGridTrilinearImplicit grid = new DenseGridTrilinearImplicit(new DenseGrid3f(goo.Value.Grid), new g3.Vector3f(goo.Value.GridOrigin), goo.Value.CellSize);

            var pts = grid.ToRhinoPts();
            int numCurves = srfs.Count;

            for (int i = 0; i < pts.Length; i++)
            {
                var pt = pts[i];
                double[] distances = new double[numCurves];
                for (int j = 0; j < srfs.Count; j++)
                {
                    double u = 0;
                    double v = 0;
                    var srf = srfs[j];
                    bool res = srf.ClosestPoint(pt, out u, out v);

                    double tempDouble = srf.PointAt(u,v).DistanceTo(pt);
                    distances[j] = tempDouble;
                }
                grid.Grid.Buffer[i] = (float)distances.Min();
            }

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
                return Resource1.g3_gh_icons_24_copy;
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
