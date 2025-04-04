using UnityEngine;

public enum EItemType
{
    EQUIPMENT,
}

public enum EEquipmentType
{
    WEAPON,
}

public abstract class Item : MonoBehaviour
{
    protected string _itemName;
    protected EItemType _itemType;

    protected abstract void _Initialize();
}

public abstract class Equipment : Item
{
    protected EEquipmentType _equipmentType;
}

public abstract class Weapon : Equipment
{
    public float damage;

    public abstract void PlayAttackEffect(bool isRight);
}