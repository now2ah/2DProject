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
    private InputAction _interactAction;
    private InputAction _lootAction;
    private InputAction _openInventoryAction;

    public event EventHandler<Vector2> OnMovePerformed;
    public event EventHandler OnMoveCanceled;
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnAttackPerformed;
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnLootPerformed;
    public event EventHandler OnOpenInventoryPerformed;

    void OnEnable()
    {
        _FindAllActions();
        _EnableAllActions();

        _moveAction.performed += _MovePerformed;
        _moveAction.canceled += _MoveCanceled;
        _jumpAction.performed += _JumpPerformed;
        _attackAction.performed += _AttackPerformed;
        _interactAction.performed += _InteractPerformed;
        _lootAction.performed += _LootPerformed;
        _openInventoryAction.performed += _OpenInventoryPerformed;
    }

    private void OnDisable()
    {
        _moveAction.performed -= _MovePerformed;
        _moveAction.canceled -= _MoveCanceled;
        _jumpAction.performed -= _JumpPerformed;
        _attackAction.performed -= _AttackPerformed;
        _interactAction.performed -= _InteractPerformed;
        _lootAction.performed -= _LootPerformed;
        _openInventoryAction.performed -= _OpenInventoryPerformed;

        _DisableAllActions();
    }

    void _FindAllActions()
    {
        _moveAction = inputActionAsset.FindAction("Move");
        _jumpAction = inputActionAsset.FindAction("Jump");
        _attackAction = inputActionAsset.FindAction("Attack");
        _interactAction = inputActionAsset.FindAction("Interact");
        _lootAction = inputActionAsset.FindAction("Loot");
        _openInventoryAction = inputActionAsset.FindAction("OpenInventory");
    }

    void _EnableAllActions()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _attackAction.Enable();
        _interactAction.Enable();
        _lootAction.Enable();
        _openInventoryAction.Enable();
    }

    void _DisableAllActions()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _attackAction.Disable();
        _interactAction.Disable();
        _lootAction.Disable();
        _openInventoryAction.Disable();
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
}
