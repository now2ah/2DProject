using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EScene
{
    INITIATE,
    TOWN,
    DUNGEON,
}

public class GameSceneManager : Singleton<GameSceneManager>
{
    int _sceneNum = 0;

    public int SceneNum { get { return _sceneNum; } }

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        DontDestroyOnLoad(this);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneNum == (int)EScene.TOWN)
        {

        }
        else if (_sceneNum == (int)EScene.DUNGEON)
        {

        }
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
        _sceneNum = sceneNum;
        SceneManager.LoadScene(sceneNum);
    }
}
