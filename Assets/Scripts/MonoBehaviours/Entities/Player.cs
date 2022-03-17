using UnityEngine;

namespace Farm2D
{
    public class Player : Character
    {
        [Header("UI Elements")]
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Inventory _inventory;

        private Canvas _gameUI;

        private void Start()
        {
            hitPoints.SetHealth(startingHP);

            _healthBar = Instantiate(_healthBar, _gameUI.transform);
            _healthBar.SetPlayer(this);

            _inventory = Instantiate(_inventory, _gameUI.transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DetectConsumableItem(collision);
        }

        public bool AdjustHealth(int amount)
        {
            if (hitPoints.Health < maxHealth)
            {
                hitPoints.SetHealth(hitPoints.Health + amount);
                return true;
            }
            return false;
        }

        public void Init(Canvas canvas) => _gameUI = canvas;

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