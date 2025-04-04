using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatLandInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;  
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool MoveIsPressed = false;
    public bool InvertMouseY { get; private set; } = true;
    public bool JumpIsPressed { get; private set; } = false;


    InputActions input = null;

    private void OnEnable()
    {
        input = new InputActions();
        input.RatLand.Enable();

        input.RatLand.Move.performed += SetMove;
        input.RatLand.Move.canceled += SetMove;

        input.RatLand.Look.performed += SetLook;
        input.RatLand.Look.canceled += SetLook;

        input.RatLand.Jump.started += SetJump;
        input.RatLand.Jump.canceled += SetJump;



    }

    private void OnDisable()
    {
        input.RatLand.Move.performed -= SetMove;
        input.RatLand.Move.canceled -= SetMove;

        input.RatLand.Look.performed -= SetLook;
        input.RatLand.Look.canceled -= SetLook;

        input.RatLand.Jump.performed -= SetJump;
        input.RatLand.Jump.canceled -= SetJump;

        input.RatLand.Disable();

    }

    private void SetMove (InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        MoveIsPressed = !(MoveInput == Vector2.zero);
    }

    private void SetLook(InputAction.CallbackContext ctx)
    {
       LookInput = ctx.ReadValue<Vector2>();
    }
    private void SetJump(InputAction.CallbackContext ctx)
    {
        JumpIsPressed = ctx.started;
    }

}
