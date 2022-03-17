using UnityEngine;

namespace Farm2D
{
    public class Consumable : MonoBehaviour
    {
        [SerializeField] private Item _itemData;

        public Item ItemData => _itemData;
    }
}