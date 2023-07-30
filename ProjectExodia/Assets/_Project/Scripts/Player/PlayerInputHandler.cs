using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectExodia
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Events
        public delegate void TouchEvent(Vector2 position, float time);
        public event TouchEvent OnStartTouchEvent;
        public event TouchEvent OnEndTouchEvent;
        #endregion
        
        private ProjectExodiaControls _playerControls;
        private Camera _mainCamera;
        
        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void Awake()
        {
            _playerControls = new ProjectExodiaControls();
        }
        
        private void Start()
        {
            _playerControls.Touch.PrimaryContact.started += OnStartPrimaryTouch;
            _playerControls.Touch.PrimaryContact.canceled += OnEndPrimaryTouch;
            _playerControls.Touch.SecondaryContact.started += OnStartSecondaryTouch;
            _playerControls.Touch.SecondaryContact.canceled += OnEndSecondaryTouch;
        }
        
        private void OnStartPrimaryTouch(InputAction.CallbackContext context)
        {
            OnStartTouchEvent?.Invoke(PrimaryWorldPosition, (float)context.startTime);
        }
        
        private void OnEndPrimaryTouch(InputAction.CallbackContext context)
        {
            OnEndTouchEvent?.Invoke(PrimaryWorldPosition, (float)context.time);
        }

        private void OnStartSecondaryTouch(InputAction.CallbackContext context)
        {
            OnStartTouchEvent?.Invoke(_playerControls.Touch.SecondaryPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
        
        private void OnEndSecondaryTouch(InputAction.CallbackContext context)
        {
            OnEndTouchEvent?.Invoke(_playerControls.Touch.SecondaryPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
        
        public void SetCamera(Camera playerCamera) => _mainCamera = playerCamera;

        public Vector2 PrimaryScreenPosition => _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        public Vector2 PrimaryWorldPosition =>
            GeneralUtils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
        
        public Ray PrimaryRay => 
            GeneralUtils.ScreenToRay(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
