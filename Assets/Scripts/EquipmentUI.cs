using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject[] equipmentSlots;

    Equipment[] _equipments;

    private void Awake()
    {
        _equipments = new Equipment[Player.MAX_EQUIPMENT_SLOT];
    }

    private void OnEnable()
    {
        _SubscribeEvent();
    }

    void _SubscribeEvent()
    {
        if (null == GameManager.Instance.Player) { return; }

        GameManager.Instance.Player.OnEquip += Player_OnEquip;
    }

    private void Player_OnEquip(object sender, System.EventArgs e)
    {
        _RefreshEquipmentUI();
        _ShowEquipment();
    }

    void _RefreshEquipmentUI()
    {
        _equipments = _GetPlayerEquipments();
        _ShowEquipment();
    }

    Equipment[] _GetPlayerEquipments()
    {
        if (_equipments != null)
        {
            return GameManager.Instance.Player.Equipments;
        }
        else { return null; }
    }

    void _ShowEquipment()
    {
        for(int i=0; i < _equipments.Length; ++i)
        {
            if (equipmentSlots[i].transform.GetChild(0).TryGetComponent<Image>(out Image image))
            {
                if (null == _equipments[i])
                    continue;

                image.sprite = _equipments[i].Item.ItemInfo.itemSprite;
                image.gameObject.SetActive(true);
            }
        }
    }
}
