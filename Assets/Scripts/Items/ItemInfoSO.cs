using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoSO", menuName = "ScriptableObjects/ItemInfoSO")]
public class ItemInfoSO : ScriptableObject
{
    public string itemName;
    public EItemType itemType;
    public Sprite itemSprite;
}







