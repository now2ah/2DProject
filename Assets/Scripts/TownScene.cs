using UnityEngine;

public class TownScene : MonoBehaviour
{
    public Vector3 cameraStartPosition;

    void Start()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            UIManager.Instance.FadeOut();
            CameraManager.Instance.SetTargetPosition(cameraStartPosition);
            UIManager.Instance.ShowTitlePanel(true);
        }
        else
        {
            UIManager.Instance.FadeOut();
            GameManager.Instance.Player.transform.position = GameManager.Instance.playerStartPosition;
        }
    }
}
