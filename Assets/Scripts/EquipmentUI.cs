using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot
{
    public Equipment containedEquipment;
    public GameObject uiObject;
    public Sprite equipmentSprite;
}

public class EquipmentUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public InventoryUI inventoryUI;

    public GameObject[] equipmentSlots;

    List<EquipmentSlot> _equipmentSlotList;
    Equipment[] _equipments;

    Equipment _selectedEquipment;
    ItemCursorSpriteUI _itemCursorSprite;

    private void Awake()
    {
        _equipmentSlotList = new List<EquipmentSlot>();
        _equipments = new Equipment[Player.MAX_EQUIPMENT_SLOT];
    }

    private void OnEnable()
    {
        _SubscribeEvent();
        _RefreshEquipmentUI();
    }

    void OnDisable()
    {
        _UnSetEquipmentSlots();
        _UnSubscribeEvent();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //check pointer is on equipment slot
        //validate equipmentslot
        //if is validate
        //equip

        RaycastResult hit = eventData.pointerCurrentRaycast;

        if (_IsEquipmentSlot(hit.gameObject))
        {

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult hit = eventData.pointerCurrentRaycast;

        if (_IsEquipmentSlot(hit.gameObject))
        {
            _selectedEquipment = _GetSelectedEquipment(hit.gameObject);

            _itemCursorSprite = UIManager.Instance.CreateCursorImage();
            _itemCursorSprite.SetSprite(_selectedEquipment.Item.ItemInfo.itemSprite);
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
        _RefreshEquipmentUI();
        _ShowEquipment();
    }

    void _RefreshEquipmentUI()
    {
        _equipments = _GetPlayerEquipments();

        _SetEquipmentSlots(_equipments);

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
            if (null == playerEquipments[i])
                continue;

            EquipmentSlot slot = new EquipmentSlot();
            slot.containedEquipment = playerEquipments[i];
            slot.uiObject = equipmentSlots[i];
            slot.equipmentSprite = playerEquipments[i].Item.ItemInfo.itemSprite;

            _equipmentSlotList.Add(slot);
        }
    }

    void _UnSetEquipmentSlots()
    {
        if (null == _equipmentSlotList || _equipmentSlotList.Count == 0)
            return;

        _equipmentSlotList.Clear();
    }

    void _ShowEquipment()
    {
        for (int i = 0; i < _equipments.Length; ++i)
        {
            if (equipmentSlots[i].transform.GetChild(0).TryGetComponent<Image>(out Image image))
            {
                if (null == _equipments[i])
                    continue;

                image.sprite = _equipments[i].Item.ItemInfo.itemSprite;
                image.SetNativeSize();
                image.gameObject.SetActive(true);
            }
        }
    }

    bool _IsEquipmentSlot(GameObject selector)
    {
        foreach(EquipmentSlot slot in _equipmentSlotList)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                return true;
        }

        return false;
    }

    Equipment _GetSelectedEquipment(GameObject selector)
    {
        Equipment selectedEquipment = null;

        foreach (EquipmentSlot slot in _equipmentSlotList)
        {
            if (slot.uiObject == selector.transform.parent.gameObject)
                selectedEquipment = slot.containedEquipment;
        }

        return selectedEquipment;
    }
}
