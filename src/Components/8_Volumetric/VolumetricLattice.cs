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
    public class VolumetricLattice : GH_Component
    {

        public VolumetricLattice()
          : base("VolumetricLattice", "latticeMsh",
            "Fill a DMesh3 object with a lightweight lattice structure",
            g3ghUtil.pluginName, "6_Volumetric")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Mesh to use as basis for lattice structure", GH_ParamAccess.item);
            pManager.AddNumberParameter("Lattice Radius", "r", "Radius of internal lattice elements.", GH_ParamAccess.item, 0.5);
            pManager.AddNumberParameter("Lattice Spacing", "d", "Spacing of internal lattice elements.", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Shell Thickness", "t", "Thickness of outer mesh shell.", GH_ParamAccess.item, 0.1);
            pManager.AddIntegerParameter("Resolution", "n", "Resolution of Grid for Marching Cubes.", GH_ParamAccess.item, 64);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DMesh3_Param(), "Mesh", "dm3", "Lattice structure of mesh volume", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DMesh3_goo goo = null;
            double lattice_radius = 0.05;
            double lattice_spacing = 0.4;
            double shell_thickness = 0.05;
            int mesh_resolution = 64;

            DA.GetData(0, ref goo);
            DA.GetData(1, ref lattice_radius);
            DA.GetData(2, ref lattice_spacing);
            DA.GetData(3, ref shell_thickness);
            DA.GetData(4, ref mesh_resolution);

            DMesh3 mesh = new DMesh3(goo.Value);

            var shellMeshImplicit = g3ghUtil.MeshToImplicit(mesh, 128, shell_thickness);
            double max_dim = mesh.CachedBounds.MaxDim;
            AxisAlignedBox3d bounds = new AxisAlignedBox3d(mesh.CachedBounds.Center, max_dim / 2);
            bounds.Expand(2 * lattice_spacing);
            AxisAlignedBox2d element = new AxisAlignedBox2d(lattice_spacing);
            AxisAlignedBox2d bounds_xy = new AxisAlignedBox2d(bounds.Min.xy, bounds.Max.xy);
            AxisAlignedBox2d bounds_xz = new AxisAlignedBox2d(bounds.Min.xz, bounds.Max.xz);
            AxisAlignedBox2d bounds_yz = new AxisAlignedBox2d(bounds.Min.yz, bounds.Max.yz);

            List<BoundedImplicitFunction3d> Tiling = new List<BoundedImplicitFunction3d>();
            foreach (g3.Vector2d uv in TilingUtil.BoundedRegularTiling2(element, bounds_xy, 0))
            {
                Segment3d seg = new Segment3d(new g3.Vector3d(uv.x, uv.y, bounds.Min.z), new g3.Vector3d(uv.x, uv.y, bounds.Max.z));
                Tiling.Add(new ImplicitLine3d() { Segment = seg, Radius = lattice_radius });
            }
            foreach (g3.Vector2d uv in TilingUtil.BoundedRegularTiling2(element, bounds_xz, 0))
            {
                Segment3d seg = new Segment3d(new g3.Vector3d(uv.x, bounds.Min.y, uv.y), new g3.Vector3d(uv.x, bounds.Max.y, uv.y));
                Tiling.Add(new ImplicitLine3d() { Segment = seg, Radius = lattice_radius });
            }
            foreach (g3.Vector2d uv in TilingUtil.BoundedRegularTiling2(element, bounds_yz, 0))
            {
                Segment3d seg = new Segment3d(new g3.Vector3d(bounds.Min.x, uv.x, uv.y), new g3.Vector3d(bounds.Max.x, uv.x, uv.y));
                Tiling.Add(new ImplicitLine3d() { Segment = seg, Radius = lattice_radius });
            }

            ImplicitNaryUnion3d lattice = new ImplicitNaryUnion3d() { Children = Tiling };
            ImplicitIntersection3d lattice_clipped = new ImplicitIntersection3d() { A = lattice, B = shellMeshImplicit };

            g3.MarchingCubes c = new g3.MarchingCubes();
            c.Implicit = lattice_clipped;
            c.RootMode = g3.MarchingCubes.RootfindingModes.LerpSteps;     
            c.RootModeSteps = 5;                                       
            c.Bounds = lattice_clipped.Bounds();
            c.CubeSize = c.Bounds.MaxDim / mesh_resolution;
            c.Bounds.Expand(3 * c.CubeSize);                           
            c.Generate();
            MeshNormals.QuickCompute(c.Mesh);                          


            DA.SetData(0, c.Mesh);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.g3_gh_icons_35;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8e74a4e3-fd43-4b40-87bf-f0b51a0ad676"); }
        }
    }
}
