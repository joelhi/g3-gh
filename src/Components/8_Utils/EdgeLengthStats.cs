using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;


using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Utils
{
    public class EdgeLengthStats : GH_Component
    {

        public EdgeLengthStats()
          : base("Mesh Edge Statistics", "edgeStats",
              "Extract information about mesh edge lengths. Useful when setting target edge lengths for remeshing.",
              g3ghUtil.pluginName, "8_Utils")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "msh", "Mesh to evaluate", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Max Edge Length", "max", "Maximum length of an edge", GH_ParamAccess.list);
            pManager.AddNumberParameter("Min Edge Length", "min", "Minimum length of an edge", GH_ParamAccess.list);
            pManager.AddNumberParameter("Average Edge Length", "avrg", "Average length of all edges", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
                return Resource1.g3_gh_icons_32_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("bb08176b-c537-433b-9cad-fabaeb7268b3"); }
        }
    }
}
