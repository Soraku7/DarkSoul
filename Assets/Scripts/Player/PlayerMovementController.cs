using System;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovementController : MonoBehaviour
{
    private CharacterInputSystem _inputSystem;
    private CameraController _cameraController;
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
    [SerializeField, Header("Jump")] private int jabMultiplier;
    private int _curJumpCount;
    
    private Animator _anim;
    private static readonly int Forward = Animator.StringToHash("forward");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int IsGround = Animator.StringToHash("isGround");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int JabVelocity = Animator.StringToHash("jabVelocity");
    
    private Rigidbody _rigidbody;
    
    [SerializeField,Header("相机锁定点")] private Transform standCameraLook;
    [SerializeField]private Transform crouchCameraLook;
    private Transform _characterCamera;
    
    private float targetRotation;
    private float rotationVelocity;
    private Vector3 movementDirection;
    
    private void Awake()
    {
        _inputSystem = GetComponent<CharacterInputSystem>();

        _anim = model.GetComponent<Animator>();

        _rigidbody = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            _characterCamera = Camera.main.transform.root;
            _cameraController = _characterCamera.GetComponent<CameraController>();
        }
    }
    
    private void Update()
    {
        PlayerRotate();
        PlayerJump();
        PlayRoll();
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void LateUpdate()
    {
        _cameraController.SetLookPlayerTarget(standCameraLook);
    }

    private void PlayerRotate()
    {
        if (_inputSystem.PlayerMovement != Vector2.zero)
        {
            _anim.SetFloat(Forward, Mathf.Lerp(_anim.GetFloat(Forward) , _inputSystem.PlayerRun? 2.0f : 1.0f , 0.5f), 0.05f, Time.deltaTime);

            _curForward = Mathf.SmoothDamp(_curForward, _inputSystem.PlayerMovement.y, ref _velocityForward, 0.1f);
            _curRight = Mathf.SmoothDamp(_curRight, _inputSystem.PlayerMovement.x, ref _velocityRight, 0.1f);
            
            targetRotation = Mathf.Atan2(_inputSystem.PlayerMovement.x, _inputSystem.PlayerMovement.y) * Mathf.Rad2Deg + _characterCamera.localEulerAngles.y;
            
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, 0.3f);

            var direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            
            direction = direction.normalized;
            _moveDirection = Vector3.Slerp(model.transform.forward, direction, 0.3f);
            //model.transform.forward = Vector3.Slerp(model.transform.forward, transform.forward * _curForward + transform.right * _curRight, 0.3f);
        }
        else
        {
            _anim.SetFloat(Forward,  .0f , 0.05f, Time.deltaTime);
            _moveDirection = Vector3.zero;
        }

        
    }

    private void Movement()
    {
         if (_inputSystem.PlayerMovement == Vector2.zero) return;
         
         var speed = _inputSystem.PlayerRun ? runSpeed : moveSpeed ;
        _rigidbody.MovePosition(_rigidbody.position + _moveDirection * (speed * Time.deltaTime));
        
    }
    
    private void PlayerJump()
    {
        if (_anim.GetBool(IsGround)) _curJumpCount = 0;

        switch (_inputSystem.PlayerJump)
        {
            //后跳判断 
            case true when _anim.GetFloat(Forward) < 0.1:
                _anim.SetTrigger(Jump);
                break;
            case true when _curJumpCount < jumpCount:
                //重置动画
                _anim.Play(Jump, -1 ,0f);
                _anim.Update(0f);
            
                _anim.SetTrigger(Jump);
                _curJumpCount += 1;
                _anim.SetBool(IsGround , false);
                _rigidbody.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
                break;
        }
    }

    private void PlayRoll()
    {
        if (_rigidbody.velocity.magnitude > 5.0f || _inputSystem.PlayerRoll)
        {
            _anim.SetTrigger(Roll);
        }
    }
    
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _anim.SetBool(IsGround , true);
        }
    }

    #region Anim
    private void OnJabUpdate()
    {
        _rigidbody.velocity = model.transform.forward * _anim.GetFloat(JabVelocity);  
    }
    #endregion
    
}
