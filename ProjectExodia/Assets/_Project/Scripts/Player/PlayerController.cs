using System;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace ProjectExodia
{
    [RequireComponent(typeof(InputSystemUIInputModule), typeof(PlayerInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputHandler inputHandler;

        [Header("Settings")] 
        [SerializeField] private float minimumDistance = .2f;
        [SerializeField] private float maximumTime = 1f;
        [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;

        private float _minDistSquared;
        private Vector2 _swipeStartPos;
        private Vector2 _swipeEndPos;
        private float _swipeStartTime;
        private float _swipeEndTime;

        private void Awake()
        {
            _minDistSquared = minimumDistance * minimumDistance;
            Debug.DrawLine(Vector3.zero, Vector3.one, Color.red, 600f);
        }

        private void OnEnable()
        {
            inputHandler.OnStartTouchEvent += BeginSwipe;
            inputHandler.OnEndTouchEvent += EndSwipe;
        }
        private void OnDisable()
        {
            inputHandler.OnStartTouchEvent -= BeginSwipe;
            inputHandler.OnEndTouchEvent -= EndSwipe;
        }

        private void BeginSwipe(Vector2 position, float time)
        {
            _swipeStartPos = position;
            _swipeStartTime = time;
        }
        
        private void EndSwipe(Vector2 position, float time)
        {
            _swipeEndPos = position;
            _swipeEndTime = time;
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            var direction = _swipeEndPos - _swipeStartPos;
            if (direction.sqrMagnitude < _minDistSquared)  return;
            if (_swipeEndTime - _swipeStartTime > maximumTime) return;
            
            ComputeSwipeDirection(direction.normalized);
            Debug.DrawLine(_swipeStartPos, _swipeEndPos, Color.red, 5f);
        }

        private void ComputeSwipeDirection(Vector2 normalizedDirection)
        {
            if (Vector2.Dot(Vector2.up, normalizedDirection) > directionThreshold)
            {
                Debug.Log("Swipe Up");
            }
            if (Vector2.Dot(Vector2.left, normalizedDirection) > directionThreshold)
            {
                Debug.Log("Swipe Left");
            }
            if (Vector2.Dot(Vector2.right, normalizedDirection) > directionThreshold)
            {
                Debug.Log("Swipe Right");
            }
            if (Vector2.Dot(Vector2.down, normalizedDirection) > directionThreshold)
            {
                Debug.Log("Swipe Down");
            }
        }
    }
}
