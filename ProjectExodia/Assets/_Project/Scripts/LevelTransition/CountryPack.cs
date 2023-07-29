using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountryPack", menuName = "Menu/CountryPack")]
public class CountryPack : ScriptableObject
{
    [SerializeField] private Texture2D floorTexture;
    [SerializeField] private Texture2D backDrop;
    [SerializeField] private List<GameObject> enemies;

    public Texture2D FloorTexture => floorTexture;
    public Texture2D BackDrop => backDrop;
    public List<GameObject> Enemies => enemies;
}
