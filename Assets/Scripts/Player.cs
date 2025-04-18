using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;


public partial class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public InputManagerSO inputManager;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private float _inputX;
    private float _inputY;

    private bool _isLookRight = false;
    private bool _isGrounded = false;
    private bool _isWalking = false;
    private bool _isAttacking = false;

    public event EventHandler<Vector3> OnLoot;
    public event EventHandler OnEquip;

    //temp
    public ItemInfoSO startArmorItemInfo;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _equipments = new Equipment[MAX_EQUIPMENT_SLOT];
        _inventoryItemList = new List<Item>();
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
        inputManager.OnJumpPerformed += InputManager_OnJumpPerformed;
        inputManager.OnAttackPerformed += InputManager_OnAttackPerformed;
        inputManager.OnInteractPerformed += InputManager_OnInteractPerformed;
        inputManager.OnLootPerformed += InputManager_OnLootPerformed;
    }

    private void OnDisable()
    {
        inputManager.OnMovePerformed -= InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled -= InputManager_OnMoveCanceled;
        inputManager.OnJumpPerformed -= InputManager_OnJumpPerformed;
        inputManager.OnAttackPerformed -= InputManager_OnAttackPerformed;
        inputManager.OnInteractPerformed -= InputManager_OnInteractPerformed;
    }

    private void InputManager_OnMovePerformed(object sender, Vector2 input)
    {
        _inputX = input.x;
        _inputY = input.y;

        if (_inputX != 0)
        {
            _isWalking = true;
        }
    }

    private void InputManager_OnMoveCanceled(object sender, EventArgs e)
    {
        _inputX = 0f;
        _isWalking = false;
    }

    private void InputManager_OnJumpPerformed(object sender, EventArgs e)
    {
        _Jump();
    }


    private void InputManager_OnAttackPerformed(object sender, EventArgs e)
    {
        _TriggerAttackAnimation();
    }

    private void InputManager_OnInteractPerformed(object sender, EventArgs e)
    {

    }

    private void InputManager_OnLootPerformed(object sender, EventArgs e)
    {
        _LootItemOnGround();
    }

    public void Initialize()
    {
        _Equip(ItemManager.Instance.CreateItem(startArmorItemInfo));
    }

    public void Attack()
    {
        if (_isAttacking) { return; }

        StartCoroutine(AttackCoroutine());
    }

    void _HandleMovement()
    {
        _rigidbody.linearVelocity = new Vector2(_inputX * moveSpeed, _rigidbody.linearVelocity.y);

        if (_isWalking)
        {
            if (_inputX > 0)
            {
                _isLookRight = true;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (_inputX < 0)
            {
                _isLookRight = false;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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

    void _Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
        }
    }

    void _TriggerAttackAnimation()
    {
        if (_isAttacking) { return; }

        if (_animator != null)
        {
            _animator.SetTrigger("AttackTrigger");
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;

        yield return null;
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        //if (_WeaponSlot != null)
        //{
        //    _WeaponSlot.PlayAttackEffect(_isLookRight);
        //}

        if (stateInfo.IsName("0_Attack_Normal"))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        _isAttacking = false;
    }

    void _SetAnimationParams()
    {
        if (null == _animator) { return; }

        _animator.SetBool("IsWalking", _isWalking);
    }
}
