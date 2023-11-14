using UnityEngine;


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
    [SerializeField, Header("Jump")] private int jabMultiplier;
    private int _curJumpCount;
    
    private Animator _anim;
    private static readonly int Forward = Animator.StringToHash("forward");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int IsGround = Animator.StringToHash("isGround");
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int JabVelocity = Animator.StringToHash("jabVelocity");
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _inputSystem = GetComponent<CharacterInputSystem>();

        _anim = model.GetComponent<Animator>();

        _rigidbody = GetComponent<Rigidbody>();
        
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

    private void PlayerRotate()
    {
        if (_inputSystem.PlayerMovement != Vector2.zero)
        {
            _anim.SetFloat(Forward, Mathf.Lerp(_anim.GetFloat(Forward) , _inputSystem.PlayerRun? 2.0f : 1.0f , 0.5f), 0.05f, Time.deltaTime);

            _curForward = Mathf.SmoothDamp(_curForward, _inputSystem.PlayerMovement.y, ref _velocityForward, 0.1f);
            _curRight = Mathf.SmoothDamp(_curRight, _inputSystem.PlayerMovement.x, ref _velocityRight, 0.1f);
            
            model.transform.forward = Vector3.Slerp(model.transform.forward, transform.forward * _curForward + transform.right * _curRight, 0.3f);;
        }
        else
        {
            _anim.SetFloat(Forward,  .0f , 0.05f, Time.deltaTime);
        }

        _moveDirection = model.transform.forward;
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
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _anim.SetBool(IsGround , false);
        }
    }

    #region Anim

    private void OnJabUpdate()
    {
        _rigidbody.velocity = model.transform.forward * _anim.GetFloat(JabVelocity);  
    }
    #endregion
    
}
