using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState { ALIVE, DEAD }

//Its parent object is PartyManager.
public class Character : MonoBehaviour
{
    //visible basic stats
    public string characterName;
    [SerializeField][TextArea(10,15)] private string description;
    public List<BattleAction> actions = new List<BattleAction>();
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int maxMP;
    [SerializeField] private int currentMP;   
    
    //primitive stats from the characterData.Minimum 1 Maximum 10
    public int strength; //affect damage
    public int agility; //affect dodge chance
    public int precision; //affect critical hit chance  
    public int speed; // affect turn order
    public int luck; //affect all
    
    //temporary stats - buffs and debuffs
    public int damageAddBuff; //applpies to the final damage dealt
    public int shieldBuff; //reduces damage taken
    
    //secondary stats generated from primitive stats


    public CharacterState characterState;
    
    public Sprite fullBodySprite, pfpSprite;

    //effects
    public GameObject placeHolder_fx;

    public string GetDescription(){
        return description;
    }


    public int GetMaxHP(){
        return maxHP;
    }

    public int GetCurrentHP(){
        return currentHP;
    }   

    public int GetMaxMP(){
        return maxMP;
    }
    public int GetCurrentMP(){
        return currentMP;
    }

    //returns the party type of the parent PartyManager
    public PartyType GetPartyType(){
        return transform.parent.GetComponent<PartyManager>().GetPartyType();
    }

    public PartyType GetOppositePartyType(){
        return transform.parent.GetComponent<PartyManager>().GetOppositePartyType();
    }

    public BattleAction GetRandomBattleAction(ActionType actionType){
        List<BattleAction> attackActions = new List<BattleAction>();
        foreach(BattleAction action in actions){
            if(action.actionType == actionType){
                attackActions.Add(action);
            }
        }
        return attackActions[UnityEngine.Random.Range(0, attackActions.Count)];
        
    }
    
    public void TakeDamage(int damage){
        currentHP -= damage;
        if(currentHP <= 0){
            characterState = CharacterState.DEAD;
        }
    }

    public void Heal(int heal){
        currentHP += heal;
        Debug.Log(characterName + " healed for " + heal + " HP.");
        if (currentHP > maxHP){
            currentHP = maxHP;
        }
    }

    public void UseMP(int mp){
        if(currentMP-mp < 0){
            Debug.Log(characterName + " does not have enough MP.");
        }else{
            currentMP -= mp;
            Debug.Log(characterName + " used " + mp + " MP.");
        }
    }

    public void RestoreMP(int mp){
        currentMP += mp;
        Debug.Log(characterName + " restored " + mp + " MP.");  
        if(currentMP > maxMP){
            currentMP = maxMP;
        }
    }
    
}
