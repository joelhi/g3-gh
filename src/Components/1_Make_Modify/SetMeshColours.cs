using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Components.Params;
using g3gh.Core.Goos;
using g3gh.Core;

using g3;

using System.Drawing;

namespace g3gh.Components.MakeModify
{
    public class SetMeshColours : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SetMeshColours()
          : base("Set Mesh Vertex Colours", "setCols",
            "Set the vertex colours of a mesh.",
            g3ghUtil.pluginName, "1_Make_Modify")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to assign vertex colours to", GH_ParamAccess.item);
            pManager.AddColourParameter("Colours", "cols", "List of vertex colours for mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh with vertex colours", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            DMesh3_goo goo = null;
            List<Color> cols = new List<Color>();

            DA.GetData(0, ref goo);
            DA.GetDataList(1, cols);

            DMesh3 msh = new DMesh3(goo.Value);

            if (cols.Count == msh.VertexCount)
            {
                for (int i = 0; i < cols.Count; i++)
                {
                    msh.SetVertexColor(i, new g3.Vector3f((float)cols[i].R / 255, (float)cols[i].G / 255, (float)cols[i].B / 255));
                }
            }
            else if (cols.Count == 1)
            {
                for (int i = 0; i < cols.Count; i++)
                {
                    msh.SetVertexColor(i, new g3.Vector3f((float)cols[0].R / 255, (float)cols[0].G / 255, (float)cols[0].B / 255));
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number of colours either need to be that same amount as number of vertices in mesh, or a single one");
            }

            DA.SetData(0, msh);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_34_copy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b8275c8c-d81c-4912-a126-6dc95c422dfa"); }
        }
    }
}
