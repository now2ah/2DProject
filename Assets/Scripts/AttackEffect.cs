using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public Player owner;
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            SoundManager.Instance.PlaySfx(ESFX.HIT);
            if (collision.gameObject.TryGetComponent<GoblinArcher>(out GoblinArcher archer))
            {
                if (owner != null)
                    archer.ApplyDamage(damage, owner);
            }
        }
    }
}
