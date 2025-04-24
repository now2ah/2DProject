using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "ScriptableObjects/InputManagerSO")]
public class InputManagerSO : ScriptableObject
{
    public InputActionAsset inputActionAsset;

    private InputAction _pointAction;
    private InputAction _clickAction;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _interactAction;
    private InputAction _lootAction;
    private InputAction _openInventoryAction;
    private InputAction _pauseAction;

    public event EventHandler<Vector2> OnPointPerformed;

    public event EventHandler OnClickStarted;
    public event EventHandler OnClickPerformed;
    public event EventHandler OnClickCanceled;
    
    public event EventHandler<Vector2> OnMovePerformed;
    public event EventHandler OnMoveCanceled;
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnAttackPerformed;
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnLootPerformed;
    public event EventHandler OnOpenInventoryPerformed;
    public event EventHandler OnPausePerformed;

    void OnEnable()
    {
        _FindAllActions();
        _EnableAllActions();

        _pointAction.performed += _PointPerformed;
        _clickAction.started += _ClickStarted;
        _clickAction.performed += _ClickPerformed;
        _clickAction.canceled += _ClickCanceled;
        _moveAction.performed += _MovePerformed;
        _moveAction.canceled += _MoveCanceled;
        _jumpAction.performed += _JumpPerformed;
        _attackAction.performed += _AttackPerformed;
        _interactAction.performed += _InteractPerformed;
        _lootAction.performed += _LootPerformed;
        _openInventoryAction.performed += _OpenInventoryPerformed;
        _pauseAction.performed += _PausePerformed;
    }

    private void OnDisable()
    {
        _pointAction.performed -= _PointPerformed;
        _clickAction.started -= _ClickStarted;
        _clickAction.performed -= _ClickPerformed;
        _clickAction.canceled -= _ClickCanceled;
        _moveAction.performed -= _MovePerformed;
        _moveAction.canceled -= _MoveCanceled;
        _jumpAction.performed -= _JumpPerformed;
        _attackAction.performed -= _AttackPerformed;
        _interactAction.performed -= _InteractPerformed;
        _lootAction.performed -= _LootPerformed;
        _openInventoryAction.performed -= _OpenInventoryPerformed;
        _pauseAction.performed -= _PausePerformed;

        _DisableAllActions();
    }

    void _FindAllActions()
    {
        _pointAction = inputActionAsset.FindAction("Point");
        _clickAction = inputActionAsset.FindAction("Click");
        _moveAction = inputActionAsset.FindAction("Move");
        _jumpAction = inputActionAsset.FindAction("Jump");
        _attackAction = inputActionAsset.FindAction("Attack");
        _interactAction = inputActionAsset.FindAction("Interact");
        _lootAction = inputActionAsset.FindAction("Loot");
        _openInventoryAction = inputActionAsset.FindAction("OpenInventory");
        _pauseAction = inputActionAsset.FindAction("Pause");
    }

    void _EnableAllActions()
    {
        _pointAction.Enable();
        _clickAction.Enable();
        _moveAction.Enable();
        _jumpAction.Enable();
        _attackAction.Enable();
        _interactAction.Enable();
        _lootAction.Enable();
        _openInventoryAction.Enable();
        _pauseAction.Enable();
    }

    void _DisableAllActions()
    {
        _pointAction.Disable();
        _clickAction.Disable();
        _moveAction.Disable();
        _jumpAction.Disable();
        _attackAction.Disable();
        _interactAction.Disable();
        _lootAction.Disable();
        _openInventoryAction.Disable();
        _pauseAction.Disable();
    }
    private void _PointPerformed(InputAction.CallbackContext obj)
    {
        OnPointPerformed?.Invoke(obj, obj.ReadValue<Vector2>());
    }

    private void _ClickStarted(InputAction.CallbackContext obj)
    {
        OnClickStarted?.Invoke(obj, EventArgs.Empty);
    }

    private void _ClickPerformed(InputAction.CallbackContext obj)
    {
        OnClickPerformed?.Invoke(obj, EventArgs.Empty);
    }

    private void _ClickCanceled(InputAction.CallbackContext obj)
    {
        OnClickCanceled?.Invoke(obj, EventArgs.Empty);
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

    private void _InteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke(obj, EventArgs.Empty);
    }

    private void _LootPerformed(InputAction.CallbackContext obj)
    {
        OnLootPerformed?.Invoke(obj, EventArgs.Empty);
    }

    private void _OpenInventoryPerformed(InputAction.CallbackContext obj)
    {
        OnOpenInventoryPerformed?.Invoke(obj, EventArgs.Empty);
    }

    private void _PausePerformed(InputAction.CallbackContext obj)
    {
        OnPausePerformed?.Invoke(obj, EventArgs.Empty);
    }
}
