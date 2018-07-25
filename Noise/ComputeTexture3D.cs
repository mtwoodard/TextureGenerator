using System.Collections;
using UnityEditor;
using UnityEngine;

[AddComponentMenu("Noise/Compute Texture 3D")]
public class ComputeTexture3D : ComputeTexture {
    public ComputeShader texture3DSlicer;

    //-------------------------------------------------------------------------------------------------------------------
	// Generator Functions
	//-------------------------------------------------------------------------------------------------------------------
    public override void GenerateTexture(){
        int kernel = computeShader.FindKernel(kernelName);
        computeShader.Dispatch(kernel, 
            squareResolution/computeThreads.x, 
            squareResolution/computeThreads.y, 
            squareResolution/computeThreads.z);
    }

    public override void CreateRenderTexture(){
        //24 is the bits of the depth buffer not the resolution of the z-direction
        RenderTexture rt = new RenderTexture(squareResolution, squareResolution, 24, RenderTextureFormat.ARGB32);
        rt.enableRandomWrite = true;
        rt.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        rt.volumeDepth = squareResolution;
        rt.Create();
        rwTexture.rt = rt;
    }

    //-------------------------------------------------------------------------------------------------------------------
	// Save/Utility Functions
	//-------------------------------------------------------------------------------------------------------------------
    RenderTexture Copy3DSliceToRenderTexture(int layer){
        RenderTexture render = new RenderTexture(squareResolution, squareResolution, 0, RenderTextureFormat.ARGB32);
		render.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
		render.enableRandomWrite = true;
		render.wrapMode = TextureWrapMode.Clamp;
		render.Create();

        int kernelIndex = texture3DSlicer.FindKernel("CSMain");
        texture3DSlicer.SetTexture(kernelIndex, "noise", rwTexture.rt);
        texture3DSlicer.SetInt("layer", layer);
        texture3DSlicer.SetTexture(kernelIndex, "Result", render);
        texture3DSlicer.Dispatch(kernelIndex, squareResolution, squareResolution, 1);

        return render;
    }
   
    public override void SaveAsset(){
        //for readability
        int dim = squareResolution;
        //Slice 3D Render Texture to individual layers
        RenderTexture[] layers = new RenderTexture[squareResolution];
        for(int i = 0; i < squareResolution; i++)
            layers[i] = Copy3DSliceToRenderTexture(i);
        //Write RenderTexture slices to static textures
        Texture2D[] finalSlices = new Texture2D[squareResolution];
        for(int i = 0; i < squareResolution; i++)
            finalSlices[i] = ConvertFromRenderTexture(layers[i]);
        //Build 3D Texture from 2D slices
        Texture3D output = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, true);
        output.filterMode = FilterMode.Trilinear;
        Color[] outputPixels = output.GetPixels();
        for(int k = 0; k < dim; k++){
            Color[] layerPixels = finalSlices[k].GetPixels();
            for(int i = 0; i < dim; i++){
                for(int j = 0; j < dim; j++){
                    outputPixels[i + j * dim + k * dim * dim] = layerPixels[i+j*dim];
                }
            }
        }

        output.SetPixels(outputPixels);
        output.Apply();

        AssetDatabase.CreateAsset(output, "Assets/Noise/" + assetName + ".asset");
    }
}
