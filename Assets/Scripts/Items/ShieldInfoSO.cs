using UnityEngine;

[CreateAssetMenu(fileName = "ShieldInfoSO", menuName = "ScriptableObjects/ShieldInfoSO")]
public class ShieldInfoSO : EquipmentInfoSO
{
    public EShieldType shieldType;
    public int armor;
}
