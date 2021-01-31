using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Utils
{
    public class Measurements : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Measurements class.
        /// </summary>
        public Measurements()
          : base("Measure Mesh", "msMsh",
                  "Measure volume and area etc. of a DMesh3 Object",
                  g3ghUtil.pluginName, "7_Utils")
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

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
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
                return Resource1.g3_gh_icons_30_copy;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9a0e2ced-5d97-46ed-8005-00a898704fa9"); }
        }
    }
}