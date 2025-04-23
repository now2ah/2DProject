using UnityEngine;

public class TownScene : MonoBehaviour
{
    public Vector3 cameraStartPosition;

    void Start()
    {
        CameraManager.Instance.SetTargetPosition(cameraStartPosition);
        UIManager.Instance.ShowTitlePanel(true);
        GameManager.Instance.OnStartGame += _OnStartGame;
    }

    private void _OnStartGame(object sender, System.EventArgs e)
    {
        ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, new Vector3(11f, 1f));
    }
}
