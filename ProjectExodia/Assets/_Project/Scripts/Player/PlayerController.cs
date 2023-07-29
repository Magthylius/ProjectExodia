using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;

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
        public delegate void PlayerEvent(PlayerCharacter playerCharacter);
        public event SwipeEvent OnPlayerSwiped;
        public event SlapEvent OnEntitySlapped;
        public event PlayerEvent OnPlayerTriggered;

        [Header("References")]
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private PlayerCharacter playerCharacterPrefab;
        [SerializeField] private TrailRenderer trailRendererPrefab;

        [Header("Settings - Player")] 
        [SerializeField] private float playerSpeed = 50;
        [SerializeField] private float xMovementRange = 5f;
        [SerializeField] private Vector3 spawnOffset;
        [SerializeField, Layer] private int playerLayer = 7;

        [Header("Settings - Tap")] 
        [SerializeField] private float tapDistance = 100;
        [SerializeField, Layer] private int entityLayer = 6;
        
        [Header("Settings - Drag")] 
        [SerializeField] private float dragMaxDistance = 100f;
        [SerializeField] private float dragSensitivity = 0.2f;

        [Header("Settings - Swipe")] 
        [SerializeField] private float minimumDistance = .2f;
        [SerializeField] private float maximumTime = 1f;
        [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;
        [SerializeField] private float swipeSphereRadius = 0.01f;
        
        [Header("Settings - Debug")]
        [SerializeField] private bool wantsDebug = false;
        [SerializeField] private bool stopMovement = false;

        public PlayerCharacter Character { get; private set; }

        private TrailRenderer _trailRenderer;
        private Coroutine _dragRoutine;
        
        private Vector2 _swipeStartPos;
        private Vector2 _swipeEndPos;
        private float _swipeStartTime;
        private float _swipeEndTime;

        private Vector2 _dragStartPos;
        private float _dragVariance;
        
        private static Vector2 UpLeft => new(-0.707106f, 0.707106f);
        private static Vector2 UpRight => new(0.707106f, 0.707106f);
        private static Vector2 DownLeft => new(-0.707106f, -0.707106f);
        private static Vector2 DownRight => new(0.707106f, -0.707106f);

        private void OnEnable()
        {
            inputHandler.OnStartTouchEvent += BeginTap;
            inputHandler.OnStartTouchEvent += BeginDrag;
            inputHandler.OnEndTouchEvent += EndDrag;
        }
        
        private void OnDisable()
        {
            inputHandler.OnStartTouchEvent -= BeginTap;
            inputHandler.OnStartTouchEvent -= BeginDrag;
            inputHandler.OnEndTouchEvent -= EndDrag;
        }
        
        private void Update()
        {
            if (!Character || stopMovement) return;
            
            var movement = new Vector3(_dragVariance * dragSensitivity, 0f, 1f) * (playerSpeed * Time.deltaTime);
            var newPosition = Character.transform.position += movement;
            newPosition.x = Mathf.Clamp(newPosition.x, -xMovementRange, xMovementRange);
            
            // ReSharper disable once Unity.InefficientPropertyAccess
            Character.transform.position = newPosition;
        }

        public void Initialize(CameraManager manager)
        {
            _trailRenderer = Instantiate(trailRendererPrefab, transform);
            Character = Instantiate(playerCharacterPrefab, spawnOffset, Quaternion.identity);
            manager.SetFollowTarget(Character.transform);
            inputHandler.SetCamera(manager.MainCamera);
        }

        private void BeginTap(Vector2 position, float time)
        {
            var mouseRay = inputHandler.PrimaryRay;
            if (wantsDebug) Debug.DrawRay(mouseRay.origin, mouseRay.direction);

            if (!Physics.Raycast(mouseRay, out var hitInfo, tapDistance)) return;

            var hitLayer = hitInfo.transform.gameObject.layer;
            if (hitLayer == entityLayer)
            {
                var entity = hitInfo.transform.GetComponent<ISlappableEntity>();
                if (entity == null) return;
            
                var slapSuccessful = entity.PerformSlap();
                if (slapSuccessful) OnEntitySlapped?.Invoke(entity);
            
                if (wantsDebug) Debug.Log($"I hit {hitInfo.transform}");
            }
            else if (hitLayer == playerLayer)
            {
                var player = hitInfo.transform.GetComponent<PlayerCharacter>();
                if (!player) return;
                
                OnPlayerTriggered?.Invoke(player);
            }
        }

        #region Swipe Functions
        private void BeginDrag(Vector2 position, float time)
        {
            _swipeStartPos = position;
            _swipeStartTime = time;
            
            _trailRenderer.gameObject.SetActive(true);
            _trailRenderer.transform.position = position;
            _trailRenderer.Clear();
            
            _dragRoutine = StartCoroutine(DragUpdate());
            _dragStartPos = inputHandler.PrimaryScreenPosition;
            
            IEnumerator DragUpdate()
            {
                while (true)
                {
                    var distance = inputHandler.PrimaryScreenPosition.x - _dragStartPos.x;
                    _dragVariance = Mathf.Clamp(distance / dragMaxDistance, -1f, 1f);

                    _trailRenderer.transform.position = inputHandler.PrimaryWorldPosition;
                    yield return null;
                }
            }
        }
        
        private void EndDrag(Vector2 position, float time)
        {
            _swipeEndPos = position;
            _swipeEndTime = time;

            _dragVariance = 0f;
            _trailRenderer.gameObject.SetActive(false);
            if (_dragRoutine != null) StopCoroutine(_dragRoutine);
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
        #endregion
    }
}
