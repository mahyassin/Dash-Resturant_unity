using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private NewActions inputActions;
    public event Action<Vector2> Moved;
    public event Action Interacted;
    public InputReader()
    {
        inputActions = new();
        inputActions.Enable();

        inputActions.PCMap.MoveAction.performed += OnMovePressed;
        inputActions.PCMap.MoveAction.canceled += OnMoveCancel;

        inputActions.PCMap.InteractionAction.performed += InterActionPressed;

    }

    private void OnMovePressed(InputAction.CallbackContext ctx)
    {
        Moved?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        Moved?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void InterActionPressed(InputAction.CallbackContext ctx)
    {
        Interacted?.Invoke();
    }
}
