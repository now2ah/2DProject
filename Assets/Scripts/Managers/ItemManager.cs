using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public ItemInfoSO[] itemInfos;
    public GameObject groundItemPrefab;

    //temp -> addressable
    public GameObject swordAttackPrefab;

    Dictionary<string, ItemInfoSO> _itemInfoDic;

    public Dictionary<string, ItemInfoSO> ItemInfoDic { get { return _itemInfoDic; } }

    private void Awake()
    {
        _itemInfoDic = new Dictionary<string, ItemInfoSO>();
    }

    private void Start()
    {
        _SetUpItemInfoDictionary();
    }

    public Equipment CreateEquipmentTo(GameObject playerObj, Item item)
    {
        Equipment equipment = null;

        if (item != null && item.ItemInfo.itemType is EItemType.EQUIPMENT)
        {
            if (item.ItemInfo is EquipmentInfoSO)
            {
                if (item.ItemInfo is WeaponInfoSO)
                {
                    switch (((WeaponInfoSO)item.ItemInfo).weaponType)
                    {
                        case EWeaponType.SWORD:
                            equipment = playerObj.AddComponent<Sword>();
                            equipment.Item = item;
                            ((Sword)equipment).AttackEffectPrefab = swordAttackPrefab;
                            break;
                    }
                }
                else if (item.ItemInfo is ShieldInfoSO)
                {

                }
            }
        }
        else
        {
            Debug.Log("item has different item type. (need to equipment type)");
        }

        return equipment;
    }

    public GameObject CreateGroundItem(EConsumableType consumableType, Vector3 position)
    {
        GameObject groundItemObj = Instantiate(groundItemPrefab, position, Quaternion.identity);
        if (groundItemObj.TryGetComponent<GroundItem>(out GroundItem groundItem))
        {
            groundItem.Initialize();
            groundItem.Item.ItemInfo = _itemInfoDic[consumableType.ToString()];
            groundItem.AddSprite();
        }

        return groundItemObj;
    }

    public GameObject CreateGroundItem(EWeaponType weaponType, Vector3 position)
    {
        GameObject groundItemObj = Instantiate(groundItemPrefab, position, Quaternion.identity);
        if (groundItemObj.TryGetComponent<GroundItem>(out GroundItem groundItem))
        {
            groundItem.Initialize();
            groundItem.Item.ItemInfo = _itemInfoDic[weaponType.ToString()];
            groundItem.AddSprite();
        }

        return groundItemObj;
    }

    void _SetUpItemInfoDictionary()
    {
        foreach(var itemInfo in itemInfos)
        {
            _itemInfoDic.Add(itemInfo.name, itemInfo);
        }
    }
}
