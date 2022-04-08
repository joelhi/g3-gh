using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.ImplicitBoolean
{
    public class ImplicitIntersection : GH_Component
    {

        public ImplicitIntersection()
          : base("Implicit Intersection", "implInters",
            "Perform a Marching Cubes based boolean intersection on some DMesh3 objects.",
            g3ghUtil.pluginName, "6_Intersect")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Meshes", "dm3", "Meshes to intersect", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Number of cells", "n", "Number of sample cells", GH_ParamAccess.item, 64);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Result of subtraction", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<DMesh3_goo> goo = new List<DMesh3_goo>();
            int numCells = 64;

            DA.GetDataList(0, goo);
            DA.GetData(1, ref numCells);

            ImplicitNaryIntersection3d diff2 = new ImplicitNaryIntersection3d();

            diff2.Children = goo.Select(x => g3ghUtil.MeshToImplicit(x.Value, numCells, 0.2f)).ToList();

            g3.MarchingCubes c = new g3.MarchingCubes();
            c.Implicit = diff2;
            c.RootMode = g3.MarchingCubes.RootfindingModes.Bisection;
            c.RootModeSteps = 5;
            c.Bounds = diff2.Bounds();
            c.CubeSize = c.Bounds.MaxDim / numCells;
            c.Bounds.Expand(3 * c.CubeSize);
            c.Generate();

            MeshNormals.QuickCompute(c.Mesh);

            DA.SetData(0, c.Mesh);
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
                return Resource1.g3_gh_icons_19;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a4eab7aa-08ac-467d-b9fe-64797060365c"); }
        }
    }
}
