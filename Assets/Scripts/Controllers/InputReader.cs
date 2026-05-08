using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader
{
    private NewActions inputActions;
    public event Action<Vector2> Moved;
    public event Action Interacted;
    public event Action Tested;

    private bool _inputLock = false;
    public InputReader()
    {
        inputActions = new();
        inputActions.Enable();

        inputActions.PCMap.MoveAction.performed += OnMovePressed;
        inputActions.PCMap.MoveAction.canceled  += OnMoveCancel;
        inputActions.PCMap.Test.performed       += TestPressed;
        inputActions.PCMap.ScreenSwiped.performed += TestPressed;
        inputActions.PCMap.ScreenTouched.canceled += OnMoveCancel;

        inputActions.PCMap.InteractionAction.performed += InterActionPressed;

    }

    private void OnMovePressed(InputAction.CallbackContext ctx)
    {

        var input = ctx.ReadValue<Vector2>();


        if(_inputLock) return;

        Moved?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        _inputLock = false;
    }

    private void InterActionPressed(InputAction.CallbackContext ctx)
    {
        Interacted?.Invoke();
    }

    private void TestPressed(InputAction.CallbackContext ctx)
    {
        
        var input = ctx.ReadValue<Vector2>();
        var magnitude = input.magnitude;
        if(magnitude > 15)
        {
            if(_inputLock) return;

            Moved?.Invoke(input.normalized);
            _inputLock = true;

        }
      
    }
}
