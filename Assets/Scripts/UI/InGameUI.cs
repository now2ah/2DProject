using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _dungeonLevelText;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] TextMeshProUGUI _expText;
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _expSlider;

    private void Awake()
    {
        GameManager.Instance.Player.OnStatChanged += Player_OnStatChanged;
    }

    private void Player_OnStatChanged(object sender, System.EventArgs e)
    {
        _UpdatePlayerValues();
    }

    void _UpdatePlayerValues()
    {
        if (_levelText != null)
        {
            _levelText.text = "Level.\n" + GameManager.Instance.Player.Level;
        }

        if (_hpText != null)
        {
            _hpText.text = GameManager.Instance.Player.CurrentHP + " / " + GameManager.Instance.Player.maxHP;
        }

        if (_expText != null)
        {
            _expText.text = GameManager.Instance.Player.CurrentExp + " / " + GameManager.Instance.Player.NextExp;
        }

        if (_hpSlider != null)
        {
            _hpSlider.value = (float)GameManager.Instance.Player.CurrentHP / (float)GameManager.Instance.Player.maxHP;
        }

        if (_expSlider != null)
        {
            _expSlider.value = (float)GameManager.Instance.Player.CurrentExp / (float)GameManager.Instance.Player.NextExp;
        }
    }
}
