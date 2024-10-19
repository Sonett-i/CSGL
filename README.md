# CSGL

Last updated: 20 October, 2024

**CSGL** is a C# project that leverages <a href="https://opentk.net/">**OpenTK**</a> to interface with **OpenGL**, allowing for the creation and rendering of 3D scenes. This library focuses on importing OBJ files, compiling shaders, and rendering 3D objects in a scene.

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

- **.obj Importing**: 
  - Supporting groups for the display of complex geometry.
  - Support for .mtl files.

- **.fbx Importing**: 
  - Import models using fbx format.

