using UnityEngine;

public class MoveTutorial : MonoBehaviour
{
    public bool isDone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isDone)
            {
                //show ui
                UIManager.Instance.ShowUIAtPosition(UIManager.Instance.TutorialMove, transform.position + new Vector3(0f, 2f));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //hide ui
            isDone = true;
            UIManager.Instance.HideUI(UIManager.Instance.TutorialMove);
        }
    }
}
