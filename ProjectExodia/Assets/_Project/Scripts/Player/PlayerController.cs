using System.Collections;
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
        public delegate void SlapEvent(ISlappableEntity entity);
        public event SwipeEvent OnPlayerSwiped;
        public event SlapEvent OnEntitySlapped;

        [Header("References")]
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private TrailRenderer trailRendererPrefab;

        [Header("Settings - Player")] 
        [SerializeField] private float playerSpeed = 50;
        
        [Header("Settings - Swipe")] 
        [SerializeField] private float minimumDistance = .2f;
        [SerializeField] private float maximumTime = 1f;
        [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;
        [SerializeField] private float swipeSphereRadius = 0.01f;
        [SerializeField] private bool wantsDebug = false;

        public Transform PlayerTransform { get; private set; }
        
        private TrailRenderer _trailRenderer;
        private Coroutine _trailUpdateRoutine;
        
        private Vector2 _swipeStartPos;
        private Vector2 _swipeEndPos;
        private float _swipeStartTime;
        private float _swipeEndTime;
        
        private static Vector2 UpLeft => new(-0.707106f, 0.707106f);
        private static Vector2 UpRight => new(0.707106f, 0.707106f);
        private static Vector2 DownLeft => new(-0.707106f, -0.707106f);
        private static Vector2 DownRight => new(0.707106f, -0.707106f);

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
        
        private void Awake()
        {
            _trailRenderer = Instantiate(trailRendererPrefab, transform);
            PlayerTransform = new GameObject("PlayerPawn").transform;
            PlayerTransform.SetParent(transform);
            playerCamera.transform.SetParent(PlayerTransform);
        }

        private void Start()
        {
            inputHandler.SetCamera(playerCamera);
        }

        private void Update()
        {
            if (!PlayerTransform) return;
            PlayerTransform.Translate(Vector3.forward * (playerSpeed * Time.deltaTime), Space.World);
        }

        private void BeginSwipe(Vector2 position, float time)
        {
            _swipeStartPos = position;
            _swipeStartTime = time;
            _trailRenderer.gameObject.SetActive(true);
            _trailRenderer.transform.position = position;
            _trailRenderer.Clear();
            _trailUpdateRoutine = StartCoroutine(TrailUpdate());
            
            IEnumerator TrailUpdate()
            {
                while (true)
                {
                    _trailRenderer.transform.position = inputHandler.PrimaryPosition;
                    yield return null;
                }
            }
        }
        
        private void EndSwipe(Vector2 position, float time)
        {
            _swipeEndPos = position;
            _swipeEndTime = time;
            _trailRenderer.gameObject.SetActive(false);
            if (_trailUpdateRoutine != null) StopCoroutine(_trailUpdateRoutine);
            
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            var distance = Vector3.Distance(_swipeEndPos, _swipeStartPos);
            if (distance < minimumDistance)  return;
            if (_swipeEndTime - _swipeStartTime > maximumTime) return;
            
            var direction = (_swipeEndPos - _swipeStartPos).normalized;
            ComputeSwipeDirection(direction);
            if (Physics.SphereCast(_swipeStartPos, swipeSphereRadius, direction, out var hitInfo, distance))
            {
                if (hitInfo.transform.TryGetComponent<ISlappableEntity>(out var entity))
                {
                    entity.PerformSlap();
                    OnEntitySlapped?.Invoke(entity);
                    if (wantsDebug) Debug.Log($"PlayerController: Entity slapped [{entity.GetName()}]");
                }
            }
            
            if (wantsDebug) Debug.DrawLine(_swipeStartPos, _swipeEndPos, Color.red, 5f);
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
