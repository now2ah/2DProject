using System.Collections;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    int hitTimes = 3;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (GameManager.Instance.IsDoneTutorial)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "PlayerProjectile")
        {
            _HitEffect();
            hitTimes--;

            if (hitTimes == 0)
                gameObject.SetActive(false);
        }
    }

    void _HitEffect()
    {
        StartCoroutine(HitEffectCoroutine());
    }

    IEnumerator HitEffectCoroutine()
    {
        float originColorG = _spriteRenderer.color.g;
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, 0f, _spriteRenderer.color.b);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, originColorG, _spriteRenderer.color.b);
    }
}
