using System;
using System.Collections.Generic;
using UnityEngine;

namespace Farm2D
{
    [RequireComponent(typeof(Animator))]
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Ammo _ammoPrefab;
        [SerializeField] private int _poolSize;
        [SerializeField] private float _projectileVelocity;

        private static List<Ammo> _ammoPool = null;

        private Camera _camera;
        private Transform _transform;
        private Animator _animator;
        private bool _isAttacking;
        private float _positiveSlope;
        private float _negativeSlope;

        private enum AttackDirection { Right, Down, Left, Up }

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;
            _isAttacking = false;

            _animator = GetComponent<Animator>();

            InitPool();
        }

        private void Start() => CalculateSlopeLines();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isAttacking = true;
                FireAmmo();
            }

            UpdateAnimationState();
        }

        private void OnDestroy()
        {
            _ammoPool.Clear();
            _ammoPool = null;
        }

        private void FireAmmo()
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

            var ammo = SpawnAmmo(_transform.position);

            if (ammo.TryGetComponent(out Arc arc))
            {
                float travelDuration = 1f / _projectileVelocity;

                StartCoroutine(arc.TravelArc(mousePos, travelDuration));
            }
        }

        private float GetSlope(Vector2 pointOne, Vector2 pointTwo) => (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);

        private void CalculateSlopeLines()
        {
            Vector2 lowerLeft = _camera.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 lowerRight = _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
            Vector2 upperLeft = _camera.ScreenToWorldPoint(new Vector2(0, Screen.height));
            Vector2 upperRight = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            _positiveSlope = GetSlope(lowerLeft, upperRight);
            _negativeSlope = GetSlope(upperLeft, lowerRight);
        }

        private bool HigherThanSlopeLine(Vector2 inputPosition, float slope)
        {
            Vector2 playerPos = _transform.position;
            Vector2 mousePos = _camera.ScreenToWorldPoint(inputPosition);

            float yIntercept = playerPos.y - (slope * playerPos.x);
            float inputIntercept = mousePos.y - (slope * mousePos.x);

            return inputIntercept > yIntercept;
        }

        private AttackDirection GetAttackDirection()
        {
            Vector2 mousePos = Input.mousePosition;

            bool higherThanPositiveSlopeLine = HigherThanSlopeLine(mousePos, _positiveSlope);
            bool higherThanNegativeSlopeLine = HigherThanSlopeLine(mousePos, _negativeSlope);

            if (higherThanPositiveSlopeLine && higherThanNegativeSlopeLine) return AttackDirection.Up;
            else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine) return AttackDirection.Down;
            else if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine) return AttackDirection.Right;
            else return AttackDirection.Left;
        }

        private void UpdateAnimationState()
        {
            if (_isAttacking)
            {
                AttackDirection attackDirection = GetAttackDirection();

                Vector2 quadrantVector = attackDirection switch
                {
                    AttackDirection.Right => Vector2.right,
                    AttackDirection.Down => Vector2.down,
                    AttackDirection.Left => Vector2.left,
                    AttackDirection.Up => Vector2.up,
                    _ => Vector2.zero,
                };

                _animator.SetBool("isAttacking", true);
                _animator.SetFloat("attackXDir", quadrantVector.x);
                _animator.SetFloat("attackYDir", quadrantVector.y);

                _isAttacking = false;
            }
            else _animator.SetBool("isAttacking", false);
        }

        private Ammo SpawnAmmo(Vector3 location)
        {
            var ammo = _ammoPool.Find(a => !a.gameObject.activeSelf);

            ammo.gameObject.SetActive(true);
            ammo.transform.position = location;

            return ammo;
        }

        private void InitPool()
        {
            if (_ammoPool == null) _ammoPool = new List<Ammo>();

            for (int i = 0; i < _poolSize; i++)
            {
                var ammo = Instantiate(_ammoPrefab);
                ammo.gameObject.SetActive(false);

                _ammoPool.Add(ammo);
            }
        }
    }
}
