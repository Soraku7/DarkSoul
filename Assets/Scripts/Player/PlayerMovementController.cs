using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    protected CharacterInputSystem _inputSystem;
    
    [Header("模型")]
    [SerializeField] 
    private GameObject _model;
    
    private Animator _anim;
    private void Awake()
    {
        _inputSystem = GetComponent<CharacterInputSystem>();

        _anim = _model.GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
    }


    private void Movement()
    {
        if (_inputSystem.playerMovement != Vector2.zero)
        {
            _anim.SetFloat("forward" , 1.0f, 0.05f , Time.deltaTime);
        }
        else
        {
            _anim.SetFloat("forward" , 0f, 0.05f , Time.deltaTime);
        }
        
    }
}
