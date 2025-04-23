using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public int attackDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {

        }
    }
}
