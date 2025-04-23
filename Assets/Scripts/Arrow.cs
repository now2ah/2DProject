using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 2f;
    public int attackDamage = 5;

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
