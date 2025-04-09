using System.Collections;
using UnityEngine;

public abstract class WeaponEffect : MonoBehaviour
{
    public abstract void PlayAttackEffect(bool isRight);
}

public class SwordEffect : WeaponEffect
{
    public Vector3 effectOffset = new Vector3(0.1f, 0.1f, 0f);

    [SerializeField] private GameObject _attackEffectPrefab;

    public GameObject AttackEffectPrefab { get { return _attackEffectPrefab; } }

    private void Awake()
    {

    }

    public override void PlayAttackEffect(bool isRight)
    {
        Quaternion direction = Quaternion.identity;
        if (!isRight)
        {
            effectOffset = new Vector3(effectOffset.x * -1f, effectOffset.y, 0f);
            direction = Quaternion.Euler(0f, 180f, 0f);
        }
        StartCoroutine(PlayEffectCoroutine(effectOffset, direction));
    }

    IEnumerator PlayEffectCoroutine(Vector3 offset, Quaternion direction)
    {
        GameObject effectObj = Instantiate(_attackEffectPrefab, transform.position + offset, direction);
        Animator animator = effectObj.GetComponent<Animator>();
        float effectTime = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(effectTime);
        yield return new WaitForSeconds(effectTime);
        Destroy(effectObj);
        Debug.Log("Destroyed");
    }
}