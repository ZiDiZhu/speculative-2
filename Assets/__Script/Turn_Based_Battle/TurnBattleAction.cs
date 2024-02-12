//for queuing up and executing actioins at runtime
// instantiated in the battle manager / battleUI
//wrote it as a struct because it's easier to see in the inspector
[System.Serializable]
public struct TurnBattleAction
{
    //takes in these parameters
    public Character actor;
    public BattleSkill battleAction;
    public Character target;

    //Generated at creation
    public int damage;
    public float hitChance; //0 to 1. generate before battle, during the battle it uses this value to compare it to compare to a random value to determine hit or miss
    public float critChance; //0 to 1
    public int critHitAddDamage; //additional damage if the attack is a critical hit

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
    public int targetHpChange;
    public int targetMpChange;

    public TurnBattleAction(Character actor, BattleSkill battleAction, Character target)
    {
        this.actor = actor;
        this.battleAction = battleAction;
        this.target = target;
        
        damage = actor.strength * 10 + battleAction.addDamage;
        damage *= (int)battleAction.multiplyDamage;
        hitChance = 1 - target.agility*0.1f + (actor.precision + actor.agility)* 0.05f - target.luck*0.01f;
        critChance = (actor.precision + actor.luck)*0.25f - target.luck*0.25f;
        critHitAddDamage = (actor.luck + actor.strength)*2;

        actionInvalidReason = ACTION_INVALID_REASON.NULL;
        actionResult = ACTION_RESULT.WAITING;
        actorHpChange = 0;
        actorMpChange = 0;
        targetHpChange = 0;
        targetMpChange = 0;
    }

    public string Execute(){
        string output = actor.characterName + " uses " + battleAction.actionName + " on " + target.characterName + ". \n";
        switch (battleAction.actionType)
        {
            case ActionType.ATTACK:
                output += ExecuteAttack();
                break;
            case ActionType.HEAL:
                output += ExecuteHeal();
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
        if (target == null) 
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
                if (actor != target) 
                {
                    actionInvalidReason = ACTION_INVALID_REASON.WRONG_TARGET_TYPE;
                    return false;
                }
                break;
            case TargetType.ALL_ALLY:
            case TargetType.SINGLE_ALLY:
                if (actor.GetPartyType() != target.GetPartyType()) 
                {
                    actionInvalidReason = ACTION_INVALID_REASON.WRONG_TARGET_TYPE;
                    return false;
                }
                break;
        }
        return true;
    }


    public string ExecuteAttack(){
        string output = "";
        int hitRoll = UnityEngine.Random.Range(0, 100);
        if (hitRoll< hitChance*100) //hit
        {
            int critHitRoll= UnityEngine.Random.Range(0, 100);
            if (critHitRoll < critChance*100) //crit hit
            {
                damage += critHitAddDamage;
                output += " Critical Hit! ";
            }
            if (damage < 0)
            {
                damage = 0;
            }
            damage += actor.damageAddBuff;
            damage -= target.shieldBuff;
            target.TakeDamage(damage);
        }
        else //miss
        {
            output += " Missed!";
        }
        output += target.characterName + " takes " + damage + " damage. ";
        return output;
    }

    public string ExecuteHeal(){
        string output = "";
        target.Heal(battleAction.addHealing);
        output += target.characterName + " heals " + battleAction.addHealing + " HP. ";
        return output;
    }

}