using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using gh3sharp.Components.Params;

using g3;

namespace gh3sharp.Components.MarchingCubes
{
    public class MarchingCubesIso : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MarchingCubesIso()
          : base("Marching Cubes Iso Surface", "isoSrf",
            "construct marching cubes iso surface from a grid",
            gh3sharpUtil.pluginName, "4_MarchingCubes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
            pManager.AddNumberParameter("Value", "val", "Value for iso surface", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Expansion", "e", "Expansion of grid beyond size of mesh", GH_ParamAccess.item, 0);
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
            Grid3f_goo goo = null;
            double val = 0;
            double exp = 0;

            DA.GetData(0, ref goo);
            DA.GetData(1, ref val);
            DA.GetData(2, ref exp);

            var iso = goo.Value;

            g3.MarchingCubes c = new g3.MarchingCubes();
            c.Implicit = iso;
            c.Bounds = iso.Bounds();
            c.CubeSize = iso.CellSize;
            c.Bounds.Expand(3 * c.CubeSize);
            c.IsoValue = val;
            c.Generate();
            DMesh3 outputMesh = c.Mesh;

            bool isValid = outputMesh.CheckValidity();

            if(!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during reduction. Please check...");

            DA.SetData(0,outputMesh);
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
            get { return new Guid("0a86bc8e-8671-451d-ab32-721184b90bf0"); }
        }
    }
}
