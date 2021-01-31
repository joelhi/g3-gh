using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Grasshopper.Kernel.Types;

using g3gh.Core;
using g3gh.Core.Goos;
using g3;
using Rhino.Geometry;
using Rhino.Display;

using g3gh;

namespace g3gh.Components.Params
{
    public class Grid3f_Param : GH_Param<Grid3f_goo>, IGH_PreviewObject
    {

        public Grid3f_Param() :
            base("Grid3f", "Grid3f", "Holds a Grid3f Object. This is a grid of point with values, which can be used to create meshes using marching cubes. ", g3ghUtil.pluginName, "0_params", GH_ParamAccess.item)
        { }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override Grid3f_goo PreferredCast(object data)
        {
            if (data is DenseGridTrilinearImplicit)
                return new Grid3f_goo((DenseGridTrilinearImplicit)data);
            else
                return null;
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            DrawViewportWires(args);
        }


        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            DisplayPipeline dp = args.Display;

            foreach (Grid3f_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDispPts();
                int len = goo.dispPts.Length;

                for (int i = 0; i < len; i++)
                {
                    dp.DrawPoint(goo.dispPts[i]);
                    //dp.DrawDot(goo.dispPts[i], goo.values[i].ToString());
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("07eb1e6d-bd57-4c66-bdbd-32cab4e898d4"); }
        }

        public bool Hidden { get; set; }

        public bool IsPreviewCapable => true;

        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get
            {
                if (Hidden)
                    return BoundingBox.Empty;

                BoundingBox box = BoundingBox.Empty;

                foreach (Grid3f_goo gGoo in this.m_data.NonNulls)
                    box.Union(gGoo.Value.Bounds().ToRhino());

                return box;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_07_copy;
            }
        }
    }
}
