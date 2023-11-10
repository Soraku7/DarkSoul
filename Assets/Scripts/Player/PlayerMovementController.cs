using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerMovementController : MonoBehaviour
{
    private CharacterInputSystem _inputSystem;
    
    [SerializeField , Header("Model")] 
    private GameObject model;
    
    private float _curForward;
    private float _curRight;
    private float _velocityForward;
    private float _velocityRight;
    
    private Vector3 _moveDirection;
    
    [SerializeField , Header("Move")] private float moveSpeed;

    [SerializeField , Header(("Run"))] private float runSpeed;

    [SerializeField , Header("Jump")] private float jumpForce;
    [SerializeField, Header("Jump")] private int jumpCount;
    
    
    private bool _isGround;
    private int _curJumpCount = 0;
    
    private Animator _anim;
    private static readonly int Forward = Animator.StringToHash("forward");
    private static readonly int Jump = Animator.StringToHash("jump");

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    
    private void Awake()
    {
        _inputSystem = GetComponent<CharacterInputSystem>();

        _anim = model.GetComponent<Animator>();

        _rigidbody = GetComponent<Rigidbody>();

        _collider = GetComponent<CapsuleCollider>();
    }
    
    private void Update()
    {
        PlayerRotate();
        PlayerJump();
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void PlayerRotate()
    {
        if (_inputSystem.playerMovement != Vector2.zero)
        {
            _anim.SetFloat(Forward, Mathf.Lerp(_anim.GetFloat("forward") , _inputSystem.playerRun? 2.0f : 1.0f , 0.5f), 0.05f, Time.deltaTime);

            _curForward = Mathf.SmoothDamp(_curForward, _inputSystem.playerMovement.y, ref _velocityForward, 0.1f);
            _curRight = Mathf.SmoothDamp(_curRight, _inputSystem.playerMovement.x, ref _velocityRight, 0.1f);
            
            model.transform.forward = Vector3.Slerp(model.transform.forward, transform.forward * _curForward + 
                                                                             transform.right * _curRight, 0.3f);;
        }
        else
        {
            _anim.SetFloat(Forward,  .0f , 0.05f, Time.deltaTime);
        }

        _moveDirection = model.transform.forward;
    }

    private void Movement()
    {
         if (_inputSystem.playerMovement == Vector2.zero) return;
         var speed = _inputSystem.playerRun ? runSpeed : moveSpeed ;
        _rigidbody.MovePosition(_rigidbody.position + _moveDirection * (speed * Time.deltaTime));
    }
    
    private void PlayerJump()
    {
        if (_isGround) _curJumpCount = 0;
        if (_inputSystem.playerJump && _curJumpCount < jumpCount)
        {  
            //重置动画
            _anim.Play(Jump, -1 ,0f);
            _anim.Update(0f);
            
            _anim.SetTrigger(Jump);
            _curJumpCount += 1;
            _isGround = false;
            _rigidbody.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }
}
