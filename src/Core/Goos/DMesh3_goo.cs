using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using g3;

using g3gh.Core;

namespace g3gh.Core.Goos
{

    public class DMesh3_goo : GH_GeometricGoo<DMesh3>
    {
        public Mesh dispMsh = null;

        public DMesh3_goo()
        {

            this.Value = null;
        }

        public DMesh3_goo(DMesh3 ms)
        {
            Value = ms;
            
        }

        public DMesh3_goo(Mesh ms)
        {
            Value = ms.ToDMesh3();
        }

        public void GenerateDispMesh()
        {
            if (dispMsh == null)
            { 
                dispMsh = this.Value.ToRhino();
            }
        }

        public override string ToString()
        {
            return "DMesh3 [V: " + this.Value.VertexCount.ToString() + "F: " + this.Value.TriangleCount.ToString() + "]";
        }


        public override string TypeDescription
        {
            get { return ("DMesh3 Goo"); }
        }

        public override string TypeName
        {
            get { return ("DMesh3"); }
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
            if (typeof(Q).IsAssignableFrom(typeof(GH_Mesh)))
            {
                if (Value == null)
                    target = default(Q);
                else
                    target = (Q)(object)(new GH_Mesh(Value.ToRhino()));
                return true;
            }

            target = default(Q);
            return false;
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new DMesh3_goo(new DMesh3(Value));
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

        public static implicit operator DMesh3(DMesh3_goo dmshGoo)
        {
            return dmshGoo.Value;
        }

        public static implicit operator DMesh3_goo(DMesh3 dMesh3)
        {
            return new DMesh3_goo(dMesh3);
        }

        public static implicit operator Mesh(DMesh3_goo dmshGoo)
        {
            return dmshGoo.Value.ToRhino();
        }

        public static implicit operator DMesh3_goo(Mesh dMesh3)
        {
            return new DMesh3_goo(dMesh3);
        }
    }
}