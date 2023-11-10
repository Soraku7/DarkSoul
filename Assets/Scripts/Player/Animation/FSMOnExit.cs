using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMOnExit : StateMachineBehaviour
{
    public string[] onExistMessage;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var msg in onExistMessage)
        {
            animator.gameObject.SendMessageUpwards(msg);
        }
    }

}
