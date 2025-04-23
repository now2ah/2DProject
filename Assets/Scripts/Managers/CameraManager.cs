using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    public CinemachineCamera cinemachineCamera;
    public CinemachinePositionComposer cinemachinePositionComposer;
    public CinemachineConfiner2D cinemachineConfiner2D;
    public GameObject trackingTarget;

    //main camera
    //cinemachine camera
    bool _isTrackingPlayer = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameManager.Instance.OnStartGame += _OnStartGame;
        _Initialize();
    }

    private void Update()
    {
        if (trackingTarget != null && _isTrackingPlayer)
        {
            trackingTarget.transform.position = GameManager.Instance.Player.transform.position;
        }
    }

    private void _OnStartGame(object sender, System.EventArgs e)
    {
        _isTrackingPlayer = true;
        GetCameraBound();
    }

    public void SetTargetPosition(Vector3 position)
    {
        if (null == trackingTarget)
            return;

        trackingTarget.transform.position = position;
        
    }

    public void GetCameraBound()
    {
        if (null == cinemachineConfiner2D)
            return;

        cinemachineConfiner2D.BoundingShape2D = GameObject.FindGameObjectWithTag("CameraBound").GetComponent<Collider2D>();
    }

    void _Initialize()
    {
        if (cinemachinePositionComposer != null)
        {
            cinemachinePositionComposer.TargetOffset.y = 1f;
        }
    }
}
