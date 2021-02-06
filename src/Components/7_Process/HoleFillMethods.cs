using System;
using System.Collections.Generic;
using System.Linq;

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
    public static class HoleFillMethods
    {
        public static DMesh3 PlanarFill(DMesh3 mesh, EdgeLoop loop, double eLen)
        {

            PlanarHoleFiller holeFiller = new PlanarHoleFiller(mesh);
            holeFiller.AddFillLoop(loop);

            Plane.FitPlaneToPoints(loop.Vertices.Select(ind => mesh.GetVertex(ind).ToRhinoPt()), out Plane pl);

            holeFiller.SetPlane(pl.Origin.ToVec3d(), pl.ZAxis.ToVec3d());

            holeFiller.FillTargetEdgeLen = eLen;

            holeFiller.Fill();

            return new DMesh3(holeFiller.Mesh);
        }

        public static DMesh3 SmoothFill(DMesh3 mesh, EdgeLoop loop, double eLen)
        {

            gs.SmoothedHoleFill smoothFill = new gs.SmoothedHoleFill(mesh, loop);
            smoothFill.TargetEdgeLength = eLen;

            bool success = smoothFill.Apply();
            return new DMesh3(smoothFill.Mesh);
        }

        public static DMesh3 MinimalFill(DMesh3 mesh, EdgeLoop loop, double eLen)
        {
            gs.MinimalHoleFill minimalHoleFill = new gs.MinimalHoleFill(mesh, loop);
            minimalHoleFill.Apply();
            return new DMesh3(minimalHoleFill.Mesh);

        }
    }
}
