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
    public class DMesh3ToFile : GH_Component
    {

        public DMesh3ToFile()
          : base("Write To File", "DMshFromObj",
            "Write a DMesh3 object to a file as either an obj, stl or off file.",
            g3ghUtil.pluginName, "9_FileIO")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to write as a file", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "path", "The file path to which the file was written.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return Resource1.g3_gh_icons_31_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("eee63ee6-901a-4f0d-a9e3-c34d42ad7778"); }
        }
    }
}
