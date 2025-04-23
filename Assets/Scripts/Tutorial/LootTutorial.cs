using UnityEngine;

public class LootTutorial : MonoBehaviour
{
    public bool isDone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isDone)
            {
                //show ui
                UIManager.Instance.ShowUIAtPosition(UIManager.Instance.TutorialLoot, transform.position + new Vector3(0f, 2f));
                GameManager.Instance.Player.OnLoot += Player_OnLoot;
            }
        }
    }

    private void Player_OnLoot(object sender, Vector3 e)
    {
        isDone = true;
        UIManager.Instance.HideUI(UIManager.Instance.TutorialLoot);
        GameManager.Instance.Player.OnLoot -= Player_OnLoot;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //hide ui
            isDone = true;
            UIManager.Instance.HideUI(UIManager.Instance.TutorialLoot);
        }
    }
}
