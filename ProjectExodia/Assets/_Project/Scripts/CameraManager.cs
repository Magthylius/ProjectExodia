using Cinemachine;
using UnityEngine;

namespace ProjectExodia
{
    public class CameraManager : ManagerBase
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
        public void SetFollowTarget(Transform followTarget) => cinemachineFreeLook.Follow = followTarget;
        public Camera MainCamera => mainCamera;
    }
}
