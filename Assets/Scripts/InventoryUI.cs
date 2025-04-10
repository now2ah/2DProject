using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> inventorySlotList;

    List<Item> _itemList;

    private void Awake()
    {
        _itemList = new List<Item>();
    }

    private void OnEnable()
    {
        _SubscribeEvent();
        _RefreshInventoryUI();
    }

    void _SubscribeEvent()
    {
        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnLoot += Player_OnLoot;
    }

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
                if (inventorySlotList[i].transform.GetChild(0).TryGetComponent<Image>(out Image image))
                {
                    if (null == _itemList[i]) { break; }

                    image.sprite = _itemList[i].ItemInfo.itemSprite;
                    image.gameObject.SetActive(true);
                }
            }
        }
    }
}
