using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    int _sceneNum = 0;

    public int SceneNum { get { return _sceneNum; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadNextScene(bool isAdditive = false)
    {
        _sceneNum++;
        if (isAdditive)
        {
            SceneManager.LoadScene(_sceneNum, LoadSceneMode.Additive);
            
        }
        else
        {
            SceneManager.LoadScene(_sceneNum);
        }
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
