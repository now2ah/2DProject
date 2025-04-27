using UnityEngine;

public class PortalStone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.isTrigger)
        {
            SoundManager.Instance.PlaySfx(ESFX.PORTAL);
            if (GameSceneManager.Instance.SceneNum == 1)
            {
                UIManager.Instance.FadeIn(() =>
                {
                    GameSceneManager.Instance.LoadNextScene();
                });
            }
            else if (GameSceneManager.Instance.SceneNum == 2)
            {
                UIManager.Instance.FadeIn(() =>
                {
                    GameSceneManager.Instance.LoadScene(1);
                });
            }
        }
    }
}
