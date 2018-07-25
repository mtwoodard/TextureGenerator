using System.Collections;
using System.Collections.Generic; //For list functionality
using UnityEditor;
using UnityEngine;

[AddComponentMenu("Noise/Noise Generator")]
public class NoiseGenerator : MonoBehaviour {
    public ComputeTexture[] computeTextures2D;
    public ComputeTexture3D[] computeTextures3D;

    public void Generate()
    {
        foreach(ComputeTexture ct in computeTextures2D){
            ct.CreateRenderTexture();
            ct.SetParameters();
            ct.SetTexture();
            ct.GenerateTexture();
            ct.SaveAsset();
        }
        foreach(ComputeTexture3D ct in computeTextures3D){
            ct.CreateRenderTexture();
            ct.SetParameters();
            ct.SetTexture();
            ct.GenerateTexture();
            ct.SaveAsset();
        }
    }
}

