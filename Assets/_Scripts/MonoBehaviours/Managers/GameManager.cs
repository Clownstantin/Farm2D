using UnityEngine;

namespace Farm2D
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Canvas _gameUI;
        [SerializeField] private Spawner _playerSpawner;

        private Player _player;

        private void Start() => SetupScene();

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }

        private void SetupScene()
        {
            var virtualCamera = CameraManager.Instance;
            _player = _playerSpawner.SpawnPlayer();
            _player.Init(_gameUI);
            virtualCamera.FollowPlayer(_player);
        }
    }
}