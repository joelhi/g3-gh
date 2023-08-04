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


namespace g3gh.Components.Params
{
    public class EdgeSpan_Param : GH_Param<EdgeSpan_goo>, IGH_PreviewObject
    {
        bool showMesh = false;

        public EdgeSpan_Param() :
            base("Edge Span", "eSpan", "Holds a collection of EdgeSpan objects.", g3ghUtil.pluginName, "0_params", GH_ParamAccess.item)
        { 
            
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override EdgeSpan_goo PreferredCast(object data)
        {
            if (data is EdgeSpan)
            {
                return new EdgeSpan_goo((EdgeSpan)data);
            }
            else
                return null;
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            DisplayPipeline dp = args.Display;

            foreach (EdgeSpan_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDispMesh();
                goo.GenerateDispCurves();
                dp.DrawMeshWires(goo.dispMsh, Color.DarkGray);
                dp.DrawCurve(goo.span, Color.DarkRed, 2);

            }
        }


        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            DisplayPipeline dp = args.Display;

            foreach (EdgeSpan_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDispMesh();
                goo.GenerateDispCurves();
                dp.DrawMeshWires(goo.dispMsh, Color.DarkGray);
                dp.DrawCurve(goo.span, Color.DarkRed, 2);

            }

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("94b719e1-5b24-450e-a300-323f08292eb3"); }
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

                foreach (EdgeSpan_goo mGoo in this.m_data.NonNulls)
                    box.Union(mGoo.Value.GetBounds().ToRhino());

                return box;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_39;
            }
        }
    }
}


