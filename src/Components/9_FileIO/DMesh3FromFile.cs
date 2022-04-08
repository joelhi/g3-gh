using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Components.Params;

using g3;

namespace g3gh.Components.FileIO
{
    public class DMesh3FromFile: GH_Component
    {

        public DMesh3FromFile()
          : base("Read From File", "DMshFromFile",
            "Read a DMesh3 object frorm either an obj, stl or off file.",
            g3ghUtil.pluginName, "9_FileIO")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "path", "path to file with extension [.obj .stl .off]",GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(),"Mesh","dm3","The read mesh from the file",GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";

            DA.GetData(0, ref path);
            DMesh3 dMsh = StandardMeshReader.ReadMesh(path);

            DA.SetData(0, dMsh);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_04;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("bec18d88-10f7-4242-b58a-f33f0ce1f018"); }
        }
    }
}
