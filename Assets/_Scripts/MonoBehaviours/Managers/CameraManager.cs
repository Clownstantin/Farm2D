using Cinemachine;
using UnityEngine;

namespace Farm2D
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public void FollowPlayer(Player player) => _virtualCamera.Follow = player.transform;
    }
}
