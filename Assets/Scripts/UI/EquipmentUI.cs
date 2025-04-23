using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : ItemSlot
{
    public Equipment containedEquipment;

    public override void SetSlot(Item item, GameObject go, Sprite sprite)
    {
        containedEquipment.Item = item;
        containedEquipment.Type = ((EquipmentInfoSO)item.ItemInfo).equipmentType;
        uiObject = go;
        itemSprite = sprite;
    }
}

public class EquipmentUI : MonoBehaviour
{
    public InventoryUI inventoryUI;

    InventoryUIPointerHandler _pointHandler;

    Equipment[] _equipments;
    EquipmentSlot[] _equipmentSlots;

    private void Awake()
    {
        _pointHandler = transform.parent.GetComponent<InventoryUIPointerHandler>();

        _equipments = new Equipment[Player.MAX_EQUIPMENT_SLOT];
        _equipmentSlots = new EquipmentSlot[Player.MAX_EQUIPMENT_SLOT];
    }

    private void OnEnable()
    {
        _SubscribeEvent();
        RefreshEquipmentUI();
    }

    void OnDisable()
    {
        //_UnSetEquipmentSlots();
        _UnSubscribeEvent();
    }

    public bool IsEquipmentSlot(GameObject selector)
    {
        foreach (EquipmentSlot slot in _equipmentSlots)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                return true;
        }

        return false;
    }

    public void RefreshEquipmentUI()
    {
        _equipments = _GetPlayerEquipments();

        _SetEquipmentSlots(_equipments);

        _ShowEquipment();
    }

    public EquipmentSlot SelectEquipmentItem(GameObject selector)
    {
        EquipmentSlot slot = _GetSelectedEquipmentSlot(selector);
        if (null == slot.containedEquipment)
            return null;

        _pointHandler.SelectedItem = _GetSelectedEquipment(selector).Item;

        return slot;
    }

    public void EquipItem(GameObject selector)
    {
        if (_pointHandler.SelectedItem.ItemInfo.itemType == EItemType.EQUIPMENT)
        {
            EquipmentSlot slot = _GetSelectedEquipmentSlot(selector);

            if (_IsValidEquipmentType(slot, _pointHandler.SelectedItem))
            {
                GameManager.Instance.Player.EquipItem(_pointHandler.SelectedItem);
                GameManager.Instance.Player.RemoveItemFromInventory(_pointHandler.SelectedItem);
            }
        }
    }

    void _SubscribeEvent()
    {
        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnEquip += Player_OnEquip;
    }

    void _UnSubscribeEvent()
    {
        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnEquip -= Player_OnEquip;
    }

    private void Player_OnEquip(object sender, System.EventArgs e)
    {
        RefreshEquipmentUI();
        _ShowEquipment();
    }

    Equipment[] _GetPlayerEquipments()
    {
        if (_equipments != null)
        {
            return GameManager.Instance.Player.Equipments;
        }
        else { return null; }
    }

    void _SetEquipmentSlots(Equipment[] playerEquipments)
    {
        if (null == playerEquipments || playerEquipments.Length == 0)
            return;

        for (int i=0; i<playerEquipments.Length; ++i)
        {
            EquipmentSlot slot = new EquipmentSlot();

            if (null == playerEquipments[i])
            {
                slot.containedEquipment = null;
                slot.uiObject = transform.GetChild(i).gameObject;
                slot.itemSprite = null;
            }
            else
            {
                slot.containedEquipment = playerEquipments[i];
                slot.uiObject = transform.GetChild(i).gameObject;
                slot.itemSprite = playerEquipments[i].Item.ItemInfo.itemSprite;
            }

            _equipmentSlots[i] = slot;
        }
    }

    void _ShowEquipment()
    {
        for (int i = 0; i < _equipments.Length; ++i)
        {
            if (_equipmentSlots[i].uiObject.transform.GetChild(0).TryGetComponent<Image>(out Image image))
            {
                if (null == _equipments[i])
                {
                    image.sprite = null;
                    image.gameObject.SetActive(false);
                }
                else
                {
                    image.sprite = _equipments[i].Item.ItemInfo.itemSprite;
                    image.SetNativeSize();
                    image.gameObject.SetActive(true);
                }
            }
        }
    }

    Equipment _GetSelectedEquipment(GameObject selector)
    {
        Equipment selectedEquipment = null;

        foreach (EquipmentSlot slot in _equipmentSlots)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedEquipment = slot.containedEquipment;
        }

        return selectedEquipment;
    }

    EquipmentSlot _GetSelectedEquipmentSlot(GameObject selector)
    {
        EquipmentSlot selectedSlot = null;
        foreach (EquipmentSlot slot in _equipmentSlots)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedSlot = slot;
        }
        return selectedSlot;
    }

    bool _IsValidEquipmentType(EquipmentSlot slot, Item item)
    {
        for (int i=0; i<_equipmentSlots.Length; ++i)
        {
            if (slot == _equipmentSlots[i] &&
                i == (int)((EquipmentInfoSO)item.ItemInfo).equipmentType)
            {
                return true;
            }
        }

        return false;
    }
}
