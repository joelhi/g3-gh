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
using Grasshopper;

namespace g3gh.Components.Params
{
    public class EdgeLoop_Param : GH_Param<EdgeLoop_goo>, IGH_PreviewObject
    {

        public EdgeLoop_Param() :
            base("Edge Loop", "eLoop", "Holds a collection of EdgeLoop objects.", g3ghUtil.pluginName, "0_params", GH_ParamAccess.item)
        { }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override EdgeLoop_goo PreferredCast(object data)
        {
            if (data is EdgeLoop)
            {
                return new EdgeLoop_goo((EdgeLoop)data);
            }
            else
                return null;
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (this.m_data.NonNulls.Count() > 1)
            {
                this.AddRuntimeMessage(
                    GH_RuntimeMessageLevel.Remark,
                    "Multiple EdgeLoops present, only mesh of the first will be rendered for performance reasons." +
                    "\nTo show all, right click and tick Show All Meshes");
                var goo = this.m_data.NonNulls.FirstOrDefault();
                goo.GenerateDispMesh();
                args.Display.DrawMeshWires(goo.dispMsh, Color.DarkGray);
            }

            switch (args.Document.PreviewMode)
            {
                case GH_PreviewMode.Wireframe:
                    Preview_DrawWires(args);
                    break;
                case GH_PreviewMode.Shaded:
                    if (CentralSettings.PreviewMeshEdges)
                    {
                        Preview_DrawWires(args);
                    }
                    break;
            }
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            // Not needed
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("94b717e1-5b44-450e-a300-323f03292cb3"); }
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

                foreach (EdgeLoop_goo mGoo in this.m_data.NonNulls)
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
                return Resource1.g3_gh_icons_45;
            }
        }
    }
}


