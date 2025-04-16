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
        }

        //_Equip(item);
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

        if (_equipments[(int)EEquipmentType.WEAPON] != null)
        {
            WeaponInfoSO weaponInfo = _equipments[(int)EEquipmentType.WEAPON].Item.ItemInfo as WeaponInfoSO;
            if (weaponInfo != null)
            {
                SpriteRenderer weaponSlotSprite = _weaponSlot.GetComponent<SpriteRenderer>();
                weaponSlotSprite.sprite = weaponInfo.itemSprite;
            }
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

        if (_equipments[(int)EEquipmentType.SHIELD] != null)
        {
            ShieldInfoSO shieldInfo = _equipments[(int)EEquipmentType.SHIELD].Item.ItemInfo as ShieldInfoSO;
            if (shieldInfo != null)
            {
                SpriteRenderer shieldSlotSprite = _shieldSlot.GetComponent<SpriteRenderer>();
                shieldSlotSprite.sprite = shieldInfo.itemSprite;
            }
        }
    }
}
