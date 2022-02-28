using UnityEngine;

namespace Farm2D
{
    public class Player : Character
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            DetectConsumableItem(collision);
        }

        public void AdjustHealth(int amount) => currentHealth += amount;

        private void DetectConsumableItem(Collider2D collision)
        {
            if (collision.TryGetComponent(out Consumable consumable))
            {
                Item itemData = consumable.Item;

                switch (itemData.Type)
                {
                    case Item.ItemType.Coin:
                        break;
                    case Item.ItemType.Health:
                        AdjustHealth(itemData.ItemQuantity);
                        break;
                }

                consumable.gameObject.SetActive(false);
            }
        }
    }
}