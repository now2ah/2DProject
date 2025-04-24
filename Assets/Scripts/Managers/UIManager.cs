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
    public float fadeSpeed = 1f;

    [SerializeField] GameObject _inventoryPanel;
    [SerializeField] GameObject _titlePanel;
    [SerializeField] GameObject _inGamePanel;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _uDiedPanel;
    [SerializeField] GameObject _fadePanel;


    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [SerializeField] Button pauseResumeButton;
    [SerializeField] Button pauseQuitButton;

    [SerializeField] Button respawnButton;
    [SerializeField] Button uDiedQuitButton;

    [SerializeField] GameObject tutorialMove;
    [SerializeField] GameObject tutorialLoot;
    [SerializeField] GameObject tutorialAttack;
    [SerializeField] GameObject tutorialJump;

    public GameObject TutorialMove => tutorialMove;
    public GameObject TutorialLoot => tutorialLoot;
    public GameObject TutorialAttack => tutorialAttack;
    public GameObject TutorialJump => tutorialJump;

    public GameObject UDiedPanel => _uDiedPanel;

    Canvas _uiCanvas;

    private void Awake()
    {
        _uiCanvas = transform.GetChild(0).GetComponent<Canvas>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButton);
        quitButton.onClick.AddListener(() => { Application.Quit(); });
        pauseResumeButton.onClick.AddListener(TogglePausePanel);
        pauseQuitButton.onClick.AddListener(() => { Application.Quit(); });
        respawnButton.onClick.AddListener(OnRespawnButton);
        uDiedQuitButton.onClick.AddListener(() => { Application.Quit(); });
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
        _titlePanel.SetActive(false);
        ShowUI(_inGamePanel);
        GameManager.Instance.Player.OnDie += Player_OnDie;
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

    public void FadeIn(Action callback = null)
    {
        StartCoroutine(FadeInCoroutine(callback));
    }

    IEnumerator FadeInCoroutine(Action callback)
    {
        if (_fadePanel != null)
        {
            _fadePanel.SetActive(true);
            if (_fadePanel.TryGetComponent<Image>(out Image image))
            {
                float alpha = 0f;
                while (image.color.a < 1)
                {
                    alpha += (Time.deltaTime * fadeSpeed);
                    image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                    yield return null;
                }
            }
            callback?.Invoke();
        }
    }

    public void FadeOut(Action callback = null)
    {
        StartCoroutine(FadeOutCoroutine(callback));
    }

    IEnumerator FadeOutCoroutine(Action callback)
    {
        if (_fadePanel != null)
        {
            _fadePanel.SetActive(true);
            if (_fadePanel.TryGetComponent<Image>(out Image image))
            {
                float alpha = 1f;
                while (image.color.a > 0)
                {
                    alpha -= (Time.deltaTime * fadeSpeed);
                    image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                    yield return null;
                }
            }
            _fadePanel.SetActive(false);
            callback?.Invoke();
        }
    }

    public void OnStartButton()
    {
        GameManager.Instance.StartGame();
    }

    public void TogglePausePanel()
    {
        if (!_pausePanel.activeSelf) 
        { 
            _pausePanel.SetActive(true);
            Time.timeScale = 0.1f;
        }
        else 
        { 
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void OnRespawnButton()
    {
        if (_uDiedPanel.activeSelf)
        {
            _uDiedPanel.SetActive(false);
            Time.timeScale = 1f;
            GameManager.Instance.RespawnPlayer();
            FadeOut();
        }
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
        TogglePausePanel();
    }

    void Player_OnDie(object sender, EventArgs e)
    {
        ShowUI(_uDiedPanel);
        Time.timeScale = 0.1f;
    }
}

