using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemInfoSO", menuName = "ScriptableObjects/ConsumableItemInfoSO")]
public class ConsumableItemInfoSO : ItemInfoSO
{
    public EConsumableType consumableType;
    public int amount;
}

