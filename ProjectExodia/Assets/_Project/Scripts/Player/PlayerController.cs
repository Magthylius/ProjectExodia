using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace ProjectExodia
{
    [RequireComponent(typeof(PlayerInput), typeof(InputSystemUIInputModule))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        
        private void Awake()
        {
            playerInput.actions.FindAction("Move").performed += OnMoveAction;
        }
        
        private static void OnMoveAction(InputAction.CallbackContext callbackContext)
        {
            Debug.Log(callbackContext.ReadValue<Vector2>());
        }
    }
}
