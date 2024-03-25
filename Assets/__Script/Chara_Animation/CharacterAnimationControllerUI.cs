using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CharacterAnimationControllerUI : MonoBehaviour
{
    [SerializeField]private AnimatedCharacter animatedCharacter;

    [SerializeField]private Button btn_Idle,btn_Stance,btn_Walk,btn_Talk,btn_Attack,btn_Block,btn_Dance,btn_Die;

    private void Awake()
    {
        btn_Idle.onClick.AddListener(animatedCharacter.Idle);
        btn_Stance.onClick.AddListener(animatedCharacter.Stance);
        btn_Walk.onClick.AddListener(animatedCharacter.Walk);
        btn_Talk.onClick.AddListener(animatedCharacter.Talk);
        btn_Attack.onClick.AddListener(animatedCharacter.Attack);
        btn_Block.onClick.AddListener(animatedCharacter.Block);
        btn_Dance.onClick.AddListener(animatedCharacter.Dance);
        btn_Die.onClick.AddListener(animatedCharacter.Die);
        
        
    }



}
