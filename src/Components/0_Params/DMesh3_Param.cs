using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Grasshopper.Kernel.Types;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using g3;
using Rhino.Geometry;
using Rhino.Display;

namespace gh3sharp.Components.Params
{
    public class DMesh3_Param : GH_Param<DMesh3_goo>, IGH_PreviewObject
    {

        public DMesh3_Param() :
            base("DMesh3 Parameter", "DMesh3", "Holds a DMesh3 Object", gh3sharpUtil.pluginName, "0_params", GH_ParamAccess.item)
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
            DisplayPipeline dp = args.Display;

            List<Mesh> meshes = new List<Mesh>();

            foreach (DMesh3_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDispMesh();

                if(goo.dispMsh.VertexColors.Count != 0)
                    dp.DrawMeshFalseColors(goo.dispMsh);
                else
                    dp.DrawMeshShaded(goo.dispMsh, new DisplayMaterial(Color.DarkSlateGray, 0.2));

                dp.DrawMeshWires(goo.dispMsh, Color.DarkGray);
            }
        }


        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            DisplayPipeline dp = args.Display;

            List<Mesh> meshes = new List<Mesh>();

            foreach (DMesh3_goo goo in this.m_data.NonNulls)
            {
                goo.GenerateDispMesh();
                dp.DrawMeshWires(goo.dispMsh, Color.DarkGray);
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
                return null;
            }
        }
    }
}
