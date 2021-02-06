﻿using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3;

using g3gh.Core;

using g3gh.Core.Goos;
using g3gh.Components.Params;
using System.Windows.Forms;

namespace g3gh.Components.Process
{
    public class HoleFiller : GH_Component
    {

        public HoleFillerType Type = HoleFillerType.Minimal;

        public HoleFiller()
          : base("Mesh Hole Filler", "mshFill",
            "MeshHoleFiller description",
            g3ghUtil.pluginName, "7_Process")
        {
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            foreach (var item in Enum.GetValues(typeof(HoleFillerType)))
                Menu_AppendItem(menu, item.ToString(), Menu_PanelTypeChanged, true, item.ToString() == Type.ToString()).Tag = "Filler";

            menu.Closed += contextMenuStrip_Closing;
        }

        

        private void Menu_PanelTypeChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is "Filler")
                Type = (HoleFillerType)Enum.Parse(typeof(HoleFillerType),item.Text);

        }

        

        private void contextMenuStrip_Closing(object sender, EventArgs e)
        {
            this.ExpireSolution(true);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddParameter(new EdgeLoop_Param(), "Loops", "loops", "The boundary loops to try to fill", GH_ParamAccess.item);
            pManager.AddNumberParameter("Target Edge Length", "eLen", "Target edge length for hole fill", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.Message = Type.ToString();

            DMesh3_goo goo = null;
            EdgeLoop_goo loop_goo = null;
            double eLen = 1;
            DA.GetData(0, ref goo);
            DA.GetData(1, ref loop_goo);
            DA.GetData(2, ref eLen);

            DMesh3 msh = new DMesh3(goo.Value);
            EdgeLoop loop = new EdgeLoop(loop_goo.Value);

            DMesh3 outMesh;

            switch (Type)
            {
                case HoleFillerType.Planar:
                    outMesh = HoleFillMethods.PlanarFill(msh,loop,eLen);
                    break;
                case HoleFillerType.Smooth:
                    outMesh = HoleFillMethods.SmoothFill(msh,loop,eLen);
                    break;
                case HoleFillerType.Minimal:
                    outMesh = HoleFillMethods.MinimalFill(msh,loop,eLen);
                    break;
                default:
                    outMesh = HoleFillMethods.PlanarFill(msh,loop,eLen);
                    break;
            }

            DA.SetData(0, outMesh);
        }

        

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
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
            get { return new Guid("3f22f345-7b53-4c0b-b048-f5db2a4f4b61"); }
        }
    }
}
