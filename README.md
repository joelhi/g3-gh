# g3-gh

{

Gif of remeshing here. As a teaser.

}

This repository contains a set of components for the Grasshopper plugin for Rhino wrapping functionality from the [geometry3sharp](https://github.com/gradientspace/geometry3Sharp) library.

![](https://github.com/joelhi/g3-gh/blob/main/src/media/toolbar.png)

I found the tools in that library quite useful so, to kill some time during a global pandemic, I thought I'd expose some of them from grasshopper. 

## Installation

Build the project and as with most other gh plugins, just copy the g3gh.gha assembly and the geometry3sharp.dll to the grasshopper libraries folder.

Should work for both Rhino on Mac and Windows. Mainly developed on mac, but have used it on Window as well and everything seemed fine.

## Current Functionality

### Remeshing

[Gifs from grasshopper rhino with some work being done?]

Smoothing

Remeshing based on edge lengths.

### Marching Cubes

### Voxelization / Lattices etc.

### Mesh Repair / Hole filler

### File Utility

## How to use it.

### Convert from - to Rhino Meshes.

Autocast, just plug into DMesh3 param / GH_Mesh param.

![](https://github.com/joelhi/g3-gh/blob/main/src/media/cast.png)









