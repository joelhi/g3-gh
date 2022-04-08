using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using g3gh.Core;
using g3gh.Core.Goos;
using g3gh.Components.Params;

using g3;

namespace g3gh.Components.Volumetric
{
    public class Voxelize : GH_Component
    {

        public Voxelize()
          : base("Voxelize", "Voxl",
            "Voxelize a mesh using a bitmap. Returns a voxelized surface mesh.",
            g3ghUtil.pluginName, "6_Volumetric")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(),"Mesh","dm3","Mesh to map as a voxel mesh.",GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number of Cells", "n", "Number of cells (Resolution)", GH_ParamAccess.item, 64);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(),"Voxel Mesh","dm3","The resutling meshed bitmap.",GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            int num_cells = 128;
            DMesh3_goo dMsh_goo = null;

            DA.GetData(0, ref dMsh_goo);
            DA.GetData(1, ref num_cells);

            DMesh3 dMsh_copy = new DMesh3(dMsh_goo.Value);
            double cell_size = dMsh_copy.CachedBounds.MaxDim / num_cells;

            DMeshAABBTree3 spatial = new DMeshAABBTree3(dMsh_copy, autoBuild: true);

            AxisAlignedBox3d bounds = dMsh_copy.CachedBounds;
            double cellsize = bounds.MaxDim / num_cells;
            ShiftGridIndexer3 indexer = new ShiftGridIndexer3(bounds.Min, cellsize);

            Bitmap3 bmp = new Bitmap3(new Vector3i(num_cells, num_cells, num_cells));
            foreach (Vector3i idx in bmp.Indices())
            {
                g3.Vector3d v = indexer.FromGrid(idx);
                bmp.Set(idx, spatial.IsInside(v));
            }

            VoxelSurfaceGenerator voxGen = new VoxelSurfaceGenerator();

            voxGen.Voxels = bmp;
            voxGen.Generate();
            DMesh3 voxMesh = voxGen.Meshes[0];

            var vecSize = dMsh_copy.CachedBounds.Extents;
            var box = dMsh_copy.GetBounds();

            // Scale voxel mesh
            //MeshTransforms.Scale(voxMesh,)

            DA.SetData(0, voxMesh);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_20;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("9bb0e2bd-7463-4150-a94a-1997f26095d3"); }
        }
    }
}
