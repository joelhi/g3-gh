using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;

using g3gh.Components.Params;

using g3;


namespace g3gh.Components.MakeModify
{
    public class TriangulatePolygon : GH_Component
    {

        public TriangulatePolygon()
          : base("Triangulate Polygon", "triPol",
            "Create a mesh by triangulating a polygon",
            g3ghUtil.pluginName, "1_Make_Modify")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve To Mesh", "crv", "Curve to triangulate into a mesh",GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh resulting from triangulation", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve crv = null;

            DA.GetData(0, ref crv);

            if (!(crv is PolylineCurve))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curve must be a polyline.");
                return;
            }

            
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.hidden; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.g3_gh_icons_44_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("25fa5980-2754-4255-b2a8-9a6b4114248b"); }
        }
    }
}
