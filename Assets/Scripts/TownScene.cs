using UnityEngine;

public class TownScene : MonoBehaviour
{
    public Vector3 cameraStartPosition;

    void Start()
    {
        CameraManager.Instance.SetTargetPosition(cameraStartPosition);
        UIManager.Instance.ShowTitlePanel(true);
    }
}
