using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject playerPrefab;

    public Vector3 playerStartPosition;

    Player _player;

    bool _isPlaying = false;
    bool _isDoneTutorial = false;

    public Player Player { get { return _player; } }
    public bool IsPlaying => _isPlaying;
    public bool IsDoneTutorial { get { return _isDoneTutorial; } set { _isDoneTutorial = value; } }
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
            _isPlaying = true;
            OnStartGame?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RespawnPlayer()
    {
        _player.SetHP(_player.maxHP);
        StartCoroutine(BackToTownCoroutine());
    }

    IEnumerator BackToTownCoroutine()
    {
        yield return null;
        GameSceneManager.Instance.LoadScene(1);
    }
}
