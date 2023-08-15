using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using g3;

using g3gh.Core;
using Grasshopper.Kernel;
using System.Drawing;

namespace g3gh.Core.Goos
{

    public class EdgeLoop_goo : GH_GeometricGoo<EdgeLoop>, IGH_PreviewData
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

        public override BoundingBox Boundingbox => this.m_value.GetBounds().ToRhino();

        public BoundingBox ClippingBox => this.m_value.GetBounds().ToRhino();

        public override object ScriptVariable()
        {
            return base.ScriptVariable();
        }

        public override bool CastTo<Q>(out Q target)
        {

            //Cast to curve.
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

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            this.GenerateDispCurves();

            args.Pipeline.DrawPolyline(this.loop.ToPolyline(), Color.MediumVioletRed, 2);
        }

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {

        }

        public static implicit operator EdgeLoop(EdgeLoop_goo Goo)
        {
            return Goo.Value;
        }

        public static implicit operator EdgeLoop_goo(EdgeLoop loop)
        {
            return new EdgeLoop_goo(loop);
        }
    }
}