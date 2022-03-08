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

    public class EdgeSpan_goo : GH_GeometricGoo<EdgeSpan>
    {
        public Mesh dispMsh = null;
        public PolylineCurve span = null;

        public EdgeSpan_goo()
        {

            this.Value = null;
        }

        public EdgeSpan_goo(EdgeSpan span)
        {
            Value = span;

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
            if (span == null)
            {
                for (int i = 0; i < Value.EdgeCount; i++)
                {
                    span = new PolylineCurve(this.Value.Vertices.Select(ind => Value.Mesh.GetVertex(ind).ToRhinoPt()));
                }
            }
        }

        public override string ToString()
        {
            return "EdgeSpan [E:" + this.Value.EdgeCount + " V:" + this.Value.VertexCount.ToString() + "]";
        }


        public override string TypeDescription
        {
            get { return ("EdgeSpan Goo"); }
        }

        public override string TypeName
        {
            get { return ("EdgeSpan"); }
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
            if (typeof(Q).IsAssignableFrom(typeof(GH_Curve)))
            {
                if (Value == null)
                    target = default(Q);
                else
                    target = (Q)(object)(new GH_Curve(new PolylineCurve(this.Value.Vertices.Select(ind => Value.Mesh.GetVertex(ind).ToRhinoPt()))));
                return true;
            }
            else if (typeof(Q).IsAssignableFrom(typeof(EdgeLoop)))
                target = (Q)(object)(this.Value);

            target = default(Q);
            return false;
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new EdgeSpan_goo(Value);
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

        public static implicit operator EdgeSpan(EdgeSpan_goo Goo)
        {
            return Goo.Value;
        }

        public static implicit operator EdgeSpan_goo(EdgeSpan loop)
        {
            return new EdgeSpan_goo(loop);
        }
    }
}