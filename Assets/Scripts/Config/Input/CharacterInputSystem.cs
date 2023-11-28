using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputSystem : MonoBehaviour
{
    private InputController _inputController;

    private void Awake()
    {
        if (_inputController == null)
            _inputController = new InputController();
        
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    public void DisableInput()
    {
        _inputController.Disable();
    }

    public void EnableInput()
    {
        _inputController.Enable();
    }
    
    public Vector2 PlayerMovement => _inputController.PlayerInput.Movement.ReadValue<Vector2>();

    public bool PlayerRun => _inputController.PlayerInput.Run.phase == InputActionPhase.Performed;

    public bool PlayerJump => _inputController.PlayerInput.Jump.WasPressedThisFrame();

    public bool PlayerRoll => _inputController.PlayerInput.Roll.WasPressedThisFrame();

    public Vector2 CameraLook => _inputController.PlayerInput.Look.ReadValue<Vector2>();

    public bool PlayerAttack => _inputController.PlayerInput.Attack.WasPressedThisFrame();
    
    
}
