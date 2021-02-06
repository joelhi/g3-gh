using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Components.Params;
using g3gh.Core.Goos;

using g3;

namespace g3gh.Components.Remesh
{
    public class ReduceDMesh3 : GH_Component
    {

        public ReduceDMesh3()
          : base("Reduce Mesh", "reduce",
            "Reduce the face count of a DMesh3 to a specific number",
            g3ghUtil.pluginName, "4_Remesh")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to reduce", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number of faces", "n", "Number of faces after reduction", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Constrain Edges", "c", "Option to constrain the edges during the reduction procedure", GH_ParamAccess.item,false);
            pManager.AddBooleanParameter("Project to Input", "p", "Project the reduced result back to the input mesh", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Reduced mesh", GH_ParamAccess.item);
        }

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
                MeshConstraintUtil.PreserveBoundaryLoops(r.Constraints, dMsh_copy);
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

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource1.g3_gh_icons_01_copy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("41abcf02-b005-4753-9075-a0320a9f069c"); }
        }
    }
}
