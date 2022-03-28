using UnityEngine;
using UnityEngine.UI;

namespace Farm2D
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private HorizontalLayoutGroup _itemContainer;

        private const int NumSlots = 6;

        private readonly Item[] _items = new Item[NumSlots];
        private readonly Image[] _itemImages = new Image[NumSlots];
        private readonly Slot[] _slots = new Slot[NumSlots];

        private void Start() => CreateSlots();

        public bool AddItem(Item itemToAdd)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                var qtyText = _slots[i].QuantityText;

                if (_items[i] && _items[i].Type == itemToAdd.Type && itemToAdd.IsStackable)
                {
                    _items[i].SetQuantity(_items[i].ItemQuantity + 1);
                    UpdateQuantityText(i, qtyText);

                    return true;
                }

                if (!_items[i])
                {
                    _items[i] = Instantiate(itemToAdd);
                    _items[i].SetQuantity(1);

                    UpdateQuantityText(i, qtyText);

                    _itemImages[i].sprite = itemToAdd.ItemSprite;
                    _itemImages[i].enabled = true;

                    return true;
                }
            }

            return false;
        }

        private void CreateSlots()
        {
            for (int i = 0; i < NumSlots; i++)
            {
                var newSlot = Instantiate(_slotPrefab, _itemContainer.transform);
                _slots[i] = newSlot;
                _itemImages[i] = newSlot.ItemImage;
            }
        }

        private void UpdateQuantityText(int index, TMPro.TMP_Text qtyText)
        {
            qtyText.enabled = true;
            qtyText.text = _items[index].ItemQuantity.ToString();
        }
    }
}