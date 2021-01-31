﻿using System;
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
    public class CleanMesh : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CutMesh class.
        /// </summary>
        public CleanMesh()
          : base("Clean Mesh", "cleanMsh",
              "Clean a DMesh3, ridding it of unused vertices and faces etc.",
              g3ghUtil.pluginName, "7_Utils")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "msh", "Mesh to Clean", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Remove Fins", "fins", "Remove fin triangles (slim / narrow ones)", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Clean Mesh", "msh", "Cleaned mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
                return Resource1.g3_gh_icons_29_copy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e0410764-d4f7-4d94-a51c-e832d6f58ab0"); }
        }
    }
}