![](https://github.com/joelhi/g3-gh/blob/main/src/media/mesh.png)

# g3-gh

This repository contains a set of components for the Grasshopper plugin for Rhino wrapping functionality from the [geometry3sharp](https://github.com/gradientspace/geometry3Sharp) library.

![](https://github.com/joelhi/g3-gh/blob/main/src/media/toolbar.png)

I found the tools in that library quite useful so, to kill some time during a global pandemic, I thought I'd expose some of them from grasshopper. 

## Installation

Build the project and as with most other gh plugins, just copy the g3gh.gha assembly and the geometry3sharp.dll to the grasshopper libraries folder.

Or go to the [releases](https://github.com/joelhi/g3-gh/releases) tab and download the built project. 

Should work for both Rhino on Mac and Windows. Mainly developed on mac, but have used it on Window as well and everything seemed fine.

## Current Functionality

**Remeshing**

- Iterative edge length remesher (with both internal and external iterations)
- Mesh face reducer
- Laplacian mesh smoother

**Marching Cubes**

- Point, curve and surface distance fields
- Marching Cube iso surface
- Mesh SDF


**Volumetric Opreations**

- Voxelization (from example)
- Inner lattice structure (from example)
- Mesh extrude

**Intersections**

- Implicit boolean operations (difference, union, intersection)
- Mesh | plane cut

**Mesh Repair** 

- AutoRepairG
- General mesh clean up utilities.

**Hole filler**

- Planar
- Smooth
- Minimal

**File Utility**

- Read from File (.obj / .off / .stl)
- Write to File (.obj / .off / .stl)

***

### Objects

Three core objects are implemented as goos with custom previews in the gh viewport.

- **DMesh3:** *Dynamic indexed mesh, main object used by the geometry3sharp library.*

- **Grid3f:** *3-dimensional grid of points with scalar float values. Used for the marching cubes algorithm.*

- **EdgeLoop:** *The edge loop describes an edge loop in a mesh. Used in this context mainly in combination with the hole fill methods.*

***

### Use

**Convert from and to Rhino Meshes**

Autocast, just plug into DMesh3 param / GH_Mesh param.

![](https://github.com/joelhi/g3-gh/blob/main/src/media/cast.png)









