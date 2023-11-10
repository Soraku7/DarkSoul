using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMClearSignal : StateMachineBehaviour
{
    [SerializeField] private string[] clearAtEnter;

    [SerializeField] private string[] clearAtExit;
     
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var signal in clearAtExit)
        {
            animator.ResetTrigger(signal);

        }
    }
}
