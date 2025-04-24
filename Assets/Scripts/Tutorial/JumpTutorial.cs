using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    public bool isDone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isDone && !GameManager.Instance.IsDoneTutorial)
            {
                //show ui
                UIManager.Instance.ShowUIAtPosition(UIManager.Instance.TutorialJump, transform.position + new Vector3(0f, 2f));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //hide ui
            isDone = true;
            UIManager.Instance.HideUI(UIManager.Instance.TutorialJump);
            GameManager.Instance.IsDoneTutorial = true;
        }
    }
}
