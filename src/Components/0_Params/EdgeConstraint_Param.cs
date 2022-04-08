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
    public class EdgeConstraint_Param : GH_Param<EdgeConstraint_goo>, IGH_PreviewObject
    {

        public EdgeConstraint_Param() :
            base("Edge Span", "eSpan", "Holds a collection of EdgeSpan objects.", g3ghUtil.pluginName, "0_params", GH_ParamAccess.item)
        { }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }


        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            DisplayPipeline dp = args.Display;

            foreach (EdgeConstraint_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDisplayCurve();
                dp.DrawCurve(goo.DisplayCurve, Color.DarkRed, 2);
                dp.DrawPoints(((PolylineCurve)goo.DisplayCurve).ToPolyline(), PointStyle.ArrowTip, 3, Color.Black);

            }
        }


        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            DisplayPipeline dp = args.Display;

            foreach (EdgeConstraint_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDisplayCurve();
                dp.DrawCurve(goo.DisplayCurve, Color.DarkRed, 2);
                dp.DrawPoints(((PolylineCurve)goo.DisplayCurve).ToPolyline(), PointStyle.ArrowTip, 3, Color.Black);

            }

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("94b719e1-5b14-450e-b300-323e08292eb5"); }
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

                foreach (EdgeConstraint_goo mGoo in this.m_data.NonNulls)
                    box.Union(mGoo.crv.GetBoundingBox().ToRhino());

                return box;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_46;
            }
        }
    }
}


