﻿using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace gh_3sharp
{
    public class gh_3sharpInfo : GH_AssemblyInfo
    {
        public override string Name => "gh_3sharp Info";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Grasshopper library to expose some functionality from the geometry3sharp library";

        public override Guid Id => new Guid("0248B474-5DB3-48C3-850E-455A32DEE9D8");

        //Return a string identifying you or your company.
        public override string AuthorName => "JoelHilmersson";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "d.j.hilmersson@gmail.com";
    }
}