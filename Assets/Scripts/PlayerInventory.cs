using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private List<Item> _inventoryItemList;

    public List<Item> InventoryItemList { get { return _inventoryItemList; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            if (collision.gameObject.TryGetComponent<Item>(out Item item))
            {
                if (_groundItemList != null)
                {
                    _groundItemList.Add(item);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            if (collision.gameObject.TryGetComponent<Item>(out Item item))
            {
                if (_groundItemList != null && _groundItemList.Contains(item))
                {
                    _groundItemList.Remove(item);
                }
            }
        }
    }

    public void LootItem(Item item)
    {
        Debug.Log(item.ItemInfo.name);

        if (_inventoryItemList != null)
        {
            _inventoryItemList.Add(item);
        }
    }

    void _LootItemOnGround()
    {
        OnLoot?.Invoke(this, transform.position);
    }
}
