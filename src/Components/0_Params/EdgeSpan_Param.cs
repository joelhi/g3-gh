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
using System.Windows.Forms;
using Grasshopper;

namespace g3gh.Components.Params
{
    public class EdgeSpan_Param : GH_Param<EdgeSpan_goo>, IGH_PreviewObject
    {
        bool showAllMeshes = false;

        public EdgeSpan_Param() :
            base("Edge Span", "eSpan", "Holds a collection of EdgeSpan objects.", g3ghUtil.pluginName, "0_params", GH_ParamAccess.item)
        { 
            
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);

            Menu_AppendItem(menu, "Show All Meshes", onShowMeshes, true, showAllMeshes);
        }

        private void onShowMeshes(object sender, EventArgs e)
        {
            showAllMeshes = !showAllMeshes;
            this.OnDisplayExpired(true);
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

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (this.m_data.NonNulls.Count() > 1 && !showAllMeshes)
            {
                this.AddRuntimeMessage(
                    GH_RuntimeMessageLevel.Remark,
                    "Multiple EdgeLoops present, only mesh of the first will be rendered for performance reasons." +
                    "\nTo show all, right click and tick Show All Meshes");
                var goo = this.m_data.NonNulls.FirstOrDefault();
                goo.GenerateDispMesh();
                args.Display.DrawMeshWires(goo.dispMsh, Color.DarkGray);
            }
            else
            {
                foreach (var goo in m_data.NonNulls)
                {
                    goo.GenerateDispMesh();
                    args.Display.DrawMeshWires(goo.dispMsh, Color.DarkGray);
                }
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


