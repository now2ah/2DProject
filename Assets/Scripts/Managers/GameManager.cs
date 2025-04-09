using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player _player;

    public Player Player { get { return _player; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();

        if (null == _player)
        {
            //instantiate Player
        }

        ItemManager.Instance.CreateGroundItem(EWeapon.SWORD, Vector3.zero);
    }
}
