using System.Collections;
using UnityEngine;

namespace Farm2D
{
    public class Enemy : Character
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _damageInterval = 1f;

        private float _currentHealth;
        private Coroutine _damageCoroutine;

        private void OnEnable() => ResetCharacter();

        private void OnCollisionEnter2D(Collision2D collision) => DetectPlayer(collision);

        private void OnCollisionExit2D(Collision2D collision) => DetectPlayer(collision);

        public override IEnumerator DamageCharacter(float damage, float interval)
        {
            var delay = new WaitForSeconds(interval);

            while (true)
            {
                StartCoroutine(FlickerCharacter());

                _currentHealth -= damage;

                if (_currentHealth < float.Epsilon)
                {
                    KillCharacter();
                    break;
                }

                if (interval > float.Epsilon) yield return delay;
                else break;
            }
        }

        public override void ResetCharacter() => _currentHealth = startingHP;

        private void DetectPlayer(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out Player player))
            {
                if (_damageCoroutine != null)
                {
                    StopCoroutine(_damageCoroutine);
                    _damageCoroutine = null;
                }
                else
                    _damageCoroutine = StartCoroutine(player.DamageCharacter(_damage, _damageInterval));
            }
        }
    }
}