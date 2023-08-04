﻿using System;
using System.Collections.Generic;
using g3;
using g3gh.Components.Params;
using g3gh.Core;
using g3gh.Core.Goos;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace g3gh.Components.Remesh
{
    public class CreateEdgeConstraint_Span : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateEdgeConstraint class.
        /// </summary>
        public CreateEdgeConstraint_Span()
          : base("Create Edge Constraint [Span]", "createSpanConstr",
              "Creates a custom edge constraint from an edge span",
              g3ghUtil.pluginName, "4_Remesh")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new EdgeSpan_Param());
            pManager.AddIntegerParameter("Edge Constraint Type", "constr", "Constraint type\nint should be generated by the edge constraint type component!", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Pin Edge Points", "pin", "Set to true to fix the location of the points.\n Otherwise they will be kept on the curve, but allowed to slide.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new EdgeConstraint_Param());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            EdgeSpan_goo eLoop = null;
            int flag = -1;
            bool pin = false;

            DA.GetData(0, ref eLoop);
            DA.GetData(1, ref flag);
            DA.GetData(2, ref pin);

            EdgeConstraint_goo edgeConstraint = new EdgeConstraint_goo(EdgeConstraint_goo.EdgeType.Span);

            edgeConstraint.crv = eLoop.Value.ToCurve(eLoop.Value.Mesh);
            edgeConstraint.vertices = eLoop.Value.Vertices;
            edgeConstraint.edges = eLoop.Value.Edges;
            edgeConstraint.edgeType = EdgeConstraint_goo.EdgeType.Loop;
            edgeConstraint.constraint = new EdgeConstraint((EdgeRefineFlags)flag, new DCurveProjectionTarget(edgeConstraint.crv));
            edgeConstraint.PinVerts = pin;

            DA.SetData(0, edgeConstraint);
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
                return Resource1.g3_gh_icons_28;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("DF6257B9-E769-4EED-8885-C1EB8F755232"); }
        }
    }
}