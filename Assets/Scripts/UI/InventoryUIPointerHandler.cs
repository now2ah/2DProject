using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemSlot
{
    public GameObject uiObject;
    public Sprite itemSprite;

    public abstract void SetSlot(Item item, GameObject go, Sprite sprite);
}

public class InventoryUIPointerHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    EquipmentUI _equipmentUI;
    InventoryUI _inventoryUI;

    Item _selectedItem;
    ItemSlot _selectedSlot;

    ItemCursorSpriteUI _itemCursorSprite;

    public Item SelectedItem { get { return _selectedItem; } set { _selectedItem = value; } }
    public ItemSlot SelectedSlot { get { return _selectedSlot; } set { _selectedSlot = value; } }

    void Awake()
    {
        _equipmentUI = transform.GetChild(0).GetComponent<EquipmentUI>();
        _inventoryUI = transform.GetChild(1).GetComponent<InventoryUI>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySfx(ESFX.PUT_ITEM);

        RaycastResult hit = eventData.pointerCurrentRaycast;

        //check slot (equipment? or inventory?)
        //check selected item
        //if equipment
        ////if equipment item
        //////equip
        ////else if inventory item
        //////equip
        //else if inventory
        ////if inventory item
        //////swap
        ////if equipment item
        //////unequip
        ///
        if (_equipmentUI.IsEquipmentSlot(hit.gameObject))
        {
            if (null == _selectedItem)
                return;

            _equipmentUI.EquipItem(hit.gameObject);

            _equipmentUI.RefreshEquipmentUI();
            _inventoryUI.RefreshInventoryUI();
        }
        else if (_inventoryUI.IsItemInventorySlot(hit.gameObject))
        {
            if (!_IsEquipmentSlot(_selectedSlot))
            {
                _inventoryUI.SwapItem(hit.gameObject);

                _inventoryUI.RefreshInventoryUI();
            }
            else if (_IsEquipmentSlot(_selectedSlot))
            {
                _inventoryUI.UnEquipItem(hit.gameObject);

                _equipmentUI.RefreshEquipmentUI();
                _inventoryUI.RefreshInventoryUI();
            }
        }

        _DestroyCursorImage();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySfx(ESFX.PUT_ITEM);

        RaycastResult hit = eventData.pointerCurrentRaycast;

        //check slot (equipment? or inventory?)
        //if equipment
        ////select equipment item
        //else if inventory
        ////select inventory item

        if (_equipmentUI.IsEquipmentSlot(hit.gameObject))
        {
            _selectedSlot = _equipmentUI.SelectEquipmentItem(hit.gameObject);

            _CreateCursorImage();
        }
        else if (_inventoryUI.IsItemInventorySlot(hit.gameObject))
        {
            _selectedSlot = _inventoryUI.SelectInventoryItem(hit.gameObject);

            _CreateCursorImage();
        }
        else
        {
            _selectedItem = null;
        }
    }

    bool _IsEquipmentSlot(ItemSlot slot)
    {
        if (slot is EquipmentSlot)
            return true;
        else
            return false;
    }

    void _CreateCursorImage()
    {
        _itemCursorSprite = UIManager.Instance.CreateCursorImage();
        _itemCursorSprite.SetSprite(_selectedItem.ItemInfo.itemSprite);
    }

    void _DestroyCursorImage()
    {
        if (null == _itemCursorSprite)
            return;

        Destroy(_itemCursorSprite.gameObject);
    }
}
