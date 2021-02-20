using System;

using g3;

namespace g3gh.Core
{
    public static class g3ghUtil
    {
        public const string pluginName = "g3";

        public static BoundedImplicitFunction3d MeshToImplicit(DMesh3 meshIn, int numcells, double max_offset)
        {
            double meshCellsize = meshIn.CachedBounds.MaxDim / numcells;
            MeshSignedDistanceGrid levelSet = new MeshSignedDistanceGrid(meshIn, meshCellsize);
            levelSet.ExactBandWidth = (int)(max_offset / meshCellsize) + 1;
            levelSet.Compute();
            return new DenseGridTrilinearImplicit(levelSet.Grid, levelSet.GridOrigin, levelSet.CellSize);
        }

        public static Vector3f Multiply(this Vector3f vec,double val)
        {
            vec.x *= (float)val;
            vec.y *= (float)val;
            vec.z *= (float)val;

            return vec;
        }
    }

    


}
