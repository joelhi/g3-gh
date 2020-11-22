using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using gh3sharp.Components.Params;

namespace gh3sharp.Components.Transform
{
    public class ScaleDMesh3 : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ScaleDMesh3()
          : base("ScaleDMesh3", "Nickname",
            "ScaleDMesh3 description",
            gh3sharpUtil.pluginName, "2_Transform")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddVectorParameter("Scale Factor", "f", "Scale factor for DMesh3", GH_ParamAccess.item, new Rhino.Geometry.Vector3d(1,1,1));
            pManager.AddPointParameter("Origin", "o", "Origin point", GH_ParamAccess.item, new Point3d(0, 0, 0));
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
            Rhino.Geometry.Vector3d sFact = new Rhino.Geometry.Vector3d(1,1,1);
            Point3d origin = new Point3d(0, 0, 0);

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref sFact);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);

            MeshTransforms.Scale(dMsh_copy, sFact.ToVec3d(), origin.ToVec3d());

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
            get { return new Guid("abb2f6bd-1f09-4068-9e49-29f91b8c8806"); }
        }
    }
}
