using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectExodia
{
    [CreateAssetMenu(menuName = "Data/Game Init Data", fileName = "GameInitData")]
    public class GameInitData : ScriptableObject
    {
        [SerializeField] private InputActionAsset actionAsset;

        public InputActionAsset GetInputActionAsset() => actionAsset;
    }
}
