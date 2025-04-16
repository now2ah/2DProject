using UnityEngine;
using UnityEngine.UI;

public class ItemCursorSpriteUI : MonoBehaviour
{
    public InputManagerSO inputManager;

    Image _image;

    float _mouseX;
    float _mouseY;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _SubscribeEvent();
    }

    public void SetSprite(Sprite sprite)
    {
        if (_image != null)
        {
            _image.sprite = sprite;
            _image.SetNativeSize();
        }
    }

    void _OnPointPerformed(object sender, Vector2 e)
    {
        _mouseX = e.x;
        _mouseY = e.y;
    }

    void _SubscribeEvent()
    {
        if (null == inputManager)
            return;

        inputManager.OnPointPerformed += _OnPointPerformed;
    }

    void _FollowCursor()
    {
        transform.position = new Vector2(_mouseX, _mouseY);
    }

    void Update()
    {
        //follow cursor
        _FollowCursor();
    }
}
