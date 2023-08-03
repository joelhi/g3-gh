﻿using System;
using System.Collections.Generic;
using g3;
using g3gh.Components.Params;
using g3gh.Core;
using g3gh.Core.Goos;
using Grasshopper.Kernel;
using gs;
using Rhino.Geometry;

namespace g3gh.Components.Make_Modify
{
    public class MeshCreaseEdgeSpans : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshCreaseEdgeLoops class.
        /// </summary>
        public MeshCreaseEdgeSpans()
          : base("Get Creased Edges", "creasedEdges",
              "Get edges with a crease angle above a cut of value",
              g3ghUtil.pluginName, "1_Make_Modify")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param());
            pManager.AddNumberParameter("Crease angle", "angle", "Angle for crease detection", GH_ParamAccess.item, 30);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new EdgeSpan_Param(), "Crease Edge Spans", "spans", "Creased spans in mesh", GH_ParamAccess.list);
            pManager.AddParameter(new EdgeLoop_Param(), "Crease Edge Loops", "loops", "Creased loops in mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            double angle = 30;

            DA.GetData(0, ref goo);
            DA.GetData(1, ref angle);

            DMesh3 mesh = new DMesh3(goo.Value);

            MeshTopology topology = new MeshTopology(mesh);

            topology.CreaseAngle = angle;

            topology.Compute();

            var e = topology.CreaseEdges;

            DA.SetDataList(0, topology.Spans);
            DA.SetDataList(1, topology.Loops);
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_43;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("61763A98-235E-4274-9797-77C718084D56"); }
        }
    }
}