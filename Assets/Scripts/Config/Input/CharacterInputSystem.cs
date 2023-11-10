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
        _inputController.Enable();
    }

    private void OnDisable()
    {
        _inputController.Disable();
    }
    
    public Vector2 playerMovement
    {
        get => _inputController.PlayerInput.Movement.ReadValue<Vector2>();
    }

    public bool playerRun
    {
        get => _inputController.PlayerInput.Run.phase == InputActionPhase.Performed;
    }

    public bool playerJump
    {
        get => _inputController.PlayerInput.Jump.WasPressedThisFrame();
    }
}
