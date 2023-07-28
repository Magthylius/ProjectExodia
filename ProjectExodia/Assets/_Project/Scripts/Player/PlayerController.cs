using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace ProjectExodia
{
    public enum SwipeDirection
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
    
    [RequireComponent(typeof(InputSystemUIInputModule), typeof(PlayerInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        public delegate void SwipeEvent(SwipeDirection direction);
        public event SwipeEvent OnPlayerSwiped;

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
        
        private static Vector2 UpLeft => new(-0.707106f, 0.707106f);
        private static Vector2 UpRight => new(0.707106f, 0.707106f);
        private static Vector2 DownLeft => new(-0.707106f, -0.707106f);
        private static Vector2 DownRight => new(0.707106f, -0.707106f);

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

            if (ExceedsDotThreshold(Vector2.up)) OnPlayerSwiped?.Invoke(SwipeDirection.Up);
            else if (ExceedsDotThreshold(UpRight)) OnPlayerSwiped?.Invoke(SwipeDirection.UpRight);
            else if (ExceedsDotThreshold(Vector2.right)) OnPlayerSwiped?.Invoke(SwipeDirection.Right);
            else if (ExceedsDotThreshold(DownRight)) OnPlayerSwiped?.Invoke(SwipeDirection.DownRight);
            else if (ExceedsDotThreshold(Vector2.down)) OnPlayerSwiped?.Invoke(SwipeDirection.Down);
            else if (ExceedsDotThreshold(DownLeft)) OnPlayerSwiped?.Invoke(SwipeDirection.DownLeft);
            else if (ExceedsDotThreshold(Vector2.left)) OnPlayerSwiped?.Invoke(SwipeDirection.Left);
            else if (ExceedsDotThreshold(UpLeft)) OnPlayerSwiped?.Invoke(SwipeDirection.UpLeft);
        }
    }
}
