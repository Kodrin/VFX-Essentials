# VFXGraph-Workshop

HDRP VFX-Graph workshop samples that go through some of the basic building blocks of working with particles in VFX-Graph. The samples go over waves, noise, point caches, textures, buffers, interactivity and more. Feel free to explore and use the samples!

![gif](Media/0.gif)


### Usage/License
**With the exception of the assets (3D models and associated textures)** , you are free to use the VFX graphs with no restrictions whatsoever. Crediting is appreciated but not necessary.

### Models from SketchFab
- [Niles Statue](https://sketchfab.com/3d-models/nile-42e02439c61049d681c897441d40aaa1#download)
- [Coptic Prayer Boo](https://sketchfab.com/3d-models/coptic-prayer-book-500ec4621d764f0dadb8edc93b8700ae#download)
- [Antique Book](https://sketchfab.com/3d-models/antique-leather-book-big-f62314240a0140a89e29119829aec000)

### References
- [RandomInsideSphere and ParticleIDToUV subgraphs by Keijiro](https://github.com/keijiro)


### Scenes
- 1_Waves (Creating curves, editing and animating them)

 ![jpg](Media/1.jpg)

- 2_Noise
  - Visualizing the different types of noise in the graph

 ![jpg](Media/2.jpg)

- 2_Textures2D
  - Showcasing how to use textures as data

 ![jpg](Media/3.jpg)

- 2_PointCloudJitter
  - Showcasing a point cloud generated from textures

 ![jpg](Media/4.jpg)

- 3_TextureBuffers
  - Using compute shaders to pass a texture to VFX graph and then read from it

 ![jpg](Media/5.jpg)

- 3_Boids
    - Computing boids on a compute shader and then passing it to VFX graph

![jpg](Media/6.jpg)

- 4_PointCacheStatue
    - Baking a point cache of an asset and using it as sub-emitters

![jpg](Media/7.jpg)

- 5_AutumnLeaves_Basic
    - VFX graph example of simple autumn leaves falling down

![jpg](Media/8.jpg)

- 5_MasterBook
    - VFX graph example showcasing how some of the building blocks in previous scenes

![jpg](Media/9.jpg)
