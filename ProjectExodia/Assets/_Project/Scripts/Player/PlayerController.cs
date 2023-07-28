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
        
        private static Vector2 UpperLeft => new(-0.707106f, 0.707106f);
        private static Vector2 UpperRight => new(0.707106f, 0.707106f);
        private static Vector2 BottomLeft => new(-0.707106f, -0.707106f);
        private static Vector2 BottomRight => new(0.707106f, -0.707106f);

        private void Awake()
        {
            _minDistSquared = minimumDistance * minimumDistance;
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
            bool ExceedsDotThreshold(in Vector2 lhsDirection) =>
                Vector2.Dot(lhsDirection, normalizedDirection) > directionThreshold;
            
            //! TODO: Remove logs and implement swipes
            if (ExceedsDotThreshold(Vector2.up))
            {
                Debug.Log("Swipe Up");
            }
            else if (ExceedsDotThreshold(UpperLeft))
            {
                Debug.Log("Swipe Up Left");
            }
            else if (ExceedsDotThreshold(Vector2.left))
            {
                Debug.Log("Swipe Left");
            }
            else if (ExceedsDotThreshold(BottomLeft))
            {
                Debug.Log("Swipe Bottom Left");
            }
            else if (ExceedsDotThreshold(Vector2.down))
            {
                Debug.Log("Swipe Down");
            }
            else if (ExceedsDotThreshold(BottomRight))
            {
                Debug.Log("Swipe Bottom Right");
            }
            else if (ExceedsDotThreshold(Vector2.right))
            {
                Debug.Log("Swipe Right");
            }
            else if (ExceedsDotThreshold(UpperRight))
            {
                Debug.Log("Swipe Bottom Right");
            }
        }
        
    }
}
