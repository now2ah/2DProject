using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public static float LOOT_DISTANCE = 2.0f;

    static float nearestDistance = Mathf.Infinity;

    Item _item;
    SpriteRenderer _spriteRenderer;

    public Item Item { get { return _item; } set { _item = value; } }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        _item = new Item();
        //if (type == EItemType.NORMAL)
        //{
            
        //}
        //else if (type == EItemType.EQUIPMENT)
        //{
        //    _item = new Equipment();
        //}
        //else if (type == EItemType.WEAPON)
        //{
        //    _item = new Weapon();
        //}

        _SubscribeEvent();
    }

    public void AddSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = _item.ItemInfo.itemSprite;
        }
    }

    void _SubscribeEvent()
    {
        GameManager.Instance.Player.OnLoot += Player_OnLoot;
    }

    private void Player_OnLoot(object sender, Vector3 playerPostion)
    {
        float distance = Vector3.Distance(playerPostion, transform.position);
        if (distance <= LOOT_DISTANCE)
        {
            //loot effect

            if (sender is Player)
            {
                ((Player)sender).LootItem(_item);
            }

            Destroy(this.gameObject);
        }
    }
}
