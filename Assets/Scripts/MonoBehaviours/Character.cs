using UnityEngine;

namespace Farm2D
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 10;

        protected int currentHealth = 5;
    }
}