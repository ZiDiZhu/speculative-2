//for queuing up and executing actioins at runtime
// instantiated in the battle manager / battleUI
//wrote it as a struct because it's easier to see in the inspector

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnBattleAction
{
    //takes in these parameters
    public Character actor;
    public BattleSkill battleAction;
    public List<Character> targets = new List<Character>();

    //Generated at creation
    
    [SerializeField]private float hitChance; //0 to 1. generate before battle, during the battle it uses this value to compare it to compare to a random value to determine hit or miss
    [SerializeField]private float critChance; //0 to 1
    [SerializeField]private int critHitAddDamage; //additional damage if the attack is a critical hit

    public enum ACTION_INVALID_REASON //no cost, no effect
    {
        NULL,
        NULL_ACTOR,
        NULL_TARGET,
        NOT_ENOUGH_MP,
        ACTOR_DEAD,
        WRONG_TARGET_TYPE
    }
    
    public enum ACTION_RESULT 
    {
        WAITING,
        INVALID,
        MISS,
        HIT,
        CRIT
    }

    public ACTION_INVALID_REASON actionInvalidReason;
    public ACTION_RESULT actionResult;

    //Generated after executed
    public int actorHpChange;
    public int actorMpChange;

    public List<int> targetHpChange = new List<int>();
    public List<int> targetMpChange;



    public TurnBattleAction(Character actor, BattleSkill battleAction, List<Character> targets)
    {
        this.actor = actor;
        this.battleAction = battleAction;

        foreach(Character target in targets){
            this.targets.Add(target);
        }

        int numTargets = targets.Count;
        
        
        foreach(Character target in targets){
            targetHpChange.Add(actor.strength * 10 + battleAction.addDamage);
            hitChance += (1 - target.agility*0.1f + (actor.precision + actor.agility)* 0.05f - target.luck*0.01f);
            critChance += ((actor.precision + actor.luck) * 0.25f - target.luck * 0.25f);
        }

        hitChance /= numTargets;
        critChance /= numTargets;
        critHitAddDamage = (actor.luck + actor.strength)*2;

        actionInvalidReason = ACTION_INVALID_REASON.NULL;
        actionResult = ACTION_RESULT.WAITING;
    }

    public string Execute(){
        string output = actor.characterName + " tries to use " + battleAction.actionName;
        bool valid = IsActionValid();
        if (!valid)
        {
            output = output + "Invalid action: " + actionInvalidReason.ToString();
            return output;
        }

        switch (battleAction.actionType)
        {
            case ActionType.ATTACK:
                foreach(Character target in targets){
                    output = output+ ExecuteAttack(target) + "\n";
                }
                break;
            case ActionType.HEAL:
                foreach(Character target in targets){
                    output = output + ExecuteHeal(target);
                }
                
                break;
            default:    
                break;
        }
        return output;
    }

    public bool IsActionValid(){
        if (actor == null) 
        {
            actionInvalidReason = ACTION_INVALID_REASON.NULL_ACTOR;
            return false;
        }
        if (targets == null) 
        {
            actionInvalidReason = ACTION_INVALID_REASON.NULL_TARGET;
            return false;
        }
        if (actor.characterState == CharacterState.DEAD) 
        {
            actionInvalidReason = ACTION_INVALID_REASON.ACTOR_DEAD;
            return false;
        }
        if (battleAction.mpCost > actor.GetCurrentMP()) 
        {
            actionInvalidReason = ACTION_INVALID_REASON.NOT_ENOUGH_MP;
            return false;
        }

        switch (battleAction.targetType){
            case TargetType.ALL_OPPONENT:
            case TargetType.SINGLE_OPPONENT:
                if (actor.GetPartyType() == targets[0].GetPartyType()) 
                {
                    actionInvalidReason = ACTION_INVALID_REASON.WRONG_TARGET_TYPE;
                    return false;
                }
                break;
            case TargetType.ALL_ALLY:
            case TargetType.SINGLE_ALLY:
                if (actor.GetPartyType() != targets[0].GetPartyType()) 
                {
                    actionInvalidReason = ACTION_INVALID_REASON.WRONG_TARGET_TYPE;
                    return false;
                }
                break;
        }
        return true;
    }


    public string ExecuteAttack(Character target){
        string output = "";
        int hitRoll = UnityEngine.Random.Range(0, 100);
        int dealtDamage = targetHpChange[targets.IndexOf(target)];
        if (hitRoll< hitChance*100) //hit
        {
            int critHitRoll= UnityEngine.Random.Range(0, 100);
            if (critHitRoll < critChance*100) //crit hit
            {
                dealtDamage += critHitAddDamage;
                output += " Critical Hit! ";
            }
            if (dealtDamage < 0)
            {
                dealtDamage = 0;
            }
            dealtDamage += actor.damageAddBuff;
            dealtDamage -= target.shieldBuff;
            target.TakeDamage(dealtDamage);
        }
        else //miss
        {
            output += " Missed!";
            dealtDamage = 0;
        }
        output += target.characterName + " takes " + dealtDamage + " damage. ";
        targetHpChange[targets.IndexOf(target)] = dealtDamage;
        return output;
    }

    public string ExecuteHeal(Character target){
        string output = "";
        target.Heal(battleAction.addHealing);
        output += target.characterName + " heals " + battleAction.addHealing + " HP. ";
        return output;
    }

}