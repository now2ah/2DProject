using UnityEngine;

public enum EItemType
{
    NORMAL,
    EQUIPMENT,
    CONSUMABLE,
}

public enum EConsumableType
{
    APPLE,
}

public enum EEquipmentType
{
    HEAD,
    WEAPON,
    ARMOR,
    SHIELD,
}

public enum EWeaponType
{
    SWORD,
}

public enum EArmorType
{
    LETHER_ARMOR,
}

public enum EShieldType
{

}

public class Item
{
    protected ItemInfoSO _itemInfo;

    public ItemInfoSO ItemInfo { get { return _itemInfo; } set { _itemInfo = value; } }

    protected virtual void _Initialize() { }
}

public interface IUsable
{
    public void Use();
}

public class ConsumableItem : IUsable
{
    public virtual void Use() { }
}

public abstract class Equipment : MonoBehaviour
{
    EEquipmentType _type;
    protected Item _item;

    public EEquipmentType Type { get { return _type; } set { _type = value; } }
    public Item Item { get { return _item; } set { _item = value; } }
}

public abstract class Weapon : Equipment
{
    public abstract void PlayAttackEffect(bool isRight);
}

public abstract class Armor : Equipment
{

}