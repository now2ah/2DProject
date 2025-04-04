using UnityEngine;

public class GameSceneManager : Singleton<GameSceneManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
