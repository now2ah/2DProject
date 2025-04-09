using UnityEngine;

public enum EItemType
{
    NORMAL,
    EQUIPMENT,
    WEAPON,
}

public enum EEquipmentType
{
    WEAPON,
}

public class Item
{
    protected ItemInfoSO _itemInfo;

    public ItemInfoSO ItemInfo { get { return _itemInfo; } set { _itemInfo = value; } }

    protected virtual void _Initialize()
    {

    }
}

public class Equipment : Item
{

}

public class Weapon : Equipment
{

}