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
            GameManager.Instance.OnStartGame += _OnStartGame;
        }
        else
        {
            GameManager.Instance.Player.transform.position = GameManager.Instance.playerStartPosition;
        }
    }

    private void _OnStartGame(object sender, System.EventArgs e)
    {
        ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, new Vector3(11f, 1f));
    }
}
