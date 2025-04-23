using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if (collision.gameObject.TryGetComponent<GoblinArcher>(out GoblinArcher archer))
            {
                archer.ApplyDamage(damage);
            }
        }
    }
}
