using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
     private CharacterInputSystem _playerInput;

        [SerializeField] private Transform lookAttarGet;
        private Transform _playerCamera;     

        
        [Range(0.1f, 1.0f),SerializeField,Header("DPI")] public float mouseInputSpeed;
        [Range(0.1f, 0.5f),SerializeField,Header("CameraRotateSmoothness")] public float rotationSmoothTime = 0.12f;

        [SerializeField, Header("ForPlayer")] private float distancePlayerOffset;
        [SerializeField, Header("ForPlayer")] private Vector3 offsetPlayer;
        [SerializeField] private Vector2 clmpCameraRang = new Vector2(-85f, 70f);
        [SerializeField] private float lookAtPlayerLerpTime;

        [SerializeField, Header("LockEnemy")] private bool isLockOn;
        [SerializeField] private Transform currentTarget;

        
        [SerializeField,Header("CameraCollision")] private Vector2 _cameraDistanceMinMax = new Vector2(0.01f, 3f);
        [SerializeField] private float colliderMotionLerpTime; 

        private Vector3 _rotationSmoothVelocity; 
        private Vector3 _currentRotation;       
        private Vector3 _camDirection;         
        private float _cameraDistance;       
        private float _yaw;                        
        private float _pitch;                     

        public LayerMask collisionLayer;       


        
        private void Awake()
        {
            _playerCamera = Camera.main.transform;
            _playerInput = lookAttarGet.transform.root.GetComponent<CharacterInputSystem>();
        }

        private void Start()
        {
            _camDirection = transform.localPosition.normalized;

            _cameraDistance = _cameraDistanceMinMax.y;
        }

        private void Update()
        {
            UpdateCursor();
            GetCameraControllerInput();
        }



        private void LateUpdate()
        {
            ControllerCamera();
            CheckCameraOcclusionAndCollision(_playerCamera);
            //CameraLockOnTarget();
        }

       
        private void ControllerCamera()
        {
            if (!isLockOn)
            {
                _currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_pitch, _yaw), ref _rotationSmoothVelocity, rotationSmoothTime);
                transform.eulerAngles = _currentRotation;
            }

            Vector3 fanlePos = lookAttarGet.position - transform.forward * distancePlayerOffset;

            transform.position = Vector3.Lerp(transform.position, fanlePos, lookAtPlayerLerpTime * Time.deltaTime);
        }

        private void GetCameraControllerInput()
        {
            if (isLockOn) return;

            _yaw += _playerInput.CameraLook.x * mouseInputSpeed;
            _pitch -= _playerInput.CameraLook.y * mouseInputSpeed;
            _pitch = Mathf.Clamp(_pitch, clmpCameraRang.x, clmpCameraRang.y);
        }


       
        private void CheckCameraOcclusionAndCollision(Transform camera)
        {
            Vector3 desiredCamPosition = transform.TransformPoint(_camDirection * 3f);

            if (Physics.Linecast(transform.position, desiredCamPosition, out var hit, collisionLayer))
            {
                _cameraDistance = Mathf.Clamp(hit.distance * .9f, _cameraDistanceMinMax.x, _cameraDistanceMinMax.y);

            }
            else
            {
                _cameraDistance = _cameraDistanceMinMax.y;

            }
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, _camDirection * (_cameraDistance - 0.1f), colliderMotionLerpTime * Time.deltaTime);

        }

       

        private void UpdateCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }



        private void CameraLockOnTarget()
        {
            if (!isLockOn) return;

            Vector3 directionOfTarget = ((currentTarget.position + currentTarget.transform.up * .7f) - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionOfTarget.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }


        public void SetLookPlayerTarget(Transform target)
        {
            lookAttarGet = target;
        }
}
