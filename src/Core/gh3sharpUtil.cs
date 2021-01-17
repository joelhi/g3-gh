using System;

using g3;

namespace gh3sharp.Core
{
    public static class gh3sharpUtil
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
    }

    


}
