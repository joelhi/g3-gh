using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using g3;

using gh3sharp.Core;

namespace gh3sharp.Core.Goos
{

    public class DMesh3_goo : GH_Goo<DMesh3>
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
                dispMsh = this.Value.ToRhino();
            
        }

        public override string ToString()
        {
            return this.Value.ToString();
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

        public override object ScriptVariable()
        {
            return base.ScriptVariable();
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