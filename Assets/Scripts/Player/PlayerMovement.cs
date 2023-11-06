using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected CharacterInputSystem _inputSystem;

    private void Awake()
    {
        _inputSystem = GetComponent<CharacterInputSystem>();
    }

    private void Update()
    {
        Movement();
    }

    
    void Movement()
    {
        if (_inputSystem.playerMovement != Vector2.zero)
        {
            Debug.Log("Move");
        }
        
    }
}
