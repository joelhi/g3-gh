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
    public class CreateMeshSDF : GH_Component
    {
        public CreateMeshSDF()
          : base("New Mesh SDF", "MeshSDF",
            "Create a new signed distance field (SDF) from a mesh",
            g3ghUtil.pluginName, "4_MarchingCubes")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddIntegerParameter("Number of Cells", "n", "Number of cells (Resolution)", GH_ParamAccess.item, 128);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            int num_cells = 128;
            DMesh3_goo dMsh_goo = null;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref num_cells);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);
            double cell_size = dMsh_copy.CachedBounds.MaxDim / num_cells;

            MeshSignedDistanceGrid sdf = new MeshSignedDistanceGrid(dMsh_copy, cell_size);
            sdf.Compute();

            var iso = new DenseGridTrilinearImplicit(sdf.Grid, sdf.GridOrigin, sdf.CellSize);
            Grid3f_goo goo = iso;

            DA.SetData(0, goo);
        }


        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_12_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("188897ae-828c-48b3-984f-6bf68bd749b2"); }
        }
    }
}
