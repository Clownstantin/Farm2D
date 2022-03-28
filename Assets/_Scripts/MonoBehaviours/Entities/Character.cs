using System.Collections;
using UnityEngine;

namespace Farm2D
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected float startingHP = 5;
        [SerializeField] protected float maxHealth = 10;

        public float MaxHealth => maxHealth;

        public virtual void KillCharacter() => Destroy(gameObject);

        public abstract void ResetCharacter();

        public abstract IEnumerator DamageCharacter(float damage, float interval);
    }
}