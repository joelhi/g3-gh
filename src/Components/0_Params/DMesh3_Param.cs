using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel.Types;

using gh3sharp.Core;
using gh3sharp.Core.Goos;
using g3;

namespace gh3sharp.Components.Params
{
    public class DMesh3_Param : GH_Param<DMesh3_goo>
    {

        public DMesh3_Param() :
            base("DMesh3 Parameter", "DMesh3", "Holds a DMesh3 Object", gh3sharpUtil.pluginName, "0_params", GH_ParamAccess.item)
        { }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }


        
        protected override DMesh3_goo PreferredCast(object data)
        {
            if (data is Rhino.Geometry.Mesh)
            {
                return new DMesh3_goo((Rhino.Geometry.Mesh)data);
            }
            else if (data is DMesh3)
            {
                return new DMesh3_goo((DMesh3)data);
            }
            else if (data is GH_Mesh)
            {
                return new DMesh3_goo(((GH_Mesh)data).Value);
            }
            else
                return null;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b55b2356-c963-4eca-811b-20b86f9dc4c0"); }
        }
    }
}
