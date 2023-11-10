using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FootIK : MonoBehaviour
{
    private Animator _theAnimator;
    
    //射线检测所需要的IK位置
    private Vector3 _leftFootIK, _rightFootIK;

    private Vector3 _leftFootPosition, _rightFootPosition;
    private Quaternion _leftFootRotation, _rightFootRotation;
    
    [Header("射线检测")]
    //射线检测的层级
    [SerializeField] private LayerMask iKLayer;
    //射线检测位置与IK位置偏移值
    [SerializeField] [Range(0, 0.2f)] private float rayHitOffset;
    //射线检测距离
    [SerializeField] private float rayCastDistance;


    [Header("IK设置")]
    [SerializeField] private bool enableIK = true;
    [SerializeField] private float iKSphereRadius = 0.05f;
    [SerializeField] private float positionSphereRadius = 0.05f;

    private void Awake()
    {
        _theAnimator = GetComponent<Animator>();
        
        // //获取IK位置
         //_leftFootIK = _theAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);
        // _rightFootIK = _theAnimator.GetIKPosition(AvatarIKGoal.RightFoot);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _leftFootIK = _theAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);
        _rightFootIK = _theAnimator.GetIKPosition(AvatarIKGoal.RightFoot);

        if (!enableIK)
        {
            Debug.LogError("未启用IK");
            return;
        }

        #region 设置IK权重
        //设置IK权重 1为都由IK控制
        _theAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot , _theAnimator.GetFloat("LIK"));
        _theAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot , _theAnimator.GetFloat("LIK"));
        
        _theAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot , _theAnimator.GetFloat("RIK"));
        _theAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot , _theAnimator.GetFloat("RIK"));
        #endregion

        #region 设置IK位置和旋转值

        _theAnimator.SetIKPosition(AvatarIKGoal.LeftFoot , _leftFootPosition);
        _theAnimator.SetIKRotation(AvatarIKGoal.LeftFoot , _leftFootRotation);
        
        _theAnimator.SetIKPosition(AvatarIKGoal.RightFoot , _rightFootPosition);
        _theAnimator.SetIKRotation(AvatarIKGoal.RightFoot , _rightFootRotation);
        #endregion
        
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(_leftFootIK + (Vector3.up * 0.5f), _leftFootIK + Vector3.down * rayCastDistance, Color.blue,
            Time.deltaTime);
        Debug.DrawLine(_rightFootIK + (Vector3.up * 0.5f), _rightFootIK + Vector3.down * rayCastDistance, Color.blue,
            Time.deltaTime);

        #region 获得旋转值和位置

        if (Physics.Raycast(_leftFootIK + (Vector3.up * 0.5f), Vector3.down, out RaycastHit hit, rayCastDistance + 1,
                iKLayer))
        {
            Debug.DrawRay(hit.point , hit.normal , Color.red , Time.deltaTime);
            //如果让脚的位置等于碰撞点 则可能穿模
            _leftFootPosition = hit.point + Vector3.up * rayHitOffset;
            _leftFootRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
        }        
        
        if (Physics.Raycast(_rightFootIK + (Vector3.up * 0.5f), Vector3.down, out RaycastHit hit_01, rayCastDistance + 1,
                iKLayer))
        {
            Debug.DrawRay(hit_01.point , hit_01.normal , Color.red , Time.deltaTime);
            //如果让脚的位置等于碰撞点 则可能穿模
            _rightFootPosition = hit_01.point + Vector3.up * rayHitOffset;
            _rightFootRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

        }    
        #endregion
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_leftFootIK , iKSphereRadius);
        Gizmos.DrawSphere(_rightFootIK , iKSphereRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_leftFootPosition , positionSphereRadius);
        Gizmos.DrawSphere(_rightFootPosition , positionSphereRadius);
    }
}
