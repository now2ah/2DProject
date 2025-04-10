using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Sword : Weapon
{
    public Vector3 effectOffset = new Vector3(0.5f, 0.25f, 0f);

    [SerializeField] private GameObject _attackEffectPrefab;

    public GameObject AttackEffectPrefab { get { return _attackEffectPrefab; } set { _attackEffectPrefab = value; } }

    public override void PlayAttackEffect(bool isRight)
    {
        Quaternion direction = Quaternion.identity;
        if (!isRight)
        {
            effectOffset = new Vector3(Mathf.Abs(effectOffset.x) * -1f, effectOffset.y, 0f);
            direction = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            effectOffset = new Vector3(Mathf.Abs(effectOffset.x), effectOffset.y, 0f);
            direction = Quaternion.Euler(0f, 0f, 0f);
        }
        StartCoroutine(PlayEffectCoroutine(effectOffset, direction));
    }

    IEnumerator PlayEffectCoroutine(Vector3 offset, Quaternion direction)
    {
        GameObject effectObj = Instantiate(_attackEffectPrefab, transform.position + offset, direction);
        Animator animator = effectObj.GetComponent<Animator>();
        float effectTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(effectTime);
        Destroy(effectObj);
    }
}
