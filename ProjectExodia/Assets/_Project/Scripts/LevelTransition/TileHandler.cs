using System;
using System.Collections;
using System.Collections.Generic;
using ProjectExodia;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    [SerializeField] private List<MaterialInstance> tileMaterials;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private void OnEnable()
    {
        LevelTransitionManager.OnCountryChange += UpdateTile;
    }

    private void OnDisable()
    {
        LevelTransitionManager.OnCountryChange -= UpdateTile;
    }

    public void UpdateTile(CountryPack country)
    {
        foreach (var tile in tileMaterials)
        {
            tile.Renderer.sharedMaterial.SetTexture(MainTex, country.FloorTexture);
        }
    }
}
