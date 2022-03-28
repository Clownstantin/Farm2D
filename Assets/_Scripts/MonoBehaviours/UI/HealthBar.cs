using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Farm2D
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private HitPoints _hitPoints;
        [SerializeField] private Image _hpMeter;
        [SerializeField] private TMP_Text _hpText;

        private Player _player;
        private float _maxHP;

        private readonly string _startTxt = "HP:";

        private const int ConvertToPercent = 100;

        private void OnEnable()
        {
            _hitPoints.OnHpChanged += UpdateHealth;
        }

        private void Start()
        {
            _maxHP = _player.MaxHealth;
            UpdateHealth(_hitPoints.Health);
        }

        private void OnDisable()
        {
            _hitPoints.OnHpChanged -= UpdateHealth;
        }

        public void Init(Player player) => _player = player;

        private void UpdateHealth(float health)
        {
            if (!_player)
            {
                _hitPoints.OnHpChanged -= UpdateHealth;
                return;
            }

            _hpMeter.fillAmount = health / _maxHP;
            _hpText.text = _startTxt + (_hpMeter.fillAmount * ConvertToPercent);
        }
    }
}