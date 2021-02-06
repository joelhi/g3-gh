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

namespace g3gh.Components.MarchingCubes
{
    public class AssignValuesGrid3f : GH_Component
    {

        public AssignValuesGrid3f()
          : base("Assign Values to Grid3f", "GridAssign",
            "AssignValuesGrid3f description",
            g3ghUtil.pluginName, "5_MarchingCubes")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
            pManager.AddNumberParameter("Values", "v", "Values for each point in grid", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo grid = null;
            List<double> vals = new List<double>();

            DA.GetData<Grid3f_goo>(0, ref grid);
            DA.GetDataList<double>(1, vals);

            if(vals.Count != (grid.Value.Grid.ni * grid.Value.Grid.nj * grid.Value.Grid.nk))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "List of values doesn't match number of points");
                return;
            }

            grid.Value.Grid.Buffer = vals.Select(v => Convert.ToSingle(v)).ToArray();

            DA.SetData(0, grid);
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
                return Resource1.g3_gh_icons_16_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("d39f4b95-90a3-4891-a446-be5a0e3154b3"); }
        }
    }
}
