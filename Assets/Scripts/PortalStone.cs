using UnityEngine;

public class PortalStone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameSceneManager.Instance.SceneNum == 1)
            {
                GameSceneManager.Instance.LoadNextScene();
            }
            else if (GameSceneManager.Instance.SceneNum == 2)
            {
                GameSceneManager.Instance.LoadScene(1);
            }
        }
    }
}
