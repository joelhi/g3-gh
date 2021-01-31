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
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ImplicitDifference()
          : base("Implicit Difference", "Nickname",
            "ImplicitDifference description",
            g3ghUtil.pluginName, "5_Intersect")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "A", "A", "Mesh to subtract from", GH_ParamAccess.item);
            pManager.AddParameter(new DMesh3_Param(), "B", "B", "Meshes to subtract with", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Number of cells", "n", "Number of sample cells", GH_ParamAccess.item, 64);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Results", "result", "Result of subtraction", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
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
                return Resource1.g3_gh_icons_18_copy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("537e28c3-91e4-4874-b541-48e02814a683"); }
        }
    }
}
