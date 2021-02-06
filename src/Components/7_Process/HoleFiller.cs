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

namespace g3gh.Components.Process
{
    public class HoleFiller : GH_Component
    {
        public enum HoleFillerType {Planar = 0,Smooth = 1,Minimal = 2}

        public HoleFillerType Type = HoleFillerType.Minimal;

        public HoleFiller()
          : base("Mesh Hole Filler", "mshFill",
            "MeshHoleFiller description",
            g3ghUtil.pluginName, "7_Process")
        {
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            foreach (var item in Enum.GetValues(typeof(g3.MarchingCubes.RootfindingModes)))
                Menu_AppendItem(menu, item.ToString(), Menu_PanelTypeChanged, true, item.ToString() == Type.ToString()).Tag = "Filler";
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddParameter(new EdgeLoop_Param(),"Loops","loops","The boundary loops to try to fill",GH_ParamAccess.list);
            pManager.AddNumberParameter("Target Edge Length", "eLen", "Target edge length for hole fill", GH_ParamAccess.item, 1);
        }

        private void Menu_PanelTypeChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is "Filler")
                Type = (HoleFillerType)Enum.Parse(typeof(HoleFillerType), item.Text, true);
        }

        private void contextMenuStrip_Closing(object sender, EventArgs e)
        {
            this.ExpireSolution(true);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            List<EdgeLoop> loops = new List<EdgeLoop>();
            double eLen = 1;
            DA.GetData(0, ref goo);
            DA.GetDataList(1, loops);
            DA.GetData(2, ref eLen);

            DMesh3 msh = new DMesh3(goo.Value);

            DMesh3 outMesh;

            if (loops.Count > 1 && (Type is HoleFillerType.Smooth || Type is HoleFillerType.Minimal))
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Smooth and Minimal fill can only deal with a single edge loop at a time. Only first hole will be filled");

            switch (Type)
            {
                case HoleFillerType.Planar:
                    outMesh = PlanarFill(msh,loops,eLen);
                    break;
                case HoleFillerType.Smooth:
                    outMesh = SmoothFill(msh,loops[0],eLen);
                    break;
                case HoleFillerType.Minimal:
                    outMesh = MinimalFill(msh,loops[0],eLen);
                    break;
                default:
                    outMesh = PlanarFill(msh,loops,eLen);
                    break;
            }

            DA.SetData(0, outMesh);
        }

        private DMesh3 PlanarFill(DMesh3 mesh, List<EdgeLoop> loops, double eLen)
        {

            PlanarHoleFiller holeFiller = new PlanarHoleFiller(mesh);
            holeFiller.AddFillLoops(loops);
            holeFiller.FillTargetEdgeLen = eLen;

            holeFiller.Fill();

            return holeFiller.Mesh;
        }

        private DMesh3 SmoothFill(DMesh3 mesh, EdgeLoop loop, double eLen)
        {

            gs.SmoothedHoleFill smoothFill = new gs.SmoothedHoleFill(mesh, loop);
            smoothFill.TargetEdgeLength = eLen;

            smoothFill.Apply();
            return smoothFill.Mesh;
        }

        private DMesh3 MinimalFill(DMesh3 mesh, EdgeLoop loop, double eLen)
        {
            gs.MinimalHoleFill minimalHoleFill = new gs.MinimalHoleFill(mesh, loop);
            minimalHoleFill.Apply();
            return minimalHoleFill.Mesh;

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
