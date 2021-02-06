using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using g3;

using g3gh.Core;

namespace g3gh.Core.Goos
{

    public class EdgeLoop_goo : GH_GeometricGoo<EdgeLoop>
    {
        public Mesh dispMsh = null;
        public PolylineCurve loop = null;

        public EdgeLoop_goo()
        {

            this.Value = null;
        }

        public EdgeLoop_goo(EdgeLoop loop)
        {
            Value = loop;

        }

        public void GenerateDispMesh()
        {
            if (dispMsh == null)
            {
                dispMsh = this.Value.Mesh.ToRhino();
            }
        }

        public void GenerateDispCurves()
        {
            if (loop == null)
            {
                loop = new PolylineCurve(this.Value.Vertices.Select(ind => Value.Mesh.GetVertex(ind).ToRhinoPt()));
            }
        }

        public override string ToString()
        {
            return "EdgeLoop [IsBoundary:" + this.Value.IsBoundaryLoop().ToString() + " V:" + this.Value.VertexCount.ToString() + "]";
        }


        public override string TypeDescription
        {
            get { return ("EdgeLoop Goo"); }
        }

        public override string TypeName
        {
            get { return ("EdgeLoop"); }
        }

        public override IGH_Goo Duplicate()
        {
            return this;
        }

        public override bool IsValid
        {
            get { return !(Value is null); }
        }

        public override BoundingBox Boundingbox => throw new NotImplementedException();

        public override object ScriptVariable()
        {
            return base.ScriptVariable();
        }

        public override bool CastTo<Q>(out Q target)
        {

            //Cast to mesh.
            if (typeof(Q).IsAssignableFrom(typeof(PolylineCurve)))
            {
                if (Value == null)
                    target = default(Q);
                else
                    target = (Q)(object)(new PolylineCurve(this.Value.Vertices.Select(ind => Value.Mesh.GetVertex(ind).ToRhinoPt())));
                return true;
            }

            target = default(Q);
            return false;
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new EdgeLoop_goo(new EdgeLoop(Value));
        }

        public override BoundingBox GetBoundingBox(Transform xform)
        {

            BoundingBox box = BoundingBox.Empty;
            box.Union(Value.GetBounds().ToRhino());
            return box;
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            throw new NotImplementedException();
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            throw new NotImplementedException();
        }
    }
}