using UnityEngine;

namespace Farm2D
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] private int _damageInflicted;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Enemy enemy) && collision is BoxCollider2D)
            {
                StartCoroutine(enemy.DamageCharacter(_damageInflicted, 0f));
                gameObject.SetActive(false);
            }
        }
    }
}
