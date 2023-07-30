using System.Collections;
using System.Collections.Generic;
using ProjectExodia;
using UnityEngine;

namespace ProjectExodia
{

    [CreateAssetMenu(fileName = "CountryPack", menuName = "Menu/CountryPack")]
    public class CountryPack : ScriptableObject
    {
        [SerializeField] private Texture2D floorTexture;
        [SerializeField] private Sprite backDrop;
        [SerializeField] private EntityBase[] enemies;

        public Texture2D FloorTexture => floorTexture;
        public Sprite BackDrop => backDrop;
        public EntityBase[] Enemies => enemies;
    }
}