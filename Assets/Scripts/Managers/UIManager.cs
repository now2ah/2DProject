using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public InputManagerSO inputManager;

    [SerializeField] private GameObject _inventoryPanel;

    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SubscribeEvent();
    }

    public void SubscribeEvent()
    {
        inputManager.OnOpenInventoryPerformed += _OnOpenInventory;
        inputManager.OnPointPerformed += _OnPointPerformed;
        inputManager.OnClickStarted += _OnClickStarted;
        inputManager.OnClickPerformed += _OnClickPerformed;
        inputManager.OnClickCanceled += _OnClickCanceled;
    }

    private void _OnPointPerformed(object sender, Vector2 e)
    {

    }

    private void _OnClickStarted(object sender, EventArgs e)
    {
        //Debug.Log("started");
    }

    private void _OnClickPerformed(object sender, EventArgs e)
    {
        //Debug.Log("performed");
    }

    private void _OnClickCanceled(object sender, EventArgs e)
    {
        //Debug.Log("canceled");
    }

    void _OnOpenInventory(object sender, EventArgs e)
    {
        if(!_inventoryPanel.activeSelf) { _inventoryPanel.SetActive(true); }
        else { _inventoryPanel.SetActive(false); }
    }
}

