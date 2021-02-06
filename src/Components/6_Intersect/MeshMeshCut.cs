using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3;

using g3gh.Core;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components._Intersect
{
    public class MeshMeshCut : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CutMesh class.
        /// </summary>
        public MeshMeshCut()
          : base("Mesh | Mesh Cut", "mshXmsh",
              "Description",
              g3ghUtil.pluginName, "6_Intersect")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to cut", GH_ParamAccess.item);
            pManager.AddParameter(new DMesh3_Param(), "Cutter", "c", "Mesh to cut with", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Remove Contained?", "rm", "Remove Contained Meshes?", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Cut mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            DMesh3_goo cut = null;
            bool cap = false;

            DA.GetData(0, ref goo);
            DA.GetData(1, ref cut);
            DA.GetData(2, ref cap);

            DMesh3 ms = new DMesh3(goo.Value);

            g3.MeshMeshCut cutter = new g3.MeshMeshCut();

            cutter.Target = new DMesh3(goo.Value);
            cutter.CutMesh = new DMesh3(cut.Value);

            cutter.Compute();

            if (cap)
                cutter.RemoveContained();

            DA.SetData(0, cutter.CutMesh);
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

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
                return Resource1.g3_gh_icons_26_copy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f7f8aa81-faf3-4e54-a23a-60f35875331b"); }
        }
    }
}
