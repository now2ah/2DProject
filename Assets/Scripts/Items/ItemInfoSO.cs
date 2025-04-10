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
    public float amount;
}

[CreateAssetMenu(fileName = "EquipmentInfoSO", menuName = "ScriptableObjects/EquipmentInfoSO")]
public class EquipmentInfoSO : ItemInfoSO
{
    public EEquipmentType equipmentType;
}

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/WeaponInfoSO")]
public class WeaponInfoSO : EquipmentInfoSO
{
    public EWeaponType weaponType;
    public float damage;
}

[CreateAssetMenu(fileName = "ShieldInfoSO", menuName = "ScriptableObjects/ShieldInfoSO")]
public class ShieldInfoSO : EquipmentInfoSO
{
    public EShieldType shieldType;
    public float armor;
}
