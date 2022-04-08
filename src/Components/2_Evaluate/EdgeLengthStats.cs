using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;


using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Evaluate
{
    public class EdgeLengthStats : GH_Component
    {

        public EdgeLengthStats()
          : base("Mesh Edge Statistics", "edgeStats",
              "Extract information about mesh edge lengths. Useful when setting target edge lengths for remeshing.",
              g3ghUtil.pluginName, "2_Evaluate")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to evaluate", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Max Edge Length", "max", "Maximum length of an edge", GH_ParamAccess.list);
            pManager.AddNumberParameter("Min Edge Length", "min", "Minimum length of an edge", GH_ParamAccess.list);
            pManager.AddNumberParameter("Average Edge Length", "avg", "Average length of all edges", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            DA.GetData(0, ref goo);

            DMesh3 mesh = new DMesh3(goo.Value);
            MeshQueries.EdgeLengthStats(mesh, out double min, out double max, out double avg);

            DA.SetData(0, max);
            DA.SetData(1, min);
            DA.SetData(2, avg);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_32;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("bb08176b-c537-433b-9cad-fabaeb7268b3"); }
        }
    }
}
