using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using gh3sharp.Components.Params;

using g3;

namespace gh3sharp.Components.Transform
{
    public class MoveDMesh3 : GH_Component
    {

        public MoveDMesh3()
          : base("Translate", "moveDMesh3",
            "Translate a DMesh3 along a translation vector",
            gh3sharpUtil.pluginName, "2_Transform")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddVectorParameter("Translation Vector", "v", "Translation vector for DMesh3", GH_ParamAccess.item, new Rhino.Geometry.Vector3d(0, 0, 0));
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            Rhino.Geometry.Vector3d vec = new Rhino.Geometry.Vector3d(0,0,0);

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref vec);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);
            MeshTransforms.Translate(dMsh_copy, vec.ToVec3d());

            DA.SetData(0, dMsh_copy);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.translate;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("f1968b8e-de03-4e61-9695-7782372ba100"); }
        }
    }
}
