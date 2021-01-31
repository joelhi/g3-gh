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

namespace g3gh.Components.Intersect
{
    public class MeshPlaneCut : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CutMesh class.
        /// </summary>
        public MeshPlaneCut()
          : base("Mesh | Plane Cut", "mshXpl",
              "Description",
              g3ghUtil.pluginName, "5_Intersect")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "ms", "Mesh to cut", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "pl", "plane to cut with", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Cap Holes?", "cap", "Cap holes on cutting plane?", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "ms", "cut mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            Plane plane = new Plane();
            bool cap = false;

            DA.GetData(0, ref goo);
            DA.GetData(1, ref plane);
            DA.GetData(2, ref cap);

            DMesh3 ms = new DMesh3(goo.Value);

            g3.MeshPlaneCut cutter = new g3.MeshPlaneCut(ms, plane.Origin.ToVec3d(), plane.ZAxis.ToVec3d());
            cutter.Cut();

            if (cap)
                cutter.FillHoles();

            DA.SetData(0, cutter.Mesh);
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_22_copy;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ee3e6d8a-48e2-4f3f-8a9a-3b47308461c0"); }
        }
    }
}