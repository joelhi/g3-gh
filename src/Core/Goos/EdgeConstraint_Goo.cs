using g3;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g3gh.Core.Goos
{
    public class EdgeConstraint_goo : GH_GeometricGoo<int[]>
    {
        public enum EdgeType
        { 
            Empty = 0,
            Span = 1,
            Loop = 2
        };

        public EdgeConstraint_goo()
        {
            this.edgeType = EdgeType.Empty;
        }

        public EdgeConstraint_goo(EdgeType type)
        {
            this.edgeType = type;
        }

        internal int[] edges;

        internal int[] vertices;

        internal DCurve3 crv;

        internal EdgeConstraint constraint;

        internal EdgeType edgeType;

        public Curve DisplayCurve = null;

        internal bool PinVerts = false;

        public override BoundingBox Boundingbox => crv.GetBoundingBox().ToRhino();

        public override string TypeName => "Edge Constraint";

        public override string TypeDescription => "Edge constraint for remeshing process";

        public void GenerateDisplayCurve()
        {
            if (DisplayCurve == null)
                DisplayCurve = crv.ToRhino();

            return;
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            throw new NotImplementedException();
        }

        public override BoundingBox GetBoundingBox(Transform xform)
        {
            return crv.GetBoundingBox().ToRhino();
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Edge Constraint | " + edgeType.ToString();
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            throw new NotImplementedException();
        }
    }
}
