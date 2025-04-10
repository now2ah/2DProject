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

        //ItemManager.Instance.CreateGroundItem(EConsumableType.APPLE, Vector3.zero + new Vector3(1f, 0f, 0f));

        ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, Vector3.zero);
        //ItemManager.Instance.CreateGroundItem(EWeaponType.SWORD, Vector3.zero + new Vector3(1f, 0f, 0f));
    }
}
