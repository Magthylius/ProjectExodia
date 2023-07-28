using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectExodia
{
    public class PlayerInputHandler : MonoBehaviour
    {
        /*
         * TODO: Cache player camera
         */
        
        #region Events
        public delegate void TouchEvent(Vector2 position, float time);
        public event TouchEvent OnStartTouchEvent;
        public event TouchEvent OnEndTouchEvent;
        #endregion
        
        private ProjectExodiaControls _playerControls;
        
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
        }

        private void OnStartPrimaryTouch(InputAction.CallbackContext context)
        {
            OnStartTouchEvent?.Invoke(PrimaryPosition, (float)context.startTime);
        }
        
        private void OnEndPrimaryTouch(InputAction.CallbackContext context)
        {
            OnEndTouchEvent?.Invoke(PrimaryPosition, (float)context.time);
        }

        public Vector2 PrimaryPosition =>
            GeneralUtils.ScreenToWorld(Camera.main, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
