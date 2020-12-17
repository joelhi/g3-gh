using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using g3;

using gh3sharp.Core;

namespace gh3sharp.Core.Goos
{

    public class Grid3f_goo : GH_GeometricGoo<DenseGridTrilinearImplicit>
    {
        public Point3d[] dispPts = null;

        public Grid3f_goo()
        {

            this.Value = null;
        }

        public Grid3f_goo(DenseGridTrilinearImplicit grid)
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
            get { return ("DenseGrid3f"); }
        }

        public override IGH_Goo Duplicate()
        {
            return this;
        }

        public override bool IsValid
        {
            get { return !(Value is null); }
        }

        public override BoundingBox Boundingbox => this.Value.Bounds().ToRhino();

        public override object ScriptVariable()
        {
            return base.ScriptVariable();
        }


        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new Grid3f_goo(this.Value);
        }

        public override BoundingBox GetBoundingBox(Transform xform)
        {
            return this.Value.Bounds().ToRhino();
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