using System.Collections;
using UnityEngine;

namespace Farm2D
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected float startingHP = 5;
        [SerializeField] protected float maxHealth = 10;

        private SpriteRenderer _spriteRenderer;

        public float MaxHealth => maxHealth;

        private void Awake() => _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        public virtual void KillCharacter() => Destroy(gameObject);

        public virtual IEnumerator FlickerCharacter()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            _spriteRenderer.color = Color.white;
        }

        public abstract void ResetCharacter();

        public abstract IEnumerator DamageCharacter(float damage, float interval);
    }
}