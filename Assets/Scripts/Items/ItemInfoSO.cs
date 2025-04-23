using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoSO", menuName = "ScriptableObjects/ItemInfoSO")]
public class ItemInfoSO : ScriptableObject
{
    public string itemName;
    public EItemType itemType;
    public Sprite itemSprite;
}

[CreateAssetMenu(fileName = "ConsumableItemInfoSO", menuName = "ScriptableObjects/ConsumableItemInfoSO")]
public class ConsumableItemInfoSO : ItemInfoSO
{
    public EConsumableType consumableType;
    public int amount;
}

[CreateAssetMenu(fileName = "EquipmentInfoSO", menuName = "ScriptableObjects/EquipmentInfoSO")]
public class EquipmentInfoSO : ItemInfoSO
{
    public EEquipmentType equipmentType;
}

[CreateAssetMenu(fileName = "HeadInfoSO", menuName = "ScriptableObjects/HeadInfoSO")]
public class HeadInfoSO : EquipmentInfoSO
{
    public EWeaponType headType;
    public int armor;
}

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/WeaponInfoSO")]
public class WeaponInfoSO : EquipmentInfoSO
{
    public EWeaponType weaponType;
    public int damage;
}

[CreateAssetMenu(fileName = "ArmorInfoSO", menuName = "ScriptableObjects/ArmorInfoSO")]
public class ArmorInfoSO : EquipmentInfoSO
{
    public EArmorType armorType;
    public Sprite itemSprite2;
    public Sprite itemSprite3;
    public int armor;
}

[CreateAssetMenu(fileName = "ShieldInfoSO", menuName = "ScriptableObjects/ShieldInfoSO")]
public class ShieldInfoSO : EquipmentInfoSO
{
    public EShieldType shieldType;
    public int armor;
}
