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
    public class VertexNormals : GH_Component
    {

        public VertexNormals()
          : base("VertexNormals", "vertN",
            "Get the normal vector of each vertex in a DMesh3 Object",
            g3ghUtil.pluginName, "2_Evaluate")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to extract vertex normals from", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Vertex Normals", "vec", "Normal vector of mesh vertices", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            DA.GetData(0, ref goo);

            DMesh3 mesh = new DMesh3(goo.Value);

            List<Rhino.Geometry.Vector3d> vecs = new List<Rhino.Geometry.Vector3d>();

            foreach (var ind in mesh.VertexIndices())
            {
                vecs.Add(mesh.GetVertexNormal(ind).ToRhinoVec());
            }

            DA.SetDataList(0, vecs);
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
                return Resource1.g3_gh_icons_37_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("422e4baf-6520-4a8d-8b11-6a02ae8ceedf"); }
        }
    }
}
