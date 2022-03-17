using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Farm2D
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Character _charPrefab;
        [Tooltip("While > 1 spawn continues.")]
        [SerializeField] private float _spawnDelay = 0f;

        private CancellationTokenSource _tokenSource;

        private async void Start()
        {
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            await SpawnEnemy(_charPrefab, token);
        }

        public Player SpawnPlayer() => _charPrefab is Player ? (Player)Instantiate(_charPrefab, transform) : null;

        public void StopSpawn()
        {
            _tokenSource.Cancel();
        }

        private async UniTask SpawnEnemy(Character enemy, CancellationToken token)
        {
            var delay = TimeSpan.FromSeconds(_spawnDelay);

            while (_spawnDelay > 0)
            {
                if (token.IsCancellationRequested)
                {
                    _tokenSource.Dispose();
                    _tokenSource = null;
                    return;
                }

                var newEnemy = (Enemy)Instantiate(enemy, transform);

                await UniTask.Delay(delay, cancellationToken: token);
            }
        }
    }
}
