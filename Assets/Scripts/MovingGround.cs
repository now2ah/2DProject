using UnityEngine;
using UnityEngine.SceneManagement;

public enum EMovingGroundType
{
    HORIZONTAL,
    VERTICAL
}

public class MovingGround : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    public EMovingGroundType groundType = EMovingGroundType.HORIZONTAL;
    private Vector3 startPos;
    private int direction = 1;


    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (groundType == EMovingGroundType.HORIZONTAL)
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
            }

            transform.position += new Vector3(speed * direction * Time.deltaTime, 0f, 0f);
        }
        else if (groundType == EMovingGroundType.VERTICAL)
        {
            if (transform.position.y > startPos.y + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.y < startPos.y - maxDistance)
            {
                direction = 1;
            }

            transform.position += new Vector3(0f, speed * direction * Time.deltaTime, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(gameObject.transform);
        }
            
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && collision.transform.parent.gameObject.activeInHierarchy)
        {
            collision.transform.SetParent(null);
        }
    }
}
