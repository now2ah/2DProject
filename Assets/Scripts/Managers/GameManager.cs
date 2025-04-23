using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject playerPrefab;

    public Vector3 playerStartPosition;

    Player _player;

    public Player Player { get { return _player; } }

    public event EventHandler OnStartGame;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (null == _player)
        {
            _player = FindAnyObjectByType<Player>();
            //instantiate Player
        }

        //_player.Initialize();

        //ItemManager.Instance.CreateGroundItem(EConsumableType.APPLE, Vector3.zero + new Vector3(1f, 0f, 0f));

        //ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, Vector3.zero);
        //ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, Vector3.zero + new Vector3(1f, 0f, 0f));
    }

    public void StartGame()
    {
        if (null == _player)
        {
            _player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity).GetComponent<Player>();
            _player.transform.SetParent(transform);
            OnStartGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
