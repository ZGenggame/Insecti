using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordEvent : MonoBehaviour {
    public MoveCube Parent;

    public Animator m_Animator;
    private GameObject CurrentEffect;

    public void AnimationDeadEffectEvent() {
        CurrentEffect = Instantiate(Parent.DeadEffectParticale, transform.position
            , Parent.DeadEffectParticale.transform.rotation);
    }

    //脚本驱动事件
    public void ToDead() {
        m_Animator.SetTrigger("IsDead");
    }

    void OnDestroy()
    {
        if (CurrentEffect != null)
        {
            Destroy(CurrentEffect,1f);
        }
    }
    
}
