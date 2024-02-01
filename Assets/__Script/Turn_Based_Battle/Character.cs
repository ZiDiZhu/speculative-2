using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState { ALIVE, DEAD }

//Its parent object is BattlePartyManager.
public class Character : MonoBehaviour
{
    //visible basic stats
    public string characterName;
    [SerializeField][TextArea(15,20)] private string description;
    [SerializeField]private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int maxMP;
    [SerializeField] private int currentMP;   
    
    //primitive stats from the characterData.Minimum 1 Maximum 10
    public int strength; //affect damage
    public int agility; //affect dodge chance
    public int precision; //affect critical hit chance  
    public int speed; // affect turn order
    public int luck; //affect all
    
    //equipment stats. not inherent to the character
    public int defense;
    
    //secondary stats generated from primitive stats
    
    
    

    //battle stat generated from basic stats - reset after each action
    public int damage;

    public CharacterState characterState;
    public List<BattleAction> actions = new List<BattleAction>();
    public Sprite fullBodySprite, pfpSprite;

    //Character-specific sounds
    public AudioClip placeHolder_sfx;


    // Start is called before the first frame update
    void Start()
    {
        damage = strength * 10;
    }

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

    //returns the party type of the parent BattlePartyManager
    public PartyType GetPartyType(){
        return transform.parent.GetComponent<BattlePartyManager>().GetPartyType();
    }

    public PartyType GetOppositePartyType(){
        return transform.parent.GetComponent<BattlePartyManager>().GetOppositePartyType();
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

    //TO be replaced
    //handles actionData and performs the appropriate action
    public string PerformAction(BattleAction action, Character target){
        string output = characterName + " performs " + action.actionName + " on " + target.characterName + ". ";
        currentMP -= action.mpCost;

        switch (action.actionType)
        {
            case ActionType.ATTACK:
                damage += (int)(damage * action.multiplyDamage);
                damage += action.addDamage;
                output += Attack(target);
                //Heal(action.addHealing);
                damage = strength * 10; //reset damage
                break;
            case ActionType.HEAL:
                //Heal(action.power);
                break;
            case ActionType.ITEM:
                break;
            case ActionType.DEFEND:
                break;
            case ActionType.FLEE:
                break;
            default:
                break;
        }

        return output;
    }

    //basic from of attack that doesn't require an actionData
    //returns the output of the attack
    public string Attack(Character target){

        damage = strength * 10; //reset damage

        string output = characterName + " attacks " + target.characterName+". ";
        //TODO: Improve calculation logic
        int criticalHitRoll = UnityEngine.Random.Range(0,100);
        if(criticalHitRoll < luck*strength){ // Critical Hit - luck, strength 
            damage *= 2;
            output += "Critical Hit! ";
        }
        if(damage < 1&&target.currentHP>1){
            damage = 1;
        }
        int random2 = UnityEngine.Random.Range(0, 150);
        if (random2 < target.luck*target.agility)
        {
            damage = 0;
            output+= target.characterName + " dodged the attack!";
        }
        else
        {
            damage -= target.defense;
            target.currentHP -= damage;
            if (damage < 0)
            {
                damage = 0;
            }
        }
       
        output += target.characterName + " took " + damage + " damage. HP:" +target.currentHP +"/"+target.maxHP+". ";
        if (target.currentHP <= 0)
        {
            output += target.characterName + " has been defeated.";
            target.characterState = CharacterState.DEAD;
        }
        Debug.Log(output);
        damage = strength * 10; //reset damage
        return output;
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
