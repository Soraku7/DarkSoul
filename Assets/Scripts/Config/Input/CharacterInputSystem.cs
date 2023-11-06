using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
