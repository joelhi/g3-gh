using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using g3;

using gh3sharp.Core;

namespace gh3sharp.Core.Goos
{

    public class Grid3f_goo : GH_GeometricGoo<DenseGrid3f>
    {
        public Point3d[] dispPts = null;

        public Grid3f_goo()
        {

            this.Value = null;
        }

        public Grid3f_goo(DenseGrid3f grid)
        {
            this.Value = grid;
        }

        public void GenerateDispPts()
        {

        }

        public override string ToString()
        {
            return this.Value.ToString();
        }


        public override string TypeDescription
        {
            get { return ("Grid3f Goo"); }
        }

        public override string TypeName
        {
            get { return ("Grid3f"); }
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


        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new Grid3f_goo(new DenseGrid3f(this.Value));
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