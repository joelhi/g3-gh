using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3;

using g3gh.Core;

using g3gh.Core.Goos;
using g3gh.Components.Params;
using System.Windows.Forms;
using GH_IO.Serialization;

namespace g3gh.Components.Process
{
    public enum HoleFillerType { Planar = 0, Smooth = 1, Minimal = 2 }

    public class IterativeHoleFiller : GH_Component
    {
        
        public HoleFillerType Type = HoleFillerType.Minimal;

        public IterativeHoleFiller()
          : base("Iterative Mesh Hole Fill", "iterHoleFill",
            "Iteratively fill holes in a DMesh3 object until no more holes remain.",
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
                Type = (HoleFillerType)Enum.Parse(typeof(HoleFillerType), item.Text);

        }

        private void contextMenuStrip_Closing(object sender, EventArgs e)
        {
            this.ExpireSolution(true);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddNumberParameter("Edge Length", "len", "Target edge length for hole fill", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.Message = Type.ToString();

            DMesh3_goo goo = null;
            double eLen = 1;
            DA.GetData(0, ref goo);
            DA.GetData(1, ref eLen);

            DMesh3 msh = new DMesh3(goo.Value);

            DMesh3 outMesh = msh;
            MeshBoundaryLoops loops = new MeshBoundaryLoops(outMesh, true);

            var lps = loops.Loops;

            bool hasLoops = (lps.Count > 0);
            int iter = 0;

            while(hasLoops)
            {
                EdgeLoop loop = lps[0];

                switch (Type)
                {
                    case HoleFillerType.Planar:
                        outMesh = HoleFillMethods.PlanarFill(outMesh, loop, eLen);
                        break;
                    case HoleFillerType.Smooth:
                        outMesh = HoleFillMethods.SmoothFill(outMesh, loop, eLen);
                        break;
                    case HoleFillerType.Minimal:
                        outMesh = HoleFillMethods.MinimalFill(outMesh, loop, eLen);
                        break;
                    default:
                        outMesh = HoleFillMethods.PlanarFill(outMesh, loop, eLen);
                        break;
                }

                loops = new MeshBoundaryLoops(outMesh, true);
                lps = loops.Loops;

                hasLoops = (lps.Count > 0);

                iter++;
                if (iter > 500) { break; }
            }

            this.Message += "\n" + iter.ToString() + " holes filled.";

            DA.SetData(0, outMesh);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("Type", (int)Type);

            return true;
        }

        public override bool Read(GH_IReader reader)
        {
            this.Type = (HoleFillerType)reader.GetInt32("Type");

            return true;
        }

        public override GH_Exposure Exposure{ get { return GH_Exposure.secondary; } }

        protected override System.Drawing.Bitmap Icon
        {
            get{ return Resource1.g3_gh_icons_42_copy; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1d228420-2e0f-4c36-8bad-8362e2a49bb1"); }
        }
    }
}
