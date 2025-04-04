using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Sword : Weapon
{
    [SerializeField] private GameObject _attackEffectPrefab;

    public GameObject AttackEffectPrefab { get { return _attackEffectPrefab; } }

    private void Awake()
    {
        _Initialize();
    }

    public override void PlayAttackEffect(bool isRight)
    {
        Vector3 offset = Vector3.zero;
        Quaternion direction = Quaternion.identity;
        if (isRight)
        {
            offset = new Vector3(0.3f * 1f, 0.5f, 0f);
        }
        else
        {
            offset = new Vector3(0.3f * -1f, 0.5f, 0f);
            direction = Quaternion.Euler(0f, 180f, 0f);
        }
        StartCoroutine(PlayEffectCoroutine(offset, direction));
    }

    IEnumerator PlayEffectCoroutine(Vector3 offset, Quaternion direction)
    {
        GameObject effectObj = Instantiate(_attackEffectPrefab, transform.position + offset, direction);
        Animator animator = effectObj.GetComponent<Animator>();
        float effectTime = animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(effectTime);
        Destroy(effectObj);
    }

    protected override void _Initialize()
    {
        _itemName = "Sword";
        _itemType = EItemType.EQUIPMENT;
        _equipmentType = EEquipmentType.WEAPON;
        damage = 5f;
    }
}
