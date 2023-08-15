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
using Grasshopper.Kernel.Parameters;
using Grasshopper;

namespace g3gh.Components.Params
{
    public class DMesh3_Param : GH_Param<DMesh3_goo>, IGH_PreviewObject
    {

        public DMesh3_Param() :
            base("DMesh3", "dm3", "Holds a collection of DMesh3 Objects, which is the main data structure used by the g3sharp library.", g3ghUtil.pluginName, "0_params", GH_ParamAccess.item)
        { }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override DMesh3_goo PreferredCast(object data)
        {
            if (data is Rhino.Geometry.Mesh)
            {
                return new DMesh3_goo((Rhino.Geometry.Mesh)data);
            }
            else if (data is DMesh3)
            {
                return new DMesh3_goo((DMesh3)data);
            }
            else if (data is GH_Mesh)
            {
                return new DMesh3_goo(((GH_Mesh)data).Value);
            }
            else
                return null;
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            if (args.Document.PreviewMode == GH_PreviewMode.Shaded && args.Display.SupportsShading)
            {
                Preview_DrawMeshes(args);
            }
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
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

        public override Guid ComponentGuid
        {
            get { return new Guid("b55b2356-c963-4eca-811b-20b86f9dc4c0"); }
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

                foreach (DMesh3_goo mGoo in this.m_data.NonNulls)
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
                return Resource1.g3_gh_icons_06;
            }
        }
    }
}
