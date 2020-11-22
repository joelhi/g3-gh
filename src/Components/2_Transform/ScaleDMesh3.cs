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
        public ScaleDMesh3()
          : base("ScaleDMesh3", "Nickname",
            "ScaleDMesh3 description",
            gh3sharpUtil.pluginName, "2_Transform")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddVectorParameter("Scale Factor", "f", "Scale factor for DMesh3", GH_ParamAccess.item, new Rhino.Geometry.Vector3d(1,1,1));
            pManager.AddPointParameter("Origin", "o", "Origin point", GH_ParamAccess.item, new Point3d(0, 0, 0));
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            Rhino.Geometry.Vector3d sFact = new Rhino.Geometry.Vector3d(1,1,1);
            Point3d origin = new Point3d(0, 0, 0);

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref sFact);
            DA.GetData(2, ref origin);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);
            MeshTransforms.Scale(dMsh_copy, sFact.ToVec3d(), origin.ToVec3d());

            DA.SetData(0, dMsh_copy);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("abb2f6bd-1f09-4068-9e49-29f91b8c8806"); }
        }
    }
}
