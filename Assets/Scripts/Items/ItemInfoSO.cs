using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoSO", menuName = "ScriptableObjects/ItemInfoSO")]
public class ItemInfoSO : ScriptableObject
{
    public string itemName;
    public EItemType itemType;
    public Sprite itemSprite;
}

[CreateAssetMenu(fileName = "EquipmentInfoSO", menuName = "ScriptableObjects/EquipmentInfoSO")]
public class EquipmentInfoSO : ItemInfoSO
{
    public EEquipmentType equipmentType;
}

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/WeaponInfoSO")]
public class WeaponInfoSO : EquipmentInfoSO
{
    public float damage;
}
