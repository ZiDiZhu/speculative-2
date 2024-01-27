using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { PARTYMEMBER, ENEMY, NPC }
public enum CharacterState { NORMAL, DEAD, UNDEAD }

public class Character : MonoBehaviour
{
    //visible basic stats
    public string characterName;
    [SerializeField][TextArea(15,20)] public string description;
    public int maxHP;
    public int currentHP;
    public int maxMP;   
    public int currentMP;   
    
    //basic stats from the characterData.Minimum 1 Maximum 10
    public int strength;    
    public int precision;
    public int agility; 
    public int luck;
    
    //equipment stats. not inherent to the character
    public int defense;

    public AudioClip placeHolder_sfx;
    
    //temporary stats - reset after each action
    public int damage;


    public CharacterType characterType;
    public CharacterState characterState;
    public List<ActionData> actions = new List<ActionData>();
    public Sprite fullBodySprite, pfpSprite;


    // Start is called before the first frame update
    void Start()
    {
        damage = strength * 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Character(){
        characterName = "Default";
        maxHP = 10;
        currentHP = 10;
        maxMP = 10;
        currentMP = 10;
        strength = 10;
        defense = 10;
        agility = 10;
        luck = 10;
    }   

    public Character(string name, int hp, int mp, int str, int def, int mag, int mdef, int agi, int luk){
        characterName = name;
        maxHP = hp;
        currentHP = hp;
        maxMP = mp;
        currentMP = mp;
        strength = str;
        defense = def;
        agility = agi;
        luck = luk;
    }

    public Character(CharacterData data){

    }

    
    //handles actionData and performs the appropriate action
    public void PerformAction(ActionData action, Character target){

        currentMP -= action.mpCost;

        switch (action.actionType)
        {
            case ActionType.ATTACK:
                damage += (int)(damage * action.multiplyDamage);
                damage += action.addDamage;
                Attack(target);
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
    }

    //basic from of attack that doesn't require an actionData
    public void Attack(Character target){

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
        }
        Debug.Log(output);

        damage = strength * 10; //reset damage
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
