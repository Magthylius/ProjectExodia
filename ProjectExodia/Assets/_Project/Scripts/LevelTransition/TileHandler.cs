using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    [SerializeField] private List<MaterialInstance> tileMaterials;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    public void UpdateTile(Texture2D tex)
    {
        foreach (var tile in tileMaterials)
        {
            tile.Renderer.sharedMaterial.SetTexture(MainTex, tex);
        }
    }
}
