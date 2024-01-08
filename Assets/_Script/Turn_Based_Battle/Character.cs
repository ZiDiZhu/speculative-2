using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public string characterName;
    public int maxHP;
    public int currentHP;
    public int maxMP;   
    public int currentMP;   
    public int strength;
    public int defense;
    public int magic;   
    public int magicDefense;    
    public int agility; 
    public int luck; 

    
    // Start is called before the first frame update
    void Start()
    {
        
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
        magic = 10;
        magicDefense = 10;
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
        magic = mag;
        magicDefense = mdef;
        agility = agi;
        luck = luk;
    }


    public void Attack(Character target){

        string output = characterName + " attacks " + target.characterName+". ";

        int damage = strength * 10 ;
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
            if (damage < 0)
            {
                damage = 0;
            }
        }
        target.currentHP -= damage;
        output += target.characterName + " took " + damage + " damage. HP:" +target.currentHP +"/"+target.maxHP+". ";
        if (target.currentHP <= 0)
        {
            output += target.characterName + " has been defeated.";
        }
        Debug.Log(output);
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
