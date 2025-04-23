using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public static int MAX_EQUIPMENT_SLOT = 4;

    [SerializeField] GameObject _headSlot;
    [SerializeField] GameObject _weaponSlot;
    [SerializeField] GameObject _armorSlot;
    [SerializeField] GameObject _shieldSlot;

    [SerializeField] GameObject _armorLSlot;
    [SerializeField] GameObject _armorRSlot;

    [SerializeField] Equipment[] _equipments;
    [SerializeField] List<Item> _inventoryItemList;

    public List<Item> InventoryItemList { get { return _inventoryItemList; } }
    public Equipment[] Equipments { get { return _equipments; } }

    public void LootItem(Item item)
    {
        if (_inventoryItemList != null)
        {
            _inventoryItemList.Add(item);

            //if player has not equipment, equip automatically
            if (item.ItemInfo.itemType == EItemType.EQUIPMENT)
            {
                if (_IsEquipmentEmpty(((EquipmentInfoSO)item.ItemInfo).equipmentType))
                {
                    _Equip(item);
                    _Remove(item);
                }
            }
        }
    }

    public void EquipItem(Item item)
    {
        _Equip(item);
    }

    public void UnEquipItem(Item item)
    {
        _UnEquip(item);
    }

    public void AddItemToInventory(Item item)
    {
        _Add(item);
    }

    public void RemoveItemFromInventory(Item item)
    {
        _Remove(item);
    }

    void _LootItemOnGround()
    {
        OnLoot?.Invoke(this, transform.position);
    }

    void _Equip(Item item)
    {
        if (null == item)
            return;

        EquipmentInfoSO equipmentInfo = item.ItemInfo as EquipmentInfoSO;

        if (!_IsEquipmentEmpty(equipmentInfo.equipmentType))
        {
            _inventoryItemList.Add(_equipments[(int)equipmentInfo.equipmentType].Item);
            Destroy(_equipments[(int)equipmentInfo.equipmentType]);
        }

        Equipment equipment = ItemManager.Instance.EquipmentTo(gameObject, item);
        if (equipment.Item.ItemInfo is EquipmentInfoSO)
        {
            EEquipmentType type = ((EquipmentInfoSO)equipment.Item.ItemInfo).equipmentType;

            _equipments[(int)type] = equipment;
        }

        OnEquip?.Invoke(this, EventArgs.Empty);

        _UpdateEquipmentSprites();
    }

    void _UnEquip(Item item)
    {
        if (null == item)
            return;

        for (int i=0; i<_equipments.Length; ++i)
        {
            if (null == _equipments[i])
                continue;

            if (item == _equipments[i].Item)
            {
                _equipments[i] = null;
                break;
            }
        }

        _UpdateEquipmentSprites();
    }

    void _Add(Item item)
    {
        if (null == item || null == _inventoryItemList)
            return;

        _inventoryItemList.Add(item);
    }

    void _Remove(Item item)
    {
        if (null == item || null == _inventoryItemList)
            return;

        if (_inventoryItemList.Contains(item))
        {
            _inventoryItemList.Remove(item);
        }
    }

    bool _IsEquipmentEmpty(EEquipmentType type)
    {
        if (_equipments[(int)type] == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void _UpdateEquipmentSprites()
    {
        if (null == _headSlot || null == _weaponSlot || null == _armorSlot || null == _shieldSlot ||
            null == _armorLSlot || null == _armorRSlot)
            return;


        if (_equipments[(int)EEquipmentType.HEAD] != null)
        {
            HeadInfoSO headInfo = _equipments[(int)EEquipmentType.HEAD].Item.ItemInfo as HeadInfoSO;
            if (headInfo != null)
            {
                SpriteRenderer headSpriteRenderer = _headSlot.GetComponent<SpriteRenderer>();
                headSpriteRenderer.sprite = headInfo.itemSprite;
            }
        }
        else
        {
            SpriteRenderer headSpriteRenderer = _headSlot.GetComponent<SpriteRenderer>();
            headSpriteRenderer.sprite = null;
        }

        if (_equipments[(int)EEquipmentType.WEAPON] != null)
        {
            WeaponInfoSO weaponInfo = _equipments[(int)EEquipmentType.WEAPON].Item.ItemInfo as WeaponInfoSO;
            if (weaponInfo != null)
            {
                SpriteRenderer weaponSlotSprite = _weaponSlot.GetComponent<SpriteRenderer>();
                weaponSlotSprite.sprite = weaponInfo.itemSprite;
            }
        }
        else
        {
            SpriteRenderer weaponSlotSprite = _weaponSlot.GetComponent<SpriteRenderer>();
            weaponSlotSprite.sprite = null;
        }

        if (_equipments[(int)EEquipmentType.ARMOR] != null)
        {
            ArmorInfoSO armorInfo = _equipments[(int)EEquipmentType.ARMOR].Item.ItemInfo as ArmorInfoSO;
            if (armorInfo != null)
            {
                SpriteRenderer armorSlotSprite = _armorSlot.GetComponent<SpriteRenderer>();
                armorSlotSprite.sprite = armorInfo.itemSprite;
                SpriteRenderer armorLSlotSprite = _armorLSlot.GetComponent<SpriteRenderer>();
                armorLSlotSprite.sprite = armorInfo.itemSprite2;
                SpriteRenderer armorRSlotSprite = _armorRSlot.GetComponent<SpriteRenderer>();
                armorRSlotSprite.sprite = armorInfo.itemSprite3;
            }
        }
        else
        {
            SpriteRenderer armorSlotSprite = _armorSlot.GetComponent<SpriteRenderer>();
            armorSlotSprite.sprite = null;
            SpriteRenderer armorLSlotSprite = _armorLSlot.GetComponent<SpriteRenderer>();
            armorLSlotSprite.sprite = null;
            SpriteRenderer armorRSlotSprite = _armorRSlot.GetComponent<SpriteRenderer>();
            armorRSlotSprite.sprite = null;
        }

        if (_equipments[(int)EEquipmentType.SHIELD] != null)
        {
            ShieldInfoSO shieldInfo = _equipments[(int)EEquipmentType.SHIELD].Item.ItemInfo as ShieldInfoSO;
            if (shieldInfo != null)
            {
                SpriteRenderer shieldSlotSprite = _shieldSlot.GetComponent<SpriteRenderer>();
                shieldSlotSprite.sprite = shieldInfo.itemSprite;
            }
        }
        else
        {
            SpriteRenderer shieldSlotSprite = _shieldSlot.GetComponent<SpriteRenderer>();
            shieldSlotSprite.sprite = null;
        }
    }
}
