using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to 3D Character Model with animator object and has the following animations
public class AnimatedCharacter : MonoBehaviour
{
    public Animator animator;
    public Motion currentMotion;

    public float corssFadeTime = 0.1f;
    public float actionTime = 1.0f; 
    
    public enum AnimationState
    {
        IDLE,
        STANCE,
        WALK,
        TALK,
        ATTACK,
        BLOCK,
        DANCE,
        VICTORY,
        DIE
    }
    public AnimationState currentState;

    
    public void Idle()
    {
        currentState = AnimationState.IDLE;
        UpdateAnimation(currentState);
    }

    public void Stance()
    {
        currentState = AnimationState.STANCE;
        UpdateAnimation(currentState);
    }

    [ContextMenu("Die")]
    public void Die(){
        currentState = AnimationState.DIE;
        UpdateAnimation(currentState);
    }


    public void Walk(){
        currentState = AnimationState.WALK;
        UpdateAnimation(currentState);
    }

    public void Talk(){
        currentState = AnimationState.TALK;
        UpdateAnimation(currentState);
    }

    public void Attack(){
        currentState = AnimationState.ATTACK;
        UpdateAnimation(currentState);
    }

    public void Block(){
        currentState = AnimationState.BLOCK;
        UpdateAnimation(currentState);
    }

    public void Dance(){
        currentState = AnimationState.DANCE;
        UpdateAnimation(currentState);
    }

    public void Victory(){
        currentState = AnimationState.VICTORY;
        UpdateAnimation(currentState);
    }

    [ContextMenu("AttackOnce")]
    public void AttackOnce()
    {
        StartCoroutine(PlayAttackOnce());
    }
    
    public IEnumerator PlayAttackOnce()
    {
        currentState = AnimationState.ATTACK;
        UpdateAnimation(currentState);
        yield return new WaitForSeconds(actionTime);
        currentState = AnimationState.STANCE;
        UpdateAnimation(currentState);
    }


    
    private void UpdateAnimation(AnimationState state){
        switch (state)
        {
            case AnimationState.IDLE:
                animator.CrossFade("Idle", corssFadeTime);
                break;
            case AnimationState.STANCE:
                animator.CrossFade("Stance", corssFadeTime);
                break;
            case AnimationState.WALK:
                animator.CrossFade("Walk", corssFadeTime);
                break;
            case AnimationState.TALK:
                animator.CrossFade("Talk", corssFadeTime);
                break;
            case AnimationState.ATTACK:
                animator.CrossFade("Attack", corssFadeTime);
                break;
            case AnimationState.BLOCK:
                animator.CrossFade("Block", corssFadeTime);
                break;
            case AnimationState.DANCE: 
                animator.CrossFade("Dance", corssFadeTime);
                break;
            case AnimationState.VICTORY:
                animator.CrossFade("Victory", corssFadeTime);
                break;
            case AnimationState.DIE:
                animator.CrossFade("Dying", corssFadeTime);
                break;
        }
        
    }

}
