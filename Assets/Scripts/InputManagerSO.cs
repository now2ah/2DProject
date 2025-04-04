using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "ScriptableObjects/InputManagerSO")]
public class InputManagerSO : ScriptableObject
{
    public InputActionAsset inputActionAsset;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;

    public event EventHandler<Vector2> OnMovePerformed;
    public event EventHandler OnMoveCanceled;
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnAttackPerformed;

    void OnEnable()
    {
        _FindAllActions();
        _EnableAllActions();

        _moveAction.performed += _MovePerformed;
        _moveAction.canceled += _MoveCanceled;
        _jumpAction.performed += _JumpPerformed;
        _attackAction.performed += _AttackPerformed;
    }

    private void OnDisable()
    {
        _moveAction.performed -= _MovePerformed;
        _moveAction.canceled -= _MoveCanceled;
        _jumpAction.performed -= _JumpPerformed;
        _attackAction.performed -= _AttackPerformed;

        _DisableAllActions();
    }

    void _FindAllActions()
    {
        _moveAction = inputActionAsset.FindAction("Move");
        _jumpAction = inputActionAsset.FindAction("Jump");
        _attackAction = inputActionAsset.FindAction("Attack");
    }

    void _EnableAllActions()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _attackAction.Enable();
    }

    void _DisableAllActions()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _attackAction.Disable();
    }

    private void _MovePerformed(InputAction.CallbackContext obj)
    {
        OnMovePerformed?.Invoke(obj, obj.ReadValue<Vector2>());
    }
    private void _MoveCanceled(InputAction.CallbackContext obj)
    {
        OnMoveCanceled?.Invoke(obj, EventArgs.Empty);
    }

    private void _JumpPerformed(InputAction.CallbackContext obj)
    {
        OnJumpPerformed?.Invoke(obj, EventArgs.Empty);
    }
    private void _AttackPerformed(InputAction.CallbackContext obj)
    {
        OnAttackPerformed?.Invoke(obj, EventArgs.Empty);
    }
}
