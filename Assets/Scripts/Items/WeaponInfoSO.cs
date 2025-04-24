using UnityEngine;


[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/WeaponInfoSO")]
public class WeaponInfoSO : EquipmentInfoSO
{
    public EWeaponType weaponType;
    public int damage;
}
