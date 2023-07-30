using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountryPack", menuName = "Menu/CountryPack")]
public class CountryPack : ScriptableObject
{
    [SerializeField] private Texture2D floorTexture;
    [SerializeField] private Sprite backDrop;
    [SerializeField] private List<GameObject> enemies;

    public Texture2D FloorTexture => floorTexture;
    public Sprite BackDrop => backDrop;
    public List<GameObject> Enemies => enemies;
}
