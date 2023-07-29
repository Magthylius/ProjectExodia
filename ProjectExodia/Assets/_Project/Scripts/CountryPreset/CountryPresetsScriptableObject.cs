using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryPresetsScriptableObject : ScriptableObject
{
    [SerializeField] private Texture2D floorTexture;
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private GameObject[] props;
}
