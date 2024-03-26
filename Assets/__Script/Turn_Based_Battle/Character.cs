using System.Collections.Generic;
using UnityEngine;
using static BattleSkill;
using static PartyManager;


//Its parent object is PartyManager.
public class Character : MonoBehaviour
{
    public enum CharacterState { ALIVE, DEAD }

    //visible basic stats
    public string characterName;
    [SerializeField][TextArea(10,15)] private string description;
    
public List<BattleSkill> actions = new List<BattleSkill>();
    public int maxHP { get; private set; }
    public int currentHP {get; private set; }
    public int maxMP {get; private set; }
    public int currentMP {get; private set; }

    //primitive stats from the characterData.Minimum 1 Maximum 10
    public int strength { get; private set; } //affect damage
    public int agility { get; private set; } //affect dodge chance
    public int precision { get; private set; } //affect critical hit chance 
    public int speed { get; private set; } // affect turn order
    public int luck { get; private set; } //affect all
    
    //temporary stats - buffs and debuffs
    public int damageAddBuff; //applpies to the final damage dealt
    public int shieldBuff; //reduces damage taken
    
    //secondary stats generated from primitive stats

    public CharacterState characterState;
    
    public Sprite fullBodySprite_Normal, fullBodySprite_Action, pfpSprite;
    //effects
    public GameObject placeHolder_fx;
    public string GetDescription(){
        return description;
    }
    //returns the party type of the parent PartyManager
    public PartyType GetPartyType(){
        return transform.parent.GetComponent<PartyManager>().partyType;
    }

    public BattleSkill GetRandomSkill(){
        return actions[UnityEngine.Random.Range(0, actions.Count)];
    }
    public BattleSkill GetRandomSkillByType(ActionType actionType){
        List<BattleSkill> attackActions = new List<BattleSkill>();
        foreach(BattleSkill action in actions){
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
