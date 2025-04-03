using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "ScriptableObjects/InputManagerSO")]
public class InputManagerSO : ScriptableObject
{
    public InputActionAsset inputActionAsset;

    private InputAction _moveAction;

    public event EventHandler<Vector2> OnMovePerformed;
    public event EventHandler OnMoveCanceled;

    void OnEnable()
    {
        _FindAllActions();
        _EnableAllActions();

        _moveAction.performed += _MovePerformed;
        _moveAction.canceled += _MoveCanceled;
    }

    private void OnDisable()
    {
        _moveAction.performed -= _MovePerformed;
        _moveAction.canceled -= _MoveCanceled;

        _DisableAllActions();
    }

    void _FindAllActions()
    {
        _moveAction = inputActionAsset.FindAction("Move");
    }

    void _EnableAllActions()
    {
        _moveAction.Enable();
    }

    void _DisableAllActions()
    {
        _moveAction.Disable();
    }

    private void _MovePerformed(InputAction.CallbackContext obj)
    {
        OnMovePerformed.Invoke(obj, obj.ReadValue<Vector2>());
    }
    private void _MoveCanceled(InputAction.CallbackContext obj)
    {
        OnMoveCanceled.Invoke(obj, EventArgs.Empty);
    }
}
