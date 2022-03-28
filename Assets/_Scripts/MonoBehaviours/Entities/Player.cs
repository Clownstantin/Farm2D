using System.Collections;
using UnityEngine;

namespace Farm2D
{
    public class Player : Character
    {
        [SerializeField] private HitPoints _hitPoints;
        [Header("UI Elements")]
        [SerializeField] private HealthBar _healthBarPrefab;
        [SerializeField] private Inventory _inventoryPrefab;

        private Inventory _inventory;
        private HealthBar _healthBar;
        private Canvas _gameUI;

        private void Start() => ResetCharacter();

        private void OnTriggerEnter2D(Collider2D collision) => DetectConsumableItem(collision);

        public bool AdjustHealth(int amount)
        {
            if (_hitPoints.Health < maxHealth)
            {
                _hitPoints.SetHealth(_hitPoints.Health + amount);
                return true;
            }
            return false;
        }

        public void Init(Canvas canvas) => _gameUI = canvas;

        public override void ResetCharacter()
        {
            _hitPoints.SetHealth(startingHP);

            _healthBar = Instantiate(_healthBarPrefab, _gameUI.transform);
            _healthBar.Init(this);

            _inventory = Instantiate(_inventoryPrefab, _gameUI.transform);
        }

        public override IEnumerator DamageCharacter(float damage, float interval)
        {
            var delay = new WaitForSeconds(interval);

            while (true)
            {
                _hitPoints.SetHealth(_hitPoints.Health - damage);

                if (_hitPoints.Health < float.Epsilon)
                {
                    KillCharacter();
                    break;
                }

                if (interval > float.Epsilon) yield return delay;
                else break;
            }
        }

        public override void KillCharacter()
        {
            base.KillCharacter();
            Destroy(_healthBar.gameObject);
            Destroy(_inventory.gameObject);
        }

        private void DetectConsumableItem(Collider2D collision)
        {
            if (collision.TryGetComponent(out Consumable consumable))
            {
                Item itemData = consumable.ItemData;
                if (!itemData) return;

                bool shouldDisappear = false;

                switch (itemData.Type)
                {
                    case Item.ItemType.Coin:
                        shouldDisappear = _inventory.AddItem(itemData);
                        break;
                    case Item.ItemType.Health:
                        shouldDisappear = AdjustHealth(itemData.ItemQuantity);
                        break;
                }

                if (shouldDisappear)
                    consumable.gameObject.SetActive(false);
            }
        }
    }
}