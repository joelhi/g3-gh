using System;
using System.Collections.Generic;
using g3gh.Core;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace g3gh.Components.Make_Modify
{
    public class MeshCreaseEdgeLoops : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshCreaseEdgeLoops class.
        /// </summary>
        public MeshCreaseEdgeLoops()
          : base("MeshCreaseEdgeLoops", "Nickname",
              "Description",
              g3ghUtil.pluginName, "1_Make_Modify")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("61763A98-235E-4274-9797-77C718084D56"); }
        }
    }
}