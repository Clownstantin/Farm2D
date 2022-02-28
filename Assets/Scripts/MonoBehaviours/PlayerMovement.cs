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
        private Vector2 movement;

        private enum AnimationStates { Idle = 0, WalkRight = 1, WalkUp = 2, WalkLeft = 3, WalkDown = 4, }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update() => UpdateAnimationStates();

        private void FixedUpdate() => Move();

        private void Move()
        {
            movement = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
            _rigidbody2D.velocity = movement.normalized * _moveSpeed;
        }

        private void UpdateAnimationStates()
        {
            if (movement.x > 0) SetAnimationState(AnimationStates.WalkRight);
            else if (movement.x < 0) SetAnimationState(AnimationStates.WalkLeft);
            else if (movement.y > 0) SetAnimationState(AnimationStates.WalkUp);
            else if (movement.y < 0) SetAnimationState(AnimationStates.WalkDown);
            else SetAnimationState(AnimationStates.Idle);
        }

        private void SetAnimationState(AnimationStates state)
        {
            int paramIndex = _animator.GetParameter(_animator.parameterCount - 1).nameHash;
            _animator.SetInteger(paramIndex, (int)state);
        }
    }
}