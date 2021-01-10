using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using gh3sharp.Components.Params;

using g3;

namespace gh3sharp.Components.MarchingCubes
{
    public class MarchingCubesIso : GH_Component
    {

        public MarchingCubesIso()
          : base("Marching Cubes Iso Surface", "isoSrf",
            "construct marching cubes iso surface from a grid",
            gh3sharpUtil.pluginName, "4_MarchingCubes")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Grid3f_Param());
            pManager.AddNumberParameter("Value", "val", "Value for iso surface", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Expansion", "e", "Expansion of grid beyond size of mesh", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grid3f_goo goo = null;
            double val = 0;
            double exp = 0;

            DA.GetData(0, ref goo);
            DA.GetData(1, ref val);
            DA.GetData(2, ref exp);

            var iso = goo.Value;

            g3.MarchingCubes c = new g3.MarchingCubes();
            c.Implicit = iso;
            c.Bounds = iso.Bounds();
            c.CubeSize = iso.CellSize;
            c.Bounds.Expand(3 * c.CubeSize);
            c.IsoValue = val;
            c.Generate();
            
            DMesh3 outputMesh = c.Mesh;

            bool isValid = outputMesh.CheckValidity();

            if(!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during reduction. Please check...");

            DA.SetData(0,outputMesh);
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
            get { return new Guid("0a86bc8e-8671-451d-ab32-721184b90bf0"); }
        }
    }
}
