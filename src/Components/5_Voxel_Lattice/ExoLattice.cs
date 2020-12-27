using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using gh3sharp.Core;

using g3;

namespace gh3sharp.Components.Voxel_Lattice
{
    public class ExoLattice : GH_Component
    {

        public ExoLattice()
          : base("ExoLattice", "Nickname",
            "ExoLattice description",
            gh3sharpUtil.pluginName, "Voxel_Lattice")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //double radius = 0.1;
            //List<BoundedImplicitFunction3d> Lines = new List<BoundedImplicitFunction3d>();
            //foreach (Index4i edge_info in mesh.Edges())
            //{
            //    var segment = new Segment3d(mesh.GetVertex(edge_info.a), mesh.GetVertex(edge_info.b));
            //    Lines.Add(new ImplicitLine3d() { Segment = segment, Radius = radius });
            //}
            //ImplicitNaryUnion3d unionN = new ImplicitNaryUnion3d() { Children = Lines };
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
            get { return new Guid("b4d92ffc-8fd9-41f3-ad34-3c8a56d9ee24"); }
        }
    }
}
