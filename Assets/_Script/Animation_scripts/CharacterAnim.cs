using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    public Animator animator;
    public enum CharacterAnimState{
        IDLE,
        WALK
    }
    [SerializeField] private CharacterAnimState state;
    
    
    public void PlayAnimation(CharacterAnimState animState){
        
        state = animState;
        animator.Play(animState.ToString(),0,1f);
    }

    private void Start()
    {
        PlayAnimation(state);
    }
}
