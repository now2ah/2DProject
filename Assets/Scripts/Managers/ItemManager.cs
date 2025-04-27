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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _SetUpItemInfoDictionary();
    }

    public Item CreateItem(ItemInfoSO itemInfo)
    {
        Item createdItem = new Item();
        createdItem.ItemInfo = itemInfo;

        return createdItem;
    }

    public Equipment EquipmentTo(GameObject playerObj, Item item)
    {
        Equipment createdEquipment = null;

        if (item != null && item.ItemInfo.itemType is EItemType.EQUIPMENT)
        {
            if (item.ItemInfo is EquipmentInfoSO)
            {
                //head
                //if (item.ItemInfo is EquipmentInfoSO)
                //{

                //}
                //weapon
                if (item.ItemInfo is WeaponInfoSO)
                {
                    switch (((WeaponInfoSO)item.ItemInfo).weaponType)
                    {
                        case EWeaponType.SWORD:
                            createdEquipment = playerObj.AddComponent<Sword>();
                            createdEquipment.Item = item;
                            ((Sword)createdEquipment).AttackEffectPrefab = swordAttackPrefab;
                            break;
                    }
                }
                //armor
                else if (item.ItemInfo is ArmorInfoSO)
                {
                    switch (((ArmorInfoSO)item.ItemInfo).armorType)
                    {
                        case EArmorType.LETHER_ARMOR:
                            createdEquipment = playerObj.AddComponent<LetherArmor>();
                            createdEquipment.Item = item;
                            break;
                    }
                }
                //shield
            }
        }
        else
        {
            Debug.Log("item has different item type. (need to equipment type)");
        }

        return createdEquipment;
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
            Debug.Log("GroundItemComp");
            groundItem.Initialize();
            groundItem.Item.ItemInfo = _itemInfoDic[weaponType.ToString()];
            groundItem.AddSprite();
        }

        return groundItemObj;
    }

    public GameObject CreateGroundItem(EArmorType armorType, Vector3 position)
    {
        GameObject groundItemObj = Instantiate(groundItemPrefab, position, Quaternion.identity);
        if (groundItemObj.TryGetComponent<GroundItem>(out GroundItem groundItem))
        {
            Debug.Log("GroundItemComp");
            groundItem.Initialize();
            groundItem.Item.ItemInfo = _itemInfoDic[armorType.ToString()];
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
