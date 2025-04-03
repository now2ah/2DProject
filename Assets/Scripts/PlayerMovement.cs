using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;

    private Rigidbody2D _rigidbody;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation _playerAnimation;

    public InputManagerSO inputManager;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        inputManager.OnMovePerformed += InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled += InputManager_OnMoveCanceled;
    }

    private void OnDisable()
    {
        inputManager.OnMovePerformed -= InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled -= InputManager_OnMoveCanceled;
    }

    private void InputManager_OnMovePerformed(object sender, Vector2 e)
    {
        Debug.Log(e);
    }

    private void InputManager_OnMoveCanceled(object sender, EventArgs e)
    {
        Debug.Log(e);
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        _rigidbody.linearVelocity = new Vector2(moveInput * moveSpeed, _rigidbody.linearVelocity.y);

        if (_playerAnimation != null)
        {
            _playerAnimation.SetWalking(moveInput != 0);
        }

        if (moveInput != 0)
        {
            if (moveInput > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (moveInput < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
            //playerAnimation.PlayJumpAnimation();
        }

        bool isAirborne = !isGrounded;
        //_playerAnimation.SetJuping(isAirborne);

        if (isAirborne && _rigidbody.linearVelocity.y < -0.1f)
        {
            //_playerAnimation?.SetFalling(true);
        }

        if (!isGrounded)
        {
            //_playerAnimation?.PlayLanding();
        }
    }
}