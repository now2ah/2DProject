using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GoblinArcher owner;
    public float speed = 2f;
    public int attackDamage;

    private void Start()
    {
        attackDamage = owner.attackDamage;
    }

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.ApplyDamage(attackDamage);
            }
        }

        Destroy(this.gameObject);
    }
}
