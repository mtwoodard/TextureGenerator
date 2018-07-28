# Noise & Texture Generator for Unity
3D and 2D Texture generation using the compute shaders within the Unity engine.
![Image of 3D Noise](https://raw.githubusercontent.com/mtwoodard/NoiseGenerator/master/noiseGenerator.png)
## Purpose
This project handles the creation and serialization of different 3D/2D textures created via custom compute shaders. This is done through a custom **ComputeTexture** object.
## Why
I created this asset in order to generate 3D textures for use in raymarching systems. In fact the example provided is intended for use with a [volumetric cloud system][clouds] as described by the wonderful people at Guerilla Games. I found 3D textures to be wonderful however the lack of support was dissapointing.
## Use
There are three main scripts to know here: **ComputeTexture**, **ComputeTexture3D**, and **Noise Generator**. Note that since these scripts are based on MonoBehaviour they need to be attached to a game object, in the future I would rather these behave in a similar manner to Render Textures. In order to generate the completed texture you will need to provide the texture scripts with some information:
* Asset Name - what you want the asset to be named at save
* Kernel Name - the name of the compute shader kernel used to generate the final texture
* RW Texture - name of the RWTexture that will be written to by the compute shader
* Square Resolution - length, width, and depth resolution (i.e. 128 = 128 x 128 x 128)
* Parameters - list of float values and their corresponding compute shader name 
* Compute Threads - thread counts specified in the compute shader above each kernel (Z is only used in the case of 3D textures)
* Compute Shader - any compute shader that writes to RWTextures
* **3D ONLY** Texture 3D Slicer - compute shader that slices 3D textures to 2D layers, should be provided with the repo
## References
* greje656 [for the wonderful compute shader noise functions][greje]
* Nesvi [for the 3D render texture save functions][nesvi]

[clouds]: http://advances.realtimerendering.com/s2015/The%20Real-time%20Volumetric%20Cloudscapes%20of%20Horizon%20-%20Zero%20Dawn%20-%20ARTR.pdf
[greje]: https://bitsquid.blogspot.com/2016/07/volumetric-clouds.html
[nesvi]: http://answers.unity.com/answers/1243556/view.html
