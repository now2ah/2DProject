using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] managers;

    private void Awake()
    {
        _LoadManagers();
    }

    private void Start()
    {
        //Load Intro or Menu scene
        SceneManager.LoadScene(1);
    }

    void _LoadManagers()
    {
        foreach (var manager in managers)
        {
            Instantiate(manager);
        }
    }
}
