using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public InputManagerSO inputManager;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private float _inputX;
    private float _inputY;

    private bool _isGrounded = false;
    private bool _isWalking = false;
    private bool _isAttacking = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _HandleMovement();
        _SetAnimationParams();
    }

    private void OnEnable()
    {
        inputManager.OnMovePerformed += InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled += InputManager_OnMoveCanceled;
        inputManager.OnAttackPerformed += InputManager_OnAttackPerformed;
    }

    private void OnDisable()
    {
        inputManager.OnMovePerformed -= InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled -= InputManager_OnMoveCanceled;
        inputManager.OnAttackPerformed -= InputManager_OnAttackPerformed;
    }

    private void InputManager_OnMovePerformed(object sender, Vector2 input)
    {
        _inputX = input.x;
        _inputY = input.y;

        if (_inputX != 0)
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    private void InputManager_OnMoveCanceled(object sender, EventArgs e)
    {

    }

    private void InputManager_OnAttackPerformed(object sender, EventArgs e)
    {
        if (_isAttacking) { return; }
        
        SoundManager.Instance.PlaySfx(ESFX.ATTACK);
    }

    void _HandleMovement()
    {
        _rigidbody.linearVelocity = new Vector2(_inputX * moveSpeed, _rigidbody.linearVelocity.y);

        if (_isWalking)
        {
            if (_inputX > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (_inputX < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
            //playerAnimation.PlayJumpAnimation();
        }

        bool isAirborne = !_isGrounded;
        //_playerAnimation.SetJuping(isAirborne);

        if (isAirborne && _rigidbody.linearVelocity.y < -0.1f)
        {
            //_playerAnimation?.SetFalling(true);
        }

        if (!_isGrounded)
        {
            //_playerAnimation?.PlayLanding();
        }
    }

    void _SetAnimationParams()
    {
        if (null == _animator) { return; }

        _animator.SetBool("IsWalking", _isWalking);
    }
}
