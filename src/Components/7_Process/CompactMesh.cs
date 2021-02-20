using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;


using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;
using System.Windows.Forms;

namespace g3gh.Components.Process
{
    public class CompactMesh : GH_Component
    {
        public CompactMesh()
          : base("Compact Mesh", "Compact",
            "Condense the indexes of a mesh.",
            g3ghUtil.pluginName, "7_Process")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to Compact", GH_ParamAccess.item); 
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Compact mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;

            DA.GetData(0, ref goo);

            DA.SetData(0, new DMesh3(goo.Value, true));

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("d4ea6603-9a78-4b7f-91d0-ff7b6419006e"); }
        }
    }
}
