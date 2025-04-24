using System.Collections;
using UnityEngine;

public class GoblinArcherIdleState : State
{
    GoblinArcher _goblinArcher;

    public GoblinArcherIdleState(GoblinArcher goblinArcher)
    {
        _goblinArcher = goblinArcher;
    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void EndState()
    {

    }
}

public class GoblinArcherAttackState : State
{
    GoblinArcher _goblinArcher;

    public GoblinArcherAttackState(GoblinArcher goblinArcher)
    {
        _goblinArcher = goblinArcher;
    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        if (_goblinArcher != null)
        {
            _goblinArcher.LookAtTarget();
            _goblinArcher.Attack();
        }
    }

    public override void EndState()
    {

    }
}

public class GoblinArcher : MonoBehaviour
{
    public float shootOffsetX = 0.25f;
    public float shootOffsetY = 0.25f;

    public int maxHP = 10;
    public int rewardExp = 10;
    public int attackDamage = 5;

    Animator _animator;
    bool _isLookRight = false;
    bool _isAttack = false;
    bool _isDead = false;
    int _currentHP;

    [SerializeField] float _attackTime = 5f;
    float _elapsedTime = 0f;

    [SerializeField] GameObject arrowPrefab;
    GameObject _target = null;
    

    StateMachine _stateMachine;
    GoblinArcherIdleState _idleState;
    GoblinArcherAttackState _attackState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = gameObject.AddComponent<StateMachine>();
        _idleState = new GoblinArcherIdleState(this);
        _attackState = new GoblinArcherAttackState(this);
        _currentHP = maxHP;
    }

    private void Start()
    {
        _stateMachine.StartState(_idleState);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _isAttack = true;
            _target = collision.gameObject;
            _stateMachine.ChangeState(_attackState);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _stateMachine.ChangeState(_idleState);
            _target = null;
            _isAttack = false;
        }
    }

    public void LookAtTarget()
    {
        if (_target != null)
        {
            if (_target.transform.position.x > transform.position.x)
            {
                _isLookRight = true;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                _isLookRight = false;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    public void Attack()
    {
        if (_elapsedTime > _attackTime)
        {
            _ShootArrow(_target);
            _elapsedTime = 0f;
        }
    }

    public void ApplyDamage(int damageAmount, Player from)
    {
        if (_isDead)
            return;

        _animator.SetTrigger("BeHitTrigger");

        _currentHP -= damageAmount;
        if (_currentHP <= 0)
        {
            //die
            _isDead = true;
            StartCoroutine(DieCoroutine(from));
            from.ApplyExp(rewardExp);
        }
    }

    void _ShootArrow(GameObject target)
    {
        if (_animator != null && target != null)
        {
            _animator.SetTrigger("AttackTrigger");
            
            if (arrowPrefab != null)
            {
                float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion direction = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));

                float flag = 0f;
                if (_isLookRight)
                {
                    flag = -1f;
                }
                else
                {
                    flag = 1f;
                }
                
                GameObject arrowObj = Instantiate(arrowPrefab, transform.position + new Vector3(shootOffsetX * flag, shootOffsetY), direction);
                if (arrowObj.TryGetComponent<Arrow>(out Arrow arrow))
                {
                    arrow.owner = this;
                }
            }
            
        }
    }

    IEnumerator DieCoroutine(Player from)
    {
        if (_animator != null)
        {
            float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
            Destroy(this.gameObject);
        }
    }
}
