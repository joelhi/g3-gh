using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;
using gh3sharp.Components.Params;
using gh3sharp.Core.Goos;

using g3;

namespace gh3sharp.Components.Remesh
{
    public class ReduceDMesh3 : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ReduceDMesh3()
          : base("Reduce DMesh3", "Nickname",
            "ReduceDMesh3 description",
            gh3sharpUtil.pluginName, "3_Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddIntegerParameter("Number of faces", "n", "Number of faces after reduction", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Constrain Edges", "c", "Option to constrain the edges during the reduction procedure", GH_ParamAccess.item,false);
            pManager.AddBooleanParameter("Project to Input", "p", "Project the reduced result back to the input mesh", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo dMsh_goo = null;
            int numF = 0;
            bool fixB = false;
            bool projBack = false;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref numF);
            DA.GetData(2, ref fixB);
            DA.GetData(3, ref projBack);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);

            Reducer r = new Reducer(dMsh_copy);

            if(fixB)
            {
                r.SetExternalConstraints(new MeshConstraints());
                MeshConstraintUtil.FixAllBoundaryEdges(r.Constraints, dMsh_copy);
            }
            if(projBack)
            {
                DMeshAABBTree3 tree = new DMeshAABBTree3(new DMesh3(dMsh_copy));
                tree.Build();
                MeshProjectionTarget target = new MeshProjectionTarget(tree.Mesh, tree);
                r.SetProjectionTarget(target);
            }

            r.ReduceToTriangleCount(numF);
            bool isValid = dMsh_copy.CheckValidity();

            if (!isValid)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Mesh seems to have been corrupted during reduction. Please check...");

            DA.SetData(0, dMsh_copy);
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
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("41abcf02-b005-4753-9075-a0320a9f069c"); }
        }
    }
}
