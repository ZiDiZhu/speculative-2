using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator animator;
    public Motion currentMotion;

    public float corssFadeTime = 0.1f;
    public float actionTime = 1.0f; 
    
    public enum AnimationState
    {
        IDLE,
        FIGHT_IDLE,
        WALK,
        ATTACK,
        DIE
    }
    public AnimationState currentState;

    
    public void Idle()
    {
        currentState = AnimationState.IDLE;
        UpdateAnimation(currentState);
    }

    public void FightIdle()
    {
        currentState = AnimationState.FIGHT_IDLE;
        UpdateAnimation(currentState);
    }

    [ContextMenu("Die")]
    public void Die(){
        currentState = AnimationState.DIE;
        UpdateAnimation(currentState);
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        StartCoroutine(AttackAction());
    }
    
    public IEnumerator AttackAction()
    {
        currentState = AnimationState.ATTACK;
        UpdateAnimation(currentState);
        yield return new WaitForSeconds(actionTime);
        currentState = AnimationState.FIGHT_IDLE;
        UpdateAnimation(currentState);
    }


    
    private void UpdateAnimation(AnimationState state){
        switch (state)
        {
            case AnimationState.IDLE:
                animator.CrossFade("Idle", corssFadeTime);
                break;
            case AnimationState.FIGHT_IDLE:
                animator.CrossFade("FightIdle", corssFadeTime);
                break;
            case AnimationState.WALK:
                animator.CrossFade("Walk", corssFadeTime);
                break;
            case AnimationState.ATTACK:
                animator.CrossFade("Attack", corssFadeTime);
                break;
            case AnimationState.DIE:
                animator.CrossFade("Dying", corssFadeTime);
                break;
        }
        
    }

}
