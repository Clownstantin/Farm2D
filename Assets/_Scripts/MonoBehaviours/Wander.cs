using System.Collections;
using UnityEngine;

namespace Farm2D
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D), typeof(BoxCollider2D))]
    public class Wander : MonoBehaviour
    {
        [SerializeField] private float _chaseSpeed = 1f;
        [SerializeField] private float _wanderSpeed = 1f;
        [SerializeField] private float _directionChangeInterval = 2f;
        [SerializeField] private bool _followPlayer = false;

        private float _currentSpeed = 0;
        private float _currentAngle = 0;

        private Rigidbody2D _rb2D;
        private BoxCollider2D _boxCollider;
        private CircleCollider2D _circleCollider;
        private Animator _animator;
        private Transform _targetTransform = null;
        private Vector3 _endPosition;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb2D = GetComponent<Rigidbody2D>();

            _boxCollider = GetComponent<BoxCollider2D>();
            _circleCollider = GetComponent<CircleCollider2D>();

            _currentSpeed = _wanderSpeed;

            StartCoroutine(WanderRoutine());
        }

        private void Update() => Debug.DrawLine(transform.position, _endPosition, Color.red);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player) && _followPlayer)
            {
                _currentSpeed = _chaseSpeed;
                _targetTransform = player.transform;

                UpdateMove();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out Player _)) return;

            SetAnimationState(AnimationState.Idle);

            _currentSpeed = _wanderSpeed;

            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);

            _targetTransform = null;
        }

        private void OnDrawGizmos()
        {
            if (_circleCollider != null)
                Gizmos.DrawWireSphere(transform.position, _circleCollider.radius);
        }

        private IEnumerator WanderRoutine()
        {
            var delay = new WaitForSeconds(_directionChangeInterval);

            while (true)
            {
                ChooseNewEndpoint();
                UpdateMove();

                yield return delay;
            }
        }

        private void UpdateMove()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(Move(_rb2D, _currentSpeed));
        }

        private IEnumerator Move(Rigidbody2D rb2D, float speed)
        {
            var fixedDelay = new WaitForFixedUpdate();

            float remainingDistance = (transform.position - _endPosition).sqrMagnitude;

            while (remainingDistance > float.Epsilon)
            {
                if (_targetTransform) _endPosition = _targetTransform.position;

                if (rb2D)
                {
                    UpdateAnimationState();

                    Vector3 newPosition = Vector3.MoveTowards(rb2D.position, _endPosition, speed * Time.deltaTime);
                    _rb2D.MovePosition(newPosition);

                    remainingDistance = (transform.position - _endPosition).sqrMagnitude;
                }

                yield return fixedDelay;
            }

            UpdateAnimationState();
        }

        private void ChooseNewEndpoint()
        {
            _currentAngle = Random.Range(0, 360);
            _currentAngle = Mathf.Repeat(_currentAngle, 360);
            _endPosition += Vector3FromAngle(_currentAngle);
        }

        private Vector3 Vector3FromAngle(float angleDegrees)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
        }

        private void UpdateAnimationState()
        {
            Vector2 direction = _endPosition - transform.position;

            if (direction.x > 0) SetAnimationState(AnimationState.WalkRight);
            else if (direction.x < 0) SetAnimationState(AnimationState.WalkLeft);
            else if (direction.y > 0) SetAnimationState(AnimationState.WalkUp);
            else if (direction.y < 0) SetAnimationState(AnimationState.WalkDown);
            else SetAnimationState(AnimationState.Idle);
        }

        private void SetAnimationState(AnimationState state)
        {
            int paramIndex = _animator.GetParameter(0).nameHash;
            _animator.SetInteger(paramIndex, (int)state);
        }

        #region AnimationEvent
        private void SwitchBoxColliderSize(int index) => _boxCollider.size = index == 1 ? new Vector2(1, 0.5f) : new Vector2(0.5f, 1);
        #endregion
    }
}
