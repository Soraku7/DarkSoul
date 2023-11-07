using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovementController : MonoBehaviour
{
    private CharacterInputSystem _inputSystem;
    
    [FormerlySerializedAs("_model")]
    [Header("模型")]
    [SerializeField] 
    private GameObject model;
    
    private float _curForward;
    private float _curRight;
    private float _velocityForward;
    private float _velocityRight;
    
    
    private Animator _anim;
    private static readonly int Forward = Animator.StringToHash("forward");

    private void Awake()
    {
        _inputSystem = GetComponent<CharacterInputSystem>();

        _anim = model.GetComponent<Animator>();
    }
    
    private void Update()
    {
        Movement();
    }


    private void Movement()
    {
        if (_inputSystem.playerMovement != Vector2.zero)
        {
            _anim.SetFloat(Forward,  1.0f , 0.05f, Time.deltaTime);

            _curForward = Mathf.SmoothDamp(_curForward, _inputSystem.playerMovement.y, ref _velocityForward, 0.1f);
            _curRight = Mathf.SmoothDamp(_curRight, _inputSystem.playerMovement.x, ref _velocityRight, 0.1f);
            
            model.transform.forward = transform.forward * _curForward +
                                      transform.right * _curRight;
        }
        else
        {
            _anim.SetFloat(Forward,  .0f , 0.05f, Time.deltaTime);
        }
        
        
    }
}
