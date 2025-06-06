using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public static float LOOT_DISTANCE = 2.0f;

    Item _item;
    SpriteRenderer _spriteRenderer;

    public Item Item { get { return _item; } set { _item = value; } }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        _UnSubscribeEvent();
    }

    public void Initialize()
    {
        _item = new Item();

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

    void _UnSubscribeEvent()
    {
        GameManager.Instance.Player.OnLoot -= Player_OnLoot;
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
