using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Farm2D
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _quantityText;

        public Image ItemImage => _itemImage;

        public TMP_Text QuantityText => _quantityText;
    }
}