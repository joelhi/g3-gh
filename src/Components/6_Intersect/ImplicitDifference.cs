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
    public class ImplicitDifference : GH_Component
    {

        public ImplicitDifference()
          : base("Implicit Difference", "implDiff",
            "Perform a Marching Cubes based boolean difference on some DMesh3 objects.",
            g3ghUtil.pluginName, "6_Intersect")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "A", "A", "Mesh to subtract from", GH_ParamAccess.item);
            pManager.AddParameter(new DMesh3_Param(), "B", "B", "Meshes to subtract with", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Number of cells", "n", "Number of sample cells", GH_ParamAccess.item, 64);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Result of subtraction", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo gooA = null;
            List<DMesh3_goo> gooB = new List<DMesh3_goo>();
            int numCells = 64;

            DA.GetData(0, ref gooA);
            DA.GetDataList(1, gooB);
            DA.GetData(2, ref numCells);

            DMesh3 A = new DMesh3(gooA.Value);
            List<DMesh3> B = gooB.Select(x => x.Value).ToList();

            ImplicitNaryDifference3d diff2 = new ImplicitNaryDifference3d();

            diff2.A = g3ghUtil.MeshToImplicit(A,numCells,0.2f);
            diff2.BSet = B.Select(x => g3ghUtil.MeshToImplicit(x,numCells,0.2f)).ToList();

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
                return Resource1.g3_gh_icons_18;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("537e28c3-91e4-4874-b541-48e02814a683"); }
        }
    }
}
