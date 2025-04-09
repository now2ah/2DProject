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
    }

    void _OnOpenInventory(object sender, EventArgs e)
    {
        if(!_inventoryPanel.activeSelf) { _inventoryPanel.SetActive(true); }
        else { _inventoryPanel.SetActive(false); }
    }
}

