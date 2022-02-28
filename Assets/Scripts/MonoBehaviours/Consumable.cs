using UnityEngine;

namespace Farm2D
{
    public class Consumable : MonoBehaviour
    {
        [SerializeField] private Item _item;

        public Item Item => _item;
    }
}