using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;

namespace g3gh.Components.Evaluate
{
    public class FaceNormals : GH_Component
    {

        public FaceNormals()
          : base("FaceNormals", "faceN",
            "Extract the face normals of a DMesh3 object",
            g3ghUtil.pluginName, "2_Evaluate")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to extract normals from", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Face Centroids", "pt", "Centroid of mesh faces", GH_ParamAccess.list);
            pManager.AddVectorParameter("Face Normals", "vec", "Normal vector of mesh faces", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_38_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0cf13727-ebe5-40a0-965f-230152dd7fd0"); }
        }
    }
}
