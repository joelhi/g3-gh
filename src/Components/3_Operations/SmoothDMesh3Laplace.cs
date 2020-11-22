using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Components.Params;
using gh3sharp.Core.Goos;

using g3;

namespace gh3sharp.Components.Remesh
{
    public class SmoothDMesh3Laplace : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SmoothDMesh3Laplace()
          : base("Smooth DMesh3 [Laplace]", "laplaceSmooth",
            "SmoothDMesh3Laplace description",
            gh3sharpUtil.pluginName, "3_Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddNumberParameter("Interior Weight", "w", "Weight for interior constraints", GH_ParamAccess.item,1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            double w = 1;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref w);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value, true);

            LaplacianMeshSmoother smoother = new LaplacianMeshSmoother(dMsh_copy);

            foreach (int vid in dMsh_copy.VertexIndices())
            {
                if (smoother.IsConstrained(vid) == false)
                    smoother.SetConstraint(vid, dMsh_copy.GetVertex(vid), w);

            }

                bool success = smoother.SolveAndUpdateMesh();

            bool isValid = dMsh_copy.CheckValidity();

            if (!success)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Laplacian smooth seems to have failed. Please check...");

            if (!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during smoothing. Please check...");

            DA.SetData(0, dMsh_copy);
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
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("69fa6a7a-29a4-4288-bae7-cbbf0b03b7dd"); }
        }
    }
}
