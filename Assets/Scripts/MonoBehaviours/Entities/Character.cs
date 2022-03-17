using UnityEngine;

namespace Farm2D
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected float startingHP = 5;
        [SerializeField] protected float maxHealth = 10;
        [SerializeField] protected HitPoints hitPoints;

        public float MaxHealth => maxHealth;

        public virtual void KillCharacter() => Destroy(gameObject);
    }
}