using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class InventorySlot
{
    public Item containedItem;
    public GameObject uiObject;
    public Sprite itemSprite;
}

public class InventoryUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public InputManagerSO inputManager;
    public EquipmentUI equipmentUI;

    List<InventorySlot> _inventorySlotList;
    List<Item> _itemList;

    Item _selectedItem;
    ItemCursorSpriteUI _itemCursorSprite;

    private void Awake()
    {
        _inventorySlotList = new List<InventorySlot>();
        _itemList = new List<Item>();
    }

    private void OnEnable()
    {
        _SubscribeEvent();
        _RefreshInventoryUI();
    }

    void OnDisable()
    {
        _UnSetInventorySlots();
        _UnSubscribeEvent();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RaycastResult hit = eventData.pointerCurrentRaycast;

        _DestroyCursorImage();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult hit = eventData.pointerCurrentRaycast;

        if (_IsItemInventorySlot(hit.gameObject))
        {
            _selectedItem = _GetSelectedItem(hit.gameObject);

            _itemCursorSprite = UIManager.Instance.CreateCursorImage();
            _itemCursorSprite.SetSprite(_selectedItem.ItemInfo.itemSprite);
        }
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
        _RefreshInventoryUI();
    }

    void _RefreshInventoryUI()
    {
        _itemList = _GetPlayersItemList();

        _SetInventorySlots(_itemList);

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

    void _SetInventorySlots(List<Item> itemList)
    {
        if (null == itemList || itemList.Count == 0)
            return;

        for (int i = 0; i < itemList.Count; ++i)
        {
            InventorySlot slot = new InventorySlot();
            slot.containedItem = itemList[i];
            slot.uiObject = transform.GetChild(i).gameObject;
            slot.itemSprite = itemList[i].ItemInfo.itemSprite;

            _inventorySlotList.Add(slot);
        }
    }

    void _UnSetInventorySlots()
    {
        if (null == _inventorySlotList || _inventorySlotList.Count == 0)
            return;

        _inventorySlotList.Clear();
    }

    void _ShowItemList()
    {
        foreach (InventorySlot slot in _inventorySlotList)
        {
            Transform imageTransform = slot.uiObject.transform.GetChild(0);
            if (imageTransform.gameObject.TryGetComponent<Image>(out Image image))
            {
                image.sprite = slot.itemSprite;
                image.SetNativeSize();
                slot.uiObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    bool _IsItemInventorySlot(GameObject selector)
    {
        foreach(InventorySlot slot in _inventorySlotList)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                return true;
        }

        return false;
    }

    Item _GetSelectedItem(GameObject selector)
    {
        Item selectedItem = null;

        foreach (InventorySlot slot in _inventorySlotList)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedItem = slot.containedItem;
        }

        return selectedItem;
    }

    void _DestroyCursorImage()
    {
        if (null == _itemCursorSprite)
            return;

        Destroy(_itemCursorSprite.gameObject);
    }
}
