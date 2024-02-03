//for queuing up and executing actioins at runtime
// instantiated in the battle manager / battleUI
//wrote it as a struct because it's easier to see in the inspector
[System.Serializable]
public struct TurnBattleAction
{
    //takes in these parameters
    public Character actor;
    public BattleAction battleAction;
    public Character target;


    //to generate these values
    public int damage;
    public float hitChance; //0 to 1. generate before battle, during the battle it uses this value to compare it to compare to a random value to determine hit or miss
    public float critChance; //0 to 1
    public int critHitAddDamage; //additional damage if the attack is a critical hit

    

    public TurnBattleAction(Character actor, BattleAction battleAction, Character target)
    {

        this.actor = actor;
        this.battleAction = battleAction;
        this.target = target;
        
        damage = actor.strength * 10 + battleAction.addDamage;
        damage *= (int)battleAction.multiplyDamage;
        hitChance = 1 - target.agility*0.1f + (actor.precision + actor.agility)* 0.05f - target.luck*0.01f;
        critChance = (actor.precision + actor.luck)*0.25f - target.luck*0.25f;
        critHitAddDamage = (actor.luck + actor.strength)*2;
    }

    public string Execute(){
        string output = actor.characterName + " uses " + battleAction.actionName + " on " + target.characterName + ". ";
        switch (battleAction.actionType)
        {
            case ActionType.ATTACK:
                output = ExecuteAttack();
                break;
            case ActionType.HEAL:
                output = ExecuteHeal();
                break;
            default:    
                break;
        }
        return output;
    }

    public string ExecuteAttack(){
        string output = actor.characterName + " attacks " + target.characterName+". ";
        int hitRoll = UnityEngine.Random.Range(0, 100);
        if (hitRoll< hitChance*100) //hit
        {
            int critHitRoll= UnityEngine.Random.Range(0, 100);
            if (critHitRoll < critChance*100) //crit hit
            {
                damage += critHitAddDamage;
                output += "Critical Hit! ";
            }
            if (damage < 0)
            {
                damage = 0;
            }
            target.TakeDamage(damage);
        }
        else //miss
        {
            output += "Missed!";
        }
        output += target.characterName + " takes " + damage + " damage. ";
        return output;
    }

    public string ExecuteHeal(){
        string output = actor.characterName + " heals " + target.characterName+". ";
        target.Heal(battleAction.addHealing);
        output += target.characterName + " heals " + battleAction.addHealing + " HP. ";
        return output;
    }

}