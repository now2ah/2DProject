using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;


public partial class Player : MonoBehaviour
{
    public int maxHP = 100;

    private int _level = 1;
    private int _currentHP;
    private int _nextExp = 10;
    private int _currentExp = 0;

    public int Level => _level;
    public int CurrentHP => _currentHP;
    public int NextExp => _nextExp;
    public int CurrentExp => _currentExp;

    public float moveSpeed = 5.0f;
    public float jumpForce = 5.5f;
    public float damagedJumpForce = 2f;
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
    public event EventHandler OnStatChanged;
    public event EventHandler OnDie;

    //temp
    public ItemInfoSO startArmorItemInfo;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _equipments = new Equipment[MAX_EQUIPMENT_SLOT];
        _inventoryItemList = new List<Item>();
        _currentHP = maxHP;

        GameManager.Instance.OnStartGame += GameManager_OnStartGame;
    }

    private void GameManager_OnStartGame(object sender, EventArgs e)
    {
        OnStatChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        _HandleMovement();
        _SetAnimationParams();
        _CheckFall();
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
        _Jump(jumpForce);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = false;
        }
    }

    public void Initialize()
    {
        _Equip(ItemManager.Instance.CreateItem(startArmorItemInfo));
    }

    public void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    public void ApplyDamage(int damageAmount)
    {
        _animator.SetTrigger("BeHitTrigger");

        SoundManager.Instance.PlaySfx(ESFX.PLAYERBEHIT);

        _Jump(damagedJumpForce);

        _currentHP -= damageAmount;
        if (_currentHP <= 0)
        {
            //die
            SoundManager.Instance.PlaySfx(ESFX.DIED);
            OnDie?.Invoke(this, EventArgs.Empty);
        }

        OnStatChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ApplyExp(int exp)
    {
        _currentExp += exp;

        if (_currentExp >= _nextExp)
        {
            _level++;
            _currentExp -= _nextExp;
            _nextExp *= 2;
        }

        OnStatChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetHP(int hp)
    {
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        else
        {
            _currentHP = hp;
        }

        OnStatChanged?.Invoke(this, EventArgs.Empty);
    }

    void _HandleMovement()
    {
        float linearVelocity = _rigidbody.linearVelocity.y;
        linearVelocity -= 9.8f * Time.deltaTime;

        _rigidbody.linearVelocity = new Vector2(_inputX * moveSpeed, linearVelocity);

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
    }

    void _Jump(float jumpForce)
    {
        if (_isGrounded)
        {
            SoundManager.Instance.PlaySfx(ESFX.JUMP);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
        }
    }

    void _TriggerAttackAnimation()
    {
        if (_isAttacking) { return; }
        _isAttacking = true;

        if (_animator != null)
        {
            _animator.SetTrigger("AttackTrigger");
        }
    }

    void _CheckFall()
    {
        if (transform.position.y < -5f)
        {
            //game over
            SoundManager.Instance.PlaySfx(ESFX.DIED);
            OnDie?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_equipments[(int)EEquipmentType.WEAPON] != null)
        {
            ((Weapon)_equipments[(int)EEquipmentType.WEAPON]).PlayAttackEffect(_isLookRight);
        }

        SoundManager.Instance.PlaySfx(ESFX.ATTACK);

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
