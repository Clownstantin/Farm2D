using System;
using UnityEngine;

namespace Farm2D
{
    [CreateAssetMenu(menuName = "HitPoints", order = 51)]
    public class HitPoints : ScriptableObject
    {
        [SerializeField] private float _health;

        public float Health => _health;

        public event Action<float> OnHpChanged;

        public void SetHealth(float value)
        {
            _health = value;
            OnHpChanged?.Invoke(_health);
        }
    }
}