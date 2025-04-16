using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class InventorySlot
{
    public Item containedItem;
    public GameObject uiObject;
    public Sprite itemSprite;

    public void SetSlot(Item item, GameObject go, Sprite sprite)
    {
        containedItem = item;
        uiObject = go;
        itemSprite = sprite;
    }
}

public class InventoryUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public InputManagerSO inputManager;
    public EquipmentUI equipmentUI;

    List<InventorySlot> _inventorySlotList;
    List<Item> _itemList;

    Item _selectedItem;
    InventorySlot _selectedSlot;
    ItemCursorSpriteUI _itemCursorSprite;

    public Item SelectedItem { get { return _selectedItem; } set { _selectedItem = value; } }

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

        if (_IsItemInventorySlot(hit.gameObject))
        {
            InventorySlot slot = _GetSelectedSlot(hit.gameObject);

            //from inventory
            _SwapItemSlot(_selectedItem, slot.containedItem);

            //from equip

            //refresh
            _RefreshInventoryUI();
            //InventorySlot tempSlot = new InventorySlot();
            //tempSlot.SetSlot(slot.containedItem, slot.uiObject, slot.itemSprite);
            //slot.SetSlot(_selectedSlot.containedItem, slot.uiObject, _selectedSlot.itemSprite);
            //_selectedSlot.SetSlot(tempSlot.containedItem, tempSlot.uiObject, tempSlot.itemSprite);
        }
        
        _DestroyCursorImage();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult hit = eventData.pointerCurrentRaycast;

        if (_IsItemInventorySlot(hit.gameObject))
        {
            _selectedItem = _GetSelectedItem(hit.gameObject);
            _selectedSlot = _GetSelectedSlot(hit.gameObject);
            if (equipmentUI != null)
            {
                equipmentUI.SelectedEquipment = _selectedItem;
            }

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

    InventorySlot _GetSelectedSlot(GameObject selector)
    {
        InventorySlot selectedSlot = null;

        foreach (InventorySlot slot in _inventorySlotList)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedSlot = slot;
        }

        return selectedSlot;
    }

    void _DestroyCursorImage()
    {
        if (null == _itemCursorSprite)
            return;

        Destroy(_itemCursorSprite.gameObject);
    }

    void _SwapItemSlot(Item fromItem, Item destItem)
    {
        int fromIndex = -1;
        int destIndex = -1;

        for (int i=0; i<_itemList.Count; ++i)
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
