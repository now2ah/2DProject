using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public InputManagerSO inputManager;

    public List<GameObject> inventorySlotList;

    List<Item> _itemList;

    Item _clickedItem;

    private void Awake()
    {
        _itemList = new List<Item>();
    }

    private void OnEnable()
    {
        _SubscribeEvent();
        _RefreshInventoryUI();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult hit = eventData.pointerCurrentRaycast;
        Debug.Log(hit.gameObject);
    }

    void _SubscribeEvent()
    {
        //inputManager.OnPointPerformed += _OnPointPerformed;
        //inputManager.OnClickStarted += _OnClickStarted;
        //inputManager.OnClickPerformed += _OnClickPerformed;
        //inputManager.OnClickCanceled += _OnClickCanceled;

        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnLoot += Player_OnLoot;
    }

    //private void _OnPointPerformed(object sender, Vector2 e)
    //{

    //}

    //private void _OnClickStarted(object sender, EventArgs e)
    //{
    //    Debug.Log("OnClick");
    //}

    //private void _OnClickPerformed(object sender, EventArgs e)
    //{

    //}

    //private void _OnClickCanceled(object sender, EventArgs e)
    //{
        
    //}

    private void Player_OnLoot(object sender, Vector3 e)
    {
        _RefreshInventoryUI();
    }

    void _RefreshInventoryUI()
    {
        _itemList = _GetPlayersItemList();
        _ShowItemList();
    }

    List<Item> _GetPlayersItemList()
    {
        if (_itemList != null)
        {
            return GameManager.Instance.Player.InventoryItemList;
        }
        else { return null; }
    }

    void _ShowItemList()
    {
        for (int i=0; i<_itemList.Count; ++i)
        {
            if (i < inventorySlotList.Count)    //temp code before implement inventory scroll view
            {
                if (inventorySlotList[i].transform.GetChild(0).TryGetComponent<UnityEngine.UI.Image>(out UnityEngine.UI.Image image))
                {
                    if (null == _itemList[i]) { break; }

                    image.sprite = _itemList[i].ItemInfo.itemSprite;
                    image.gameObject.SetActive(true);
                }
            }
        }
    }
}
