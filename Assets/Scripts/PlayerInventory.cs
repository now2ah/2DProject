using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public static int MAX_EQUIPMENT_SLOT = 5;

    [SerializeField] Equipment[] _equipments;
    [SerializeField] List<Item> _inventoryItemList;

    public List<Item> InventoryItemList { get { return _inventoryItemList; } }
    public Equipment[] Equipments { get { return _equipments; } }

    public void LootItem(Item item)
    {
        Debug.Log(item.ItemInfo.name);

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
        if (_inventoryItemList != null)
        {
            if (_inventoryItemList.Contains(item))
            {
                _inventoryItemList.Remove(item);
            }
        }

        Equipment equipment = ItemManager.Instance.CreateEquipmentTo(gameObject, item);
        if (equipment.Item.ItemInfo is EquipmentInfoSO)
        {
            EEquipmentType type = ((EquipmentInfoSO)equipment.Item.ItemInfo).equipmentType;

            //equipment slot already has an another equipment
            if (_equipments[(int)type] != null)
            {
                //unequip _equipments[(int)type]
            }
            
            _equipments[(int)type] = equipment;
        }

        OnEquip?.Invoke(this, EventArgs.Empty);

        _UpdateEquipmentSprites();
    }

    void _UpdateEquipmentSprites()
    {
        SpriteRenderer weaponSlotSprite = _RWeaponSlot.GetComponent<SpriteRenderer>();
        weaponSlotSprite.sprite = _equipments[(int)EEquipmentType.WEAPON].Item.ItemInfo.itemSprite;
    }
}
