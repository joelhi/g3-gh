using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;


using g3gh.Core;

using g3;

using g3gh.Core.Goos;
using g3gh.Components.Params;
using System.Windows.Forms;

namespace g3gh.Components.Process
{
    public class CleanMesh : GH_Component
    {
        public bool RemoveDupTris = true;
        public bool RemoveOcclTris = true;
        public bool RemoveFinTris = true;

        public bool RemoveUnusedVerts = true;
        

        public CleanMesh()
          : base("Clean Mesh", "cleanMsh",
              "Clean a DMesh3, ridding it of unused vertices and faces etc. Right click for clean settings.",
              g3ghUtil.pluginName, "7_Process")
        {
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Remove Duplicate Triangles",Menu_PanelTypeChanged,true,RemoveDupTris).Tag = "DupTris";
            Menu_AppendItem(menu, "Remove Occluded Triangles", Menu_PanelTypeChanged, true, RemoveOcclTris).Tag = "OccTris";
            Menu_AppendItem(menu, "Remove Fin Triangles", Menu_PanelTypeChanged, true, RemoveFinTris).Tag = "FinTris";

            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "Remove Unused Vertices", Menu_PanelTypeChanged, true, RemoveUnusedVerts).Tag = "UnusedVert";
        }

        private void Menu_PanelTypeChanged(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;

            if (item is null)
                return;

            if (item.Text == "Remove Duplicate Triangles")
            {
                RemoveDupTris = !RemoveDupTris;
                item.Checked = !item.Checked;
            }
            else if (item.Text is "Remove Occluded Triangles")
            {
                RemoveOcclTris = !RemoveOcclTris;
                item.Checked = !item.Checked;
            }
            else if (item.Text is "Remove Fin Triangles")
            {
                RemoveFinTris = !RemoveFinTris;
                item.Checked = !item.Checked;
            }
            else if (item.Text is "Remove Unused Vertices")
            {
                RemoveUnusedVerts = !RemoveUnusedVerts;
                item.Checked = !item.Checked;
            }

            this.ExpireSolution(true);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to Clean", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Clean Mesh", "dm3", "Cleaned mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;

            DA.GetData(0, ref goo);

            DMesh3 mesh = new DMesh3(goo.Value);

            if (RemoveDupTris)
            {
                gs.RemoveDuplicateTriangles removeDuplicate = new gs.RemoveDuplicateTriangles(mesh);
                removeDuplicate.Apply();
                mesh = removeDuplicate.Mesh;
            }

            if (RemoveOcclTris)
            {
                gs.RemoveOccludedTriangles removeOccluded = new gs.RemoveOccludedTriangles(mesh);
                removeOccluded.Apply();
                mesh = removeOccluded.Mesh;
            }
            
            if(RemoveUnusedVerts)
                MeshEditor.RemoveUnusedVertices(mesh);

            if(RemoveFinTris)
                MeshEditor.RemoveFinTriangles(mesh);

            DA.SetData(0, mesh);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_29_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("e0410764-d4f7-4d94-a51c-e832d6f58ab0"); }
        }
    }
}
