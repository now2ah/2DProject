using UnityEngine;
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

    void _GetPlayersItemList()
    {
        if (_itemList != null)
        {
            _itemList = GameManager.Instance.Player.InventoryItemList;

            foreach(var item in _itemList)
            {
            }
        }
    }

    private void OnEnable()
    {
        _GetPlayersItemList();
    }
}
