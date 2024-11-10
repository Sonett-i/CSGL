# CSGL

**CSGL** is a C# project that leverages <a href="https://opentk.net/">**OpenTK**</a> to interface with **OpenGL**, allowing for the creation and rendering of 3D scenes. This library focuses on importing OBJ files, compiling shaders, and rendering 3D objects in a scene.

Last updated: 10 November, 2024

---
V1

![NVIDIA_Share_arU2zMec7o](https://github.com/user-attachments/assets/4dd78fb3-1c4b-472d-87b6-10f1a650e7cd)
***
V2

![Interface_aYTwmZI3gT](https://github.com/user-attachments/assets/cf739dde-ed60-42ef-93ae-6e0c436fda6d)

https://github.com/user-attachments/assets/44da7e48-933b-4661-93de-0f71ebfb1cdf

---


## New Features
- **First Person Camera**: 
  - The camera is able to explore the scene (WASD) with movement relative to the camera's forward and right vector.
  - The camera can be raised/lowered along it's up vector with (Q, E).
  - The camera can follow the player 'satellite' with the space bar.

- **Player Satellite**
  - Satellite solar panel meshes can be rotated using the arrow keys.
  - Satellite player can be oriented using numpad keys:
    -  numpad 4, 6: X-axis
    -  numpad 2, 8: Y-axis
    -  numpad 1, 9: Z-axis 
  - Supports different face definitions, allowing flexible importing of various OBJ files.
 
- **Cubemap**
  - A cubemap has been added which displays a 6 sided cube behind the scene's geometry
 
    
- **Resource Importing**
  - Textures, Models, Shaders and Materials are automatically found within the **\Resources\** directory.
  - Material.json files are correctly instanced during startup. 

## Features

- **Importing .obj Files**: 
  - Load and parse `.obj` files to extract vertex data, texture coordinates, and normals.
  - Supports different face definitions, allowing flexible importing of various OBJ files.

- **Rendering 3D Objects**:
  - Render complex 3D objects using OpenGL through OpenTK.
  - Supports different shapes including cubes, spheres, and custom imported models.
  - Provides functionality for setting up and managing vertex buffers, textures, and indices.

- **Shader Compilation**:
  - Compile vertex and fragment shaders from GLSL.
  - Manage shader program creation and attribute linking.
  - Supports setting shader uniforms for dynamic rendering.

## Future Development

- **.json Importing**: 
  - Scene.json for customizable scenes
  - Further additions to material.json
