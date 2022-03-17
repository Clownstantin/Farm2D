using UnityEngine;

namespace Farm2D
{
    [CreateAssetMenu(fileName = "new Item", menuName = "Item", order = 51)]
    public class Item : ScriptableObject
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _itemSprite;
        [SerializeField] private int _quantity;
        [SerializeField] private bool _isStackable;
        [SerializeField] private ItemType _itemType;

        public enum ItemType { Coin, Health }

        public string ItemName => _itemName;
        public Sprite ItemSprite => _itemSprite;
        public int ItemQuantity => _quantity;
        public bool IsStackable => _isStackable;
        public ItemType Type => _itemType;

        public void SetQuantity(int value) => _quantity = value;
    }
}