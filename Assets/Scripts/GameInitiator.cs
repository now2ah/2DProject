using UnityEngine;

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
    }

    void _LoadManagers()
    {
        foreach (var manager in managers)
        {
            Instantiate(manager);
        }
    }
}
