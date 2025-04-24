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
        }
    }

    public void StartGame()
    {
        if (null == _player)
        {
            _player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity).GetComponent<Player>();
            _player.transform.SetParent(transform);
            _isPlaying = true;
            OnStartGame?.Invoke(this, EventArgs.Empty);
            ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, new Vector3(11f, 1f));
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
