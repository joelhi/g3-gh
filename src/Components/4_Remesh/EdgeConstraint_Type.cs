using System;
using System.Collections.Generic;
using g3;
using g3gh.Core;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace g3gh.Components.Remesh
{
    public class EdgeConstraint_Type : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the EdgeConstraint_Type class.
        /// </summary>
        public EdgeConstraint_Type()
          : base("Edge Constraint Type", "eConstrType",
              "Create a set of edge constraint flags to use as custom constraints",
              g3ghUtil.pluginName, "4_Remesh")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Allow Flip", "f", "Allow remesher to flip edge?", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Allow Collapse", "c", "Allow remesher to flip edge?", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Allow Split", "s", "Allow remesher to flip edge?", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Preserve Topology?", "t", "Allow remesher to flip edge?", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Edge Constraint Type", "constr", "Constraint type int with the specified flags", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            bool flip = true;
            bool collapse = true;
            bool split = true;
            bool topology = true;

            DA.GetData(0, ref flip);
            DA.GetData(1, ref collapse);
            DA.GetData(2, ref split);
            DA.GetData(3, ref topology);

            EdgeRefineFlags constraint = EdgeRefineFlags.NoConstraint;

            if (!flip)
                constraint = constraint | EdgeRefineFlags.NoFlip;

            if (!collapse)
                constraint = constraint | EdgeRefineFlags.NoCollapse;

            if (!split)
                constraint = constraint | EdgeRefineFlags.NoSplit;

            if (topology)
                constraint = constraint | EdgeRefineFlags.PreserveTopology;


            DA.SetData(0, (int)constraint);
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_26;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("649144F8-5FDE-4A3A-994D-B888DB11BB27"); }
        }
    }
}