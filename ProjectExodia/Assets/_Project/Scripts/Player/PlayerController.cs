using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace ProjectExodia
{
    public class PlayerController : MonoBehaviour
    {
        public void Initialize(GameInitData gameInitData)
        {
            var input = gameObject.AddComponent<PlayerInput>();
            var inputModule = gameObject.AddComponent<InputSystemUIInputModule>();
            input.actions = gameInitData.GetInputActionAsset();
            input.uiInputModule = inputModule;

            input.ActivateInput();
        }

        public void OnMove(InputValue inputValue)
        {
            Debug.Log(inputValue.Get<Vector2>());
        }
    }
}
