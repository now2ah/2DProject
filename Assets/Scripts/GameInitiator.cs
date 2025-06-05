using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] managers;

    private void Start()
    {
        StartCoroutine(_LoadManagers(() =>
        {
            GameSceneManager.Instance.LoadNextScene();
        }));
    }

    IEnumerator _LoadManagers(Action callback)
    {
        foreach (var manager in managers)
        {
            Instantiate(manager);
            yield return null;
        }
        callback?.Invoke();
    }
}
