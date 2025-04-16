using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public InputManagerSO inputManager;

    public GameObject itemCursorSpritePrefab;   //addressable

    [SerializeField] private GameObject _inventoryPanel;

    Canvas _uiCanvas;

    private void Awake()
    {
        _uiCanvas = transform.GetChild(0).GetComponent<Canvas>();
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

    public ItemCursorSpriteUI CreateCursorImage()
    {
        ItemCursorSpriteUI itemCursorSpriteUI = null;
        GameObject itemCursorSpriteObj = Instantiate(itemCursorSpritePrefab);
        itemCursorSpriteObj.transform.SetParent(_uiCanvas.transform);
        if (itemCursorSpriteObj.TryGetComponent<ItemCursorSpriteUI>(out ItemCursorSpriteUI itemCursorSpriteComponent))
        {
            itemCursorSpriteUI = itemCursorSpriteComponent;
        }

        return itemCursorSpriteUI;
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

