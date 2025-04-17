using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class InventorySlot : ItemSlot
{
    public Item containedItem;

    public override void SetSlot(Item item, GameObject go, Sprite sprite)
    {
        containedItem = item;
        uiObject = go;
        itemSprite = sprite;
    }
}

public class InventoryUI : MonoBehaviour
{
    static int MAX_INVENTORY_SLOTS = 15;

    public InputManagerSO inputManager;
    public EquipmentUI equipmentUI;

    InventoryUIPointerHandler _pointHandler;

    List<Item> _itemList;
    InventorySlot[] _inventorySlots;

    private void Awake()
    {
        _pointHandler = transform.parent.GetComponent<InventoryUIPointerHandler>();

        _inventorySlots = new InventorySlot[MAX_INVENTORY_SLOTS];
        _itemList = new List<Item>();
    }

    private void OnEnable()
    {
        _SubscribeEvent();
        RefreshInventoryUI();
    }

    void OnDisable()
    {
        _UnSetInventorySlots();
        _UnSubscribeEvent();
    }

    public void RefreshInventoryUI()
    {
        _itemList = _GetPlayersItemList();

        _SetInventorySlots(_itemList);

        _ShowItemList();
    }

    public bool IsItemInventorySlot(GameObject selector)
    {
        foreach (InventorySlot slot in _inventorySlots)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                return true;
        }

        return false;
    }

    public InventorySlot SelectInventoryItem(GameObject selector)
    {
        InventorySlot slot = null;
        _pointHandler.SelectedItem = _GetSelectedItem(selector);
        slot = _GetSelectedSlot(selector);

        return slot;
    }

    public void SwapItem(GameObject selector)
    {
        if (null == _pointHandler)
            return;

        InventorySlot slot = _GetSelectedSlot(selector);

        //from inventory
        if (_pointHandler.SelectedSlot != null)
        {
            _SwapItemSlot(_pointHandler.SelectedItem, slot.containedItem);
        }

        //refresh
        RefreshInventoryUI();
    }

    public void UnEquipItem(GameObject selector)
    {
        if (null == _pointHandler)
            return;

        GameManager.Instance.Player.AddItemToInventory(_pointHandler.SelectedItem);
        GameManager.Instance.Player.UnEquipItem(_pointHandler.SelectedItem);

    }

    void _SubscribeEvent()
    {
        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnLoot += Player_OnLoot;
    }

    void _UnSubscribeEvent()
    {
        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnLoot -= Player_OnLoot;
    }

    private void Player_OnLoot(object sender, Vector3 e)
    {
        RefreshInventoryUI();
    }

    List<Item> _GetPlayersItemList()
    {
        if (_itemList != null)
        {
            return GameManager.Instance.Player.InventoryItemList;
        }
        else { return null; }
    }

    void _SetInventorySlots(List<Item> itemList)
    {
        if (null == itemList)
            return;

        for (int i = 0; i < _inventorySlots.Length; ++i)
        {
            if (i < itemList.Count)
            {
                InventorySlot slot = new InventorySlot();
                slot.containedItem = itemList[i];
                slot.uiObject = transform.GetChild(i).gameObject;
                slot.itemSprite = itemList[i].ItemInfo.itemSprite;
                _inventorySlots[i] = slot;
            }
            else
            {
                InventorySlot slot = new InventorySlot();
                slot.containedItem = null;
                slot.uiObject = transform.GetChild(i).gameObject;
                slot.itemSprite = null;
                _inventorySlots[i] = slot;
            }
        }
    }

    void _UnSetInventorySlots()
    {
        if (null == _inventorySlots || _inventorySlots.Length == 0)
            return;

        for (int i = 0; i < _inventorySlots.Length; ++i)
        {
            _inventorySlots[i] = null;
        }
    }

    void _ShowItemList()
    {
        foreach (InventorySlot slot in _inventorySlots)
        {
            if (slot.containedItem != null)
            {
                Transform imageTransform = slot.uiObject.transform.GetChild(0);
                if (imageTransform.gameObject.TryGetComponent<Image>(out Image image))
                {
                    image.sprite = slot.itemSprite;
                    image.SetNativeSize();
                    slot.uiObject.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                Transform imageTransform = slot.uiObject.transform.GetChild(0);
                if (imageTransform.gameObject.TryGetComponent<Image>(out Image image))
                {
                    image.sprite = null;
                    slot.uiObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    Item _GetSelectedItem(GameObject selector)
    {
        Item selectedItem = null;

        foreach (InventorySlot slot in _inventorySlots)
        {
            if (null == slot)
                continue;

            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedItem = slot.containedItem;
        }

        return selectedItem;
    }

    InventorySlot _GetSelectedSlot(GameObject selector)
    {
        InventorySlot selectedSlot = null;

        foreach (InventorySlot slot in _inventorySlots)
        {
            if (null == slot)
                continue;

            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedSlot = slot;
        }

        return selectedSlot;
    }

    void _SwapItemSlot(Item fromItem, Item destItem)
    {
        int fromIndex = -1;
        int destIndex = -1;

        for (int i = 0; i < _itemList.Count; ++i)
        {
            if (_itemList[i] == fromItem)
            {
                fromIndex = i;
            }

            if (_itemList[i] == destItem)
            {
                destIndex = i;
            }
        }

        if (fromIndex == -1 || destIndex == -1)
            return;

        Item temp = new Item();
        temp = _itemList[destIndex];
        _itemList[destIndex] = _itemList[fromIndex];
        _itemList[fromIndex] = temp;
    }
}
