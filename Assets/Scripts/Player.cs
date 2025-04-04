using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public InputManagerSO inputManager;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Transform _itemBagTransform;

    private float _inputX;
    private float _inputY;

    private bool _isLookRight = false;
    private bool _isGrounded = false;
    private bool _isWalking = false;
    private bool _isAttacking = false;

    [SerializeField] private GameObject _LWeaponSlot;
    [SerializeField] private GameObject _LShieldSlot;
    [SerializeField] private GameObject _RWeaponSlot;
    [SerializeField] private GameObject _RShieldSlot;

    //equipment
    private Weapon _weapon;

    public GameObject tempSwordEffect; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _itemBagTransform = transform.GetChild(2);
    }

    private void Update()
    {
        _HandleMovement();
        _SetAnimationParams();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            if (collision.TryGetComponent<Equipment>(out Equipment item))
            {
                _Equip(item);
                item.transform.SetParent(_itemBagTransform);
                SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
                spriteRenderer.enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        inputManager.OnMovePerformed += InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled += InputManager_OnMoveCanceled;
        inputManager.OnJumpPerformed += InputManager_OnJumpPerformed;
        inputManager.OnAttackPerformed += InputManager_OnAttackPerformed;
    }

    private void OnDisable()
    {
        inputManager.OnMovePerformed -= InputManager_OnMovePerformed;
        inputManager.OnMoveCanceled -= InputManager_OnMoveCanceled;
        inputManager.OnJumpPerformed -= InputManager_OnJumpPerformed;
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

    public void Attack()
    {
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

        if (_weapon != null)
        {
            _weapon.PlayAttackEffect(_isLookRight);
        }

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

    void _Equip(Equipment item)
    {
        _weapon = item as Weapon;
        SpriteRenderer slotSprite = _RWeaponSlot.GetComponent<SpriteRenderer>();
        slotSprite.sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    void _SetAnimationParams()
    {
        if (null == _animator) { return; }

        _animator.SetBool("IsWalking", _isWalking);
    }
}
