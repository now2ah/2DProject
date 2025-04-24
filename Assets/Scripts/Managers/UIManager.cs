using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public InputManagerSO inputManager;

    public GameObject itemCursorSpritePrefab;   //addressable

    [SerializeField] GameObject _inventoryPanel;
    [SerializeField] GameObject _titlePanel;
    [SerializeField] GameObject _inGamePanel;
    [SerializeField] GameObject _pausePanel;

    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [SerializeField] Button pauseResumeButon;
    [SerializeField] Button pauseQuitButton;

    [SerializeField] GameObject tutorialMove;
    [SerializeField] GameObject tutorialLoot;
    [SerializeField] GameObject tutorialAttack;
    [SerializeField] GameObject tutorialJump;

    public GameObject TutorialMove => tutorialMove;
    public GameObject TutorialLoot => tutorialLoot;
    public GameObject TutorialAttack => tutorialAttack;
    public GameObject TutorialJump => tutorialJump;


    Canvas _uiCanvas;

    private void Awake()
    {
        _uiCanvas = transform.GetChild(0).GetComponent<Canvas>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        startButton.onClick.AddListener(_OnStartButton);
        quitButton.onClick.AddListener(() => { Application.Quit(); });
        pauseResumeButon.onClick.AddListener(_TogglePausePanel);
        pauseQuitButton.onClick.AddListener(() => { Application.Quit(); });
        SubscribeEvent();
    }

    public void SubscribeEvent()
    {
        inputManager.OnOpenInventoryPerformed += _OnOpenInventory;
        inputManager.OnPointPerformed += _OnPointPerformed;
        inputManager.OnClickStarted += _OnClickStarted;
        inputManager.OnClickPerformed += _OnClickPerformed;
        inputManager.OnClickCanceled += _OnClickCanceled;
        inputManager.OnPausePerformed += _OnPausePerformed;
        GameManager.Instance.OnStartGame += _OnStartGame;
    }

    private void _OnStartGame(object sender, EventArgs e)
    {
        ShowUI(_inGamePanel);
    }

    public ItemCursorSpriteUI CreateCursorImage()
    {
        ItemCursorSpriteUI itemCursorSpriteUI = null;
        GameObject itemCursorSpriteObj = Instantiate(itemCursorSpritePrefab);
        itemCursorSpriteObj.transform.SetParent(_uiCanvas.transform);
        if (itemCursorSpriteObj.TryGetComponent<ItemCursorSpriteUI>(out ItemCursorSpriteUI itemCursorSpriteComponent))
        {
            itemCursorSpriteUI = itemCursorSpriteComponent;
        }

        return itemCursorSpriteUI;
    }

    public void ShowTitlePanel(bool isOn)
    {
        if (null == _titlePanel)
            return;

        _titlePanel.SetActive(isOn);
    }

    public void ShowUIAtPosition(GameObject ui, Vector3 position)
    {
        if (ui != null)
        {
            ui.SetActive(true);
            if (ui.TryGetComponent<RectTransform>(out RectTransform rectTr))
            {
                rectTr.anchoredPosition = position;
            }
        }
    }

    public void ShowUI(GameObject ui)
    {
        if (ui != null)
        {
            ui.SetActive(true);
        }
    }

    public void HideUI(GameObject ui)
    {
        if (ui != null)
        {
            ui.SetActive(false);
        }
    }

    void _OnStartButton()
    {
        GameManager.Instance.StartGame();
        _titlePanel.SetActive(false);
    }

    void _TogglePausePanel()
    {
        if (!_pausePanel.activeSelf) { _pausePanel.SetActive(true); }
        else { _pausePanel.SetActive(false); }
    }

    private void _OnPointPerformed(object sender, Vector2 e)
    {

    }

    private void _OnClickStarted(object sender, EventArgs e)
    {
        //Debug.Log("started");
    }

    private void _OnClickPerformed(object sender, EventArgs e)
    {
        //Debug.Log("performed");
    }

    private void _OnClickCanceled(object sender, EventArgs e)
    {
        //Debug.Log("canceled");
    }

    void _OnOpenInventory(object sender, EventArgs e)
    {
        if(!_inventoryPanel.activeSelf) { _inventoryPanel.SetActive(true); }
        else { _inventoryPanel.SetActive(false); }
    }

    void _OnPausePerformed(object sender, EventArgs e)
    {
        _TogglePausePanel();
    }
}

