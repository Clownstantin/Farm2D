using UnityEngine;

namespace Farm2D
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 3f;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private Vector2 _movement;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update() => UpdateAnimationStates();

        private void FixedUpdate() => Move();

        private void Move()
        {
            _movement = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
            _rigidbody2D.velocity = _movement.normalized * _moveSpeed;
        }

        private void UpdateAnimationStates()
        {
            if (Mathf.Approximately(_movement.x, 0) && Mathf.Approximately(_movement.y, 0))
                SetBoolParam(false);
            else
                SetBoolParam(true);

            _animator.SetFloat("xDir", _movement.x);
            _animator.SetFloat("yDir", _movement.y);
        }

        private void SetBoolParam(bool boolean) => _animator.SetBool("isWalking", boolean);
    }

    public enum AnimationState { Idle = 0, WalkRight = 1, WalkUp = 2, WalkLeft = 3, WalkDown = 4, }
}