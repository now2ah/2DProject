using System.Collections.Generic;
using UnityEngine;

public enum EItem
{

}

public enum EEquipment
{

}

public enum EWeapon
{
    SWORD,
}

public class ItemManager : Singleton<ItemManager>
{
    public ItemInfoSO[] itemInfos;
    public GameObject groundItemPrefab;

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

    public GameObject CreateGroundItem(EWeapon weapon, Vector3 position)
    {
        GameObject groundItemObj = Instantiate(groundItemPrefab, position, Quaternion.identity);
        if (groundItemObj.TryGetComponent<GroundItem>(out GroundItem groundItem))
        {
            groundItem.Initialize(EItemType.WEAPON);
            groundItem.Item.ItemInfo = _itemInfoDic[weapon.ToString()];
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
