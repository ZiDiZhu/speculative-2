//for run time action queue
//wrote it as a struct because it's easier to see in the inspector
[System.Serializable]
public struct TurnBattleAction
{
    public Character actor;
    public BattleAction battleAction;
    public Character target;

    public TurnBattleAction(Character actor, BattleAction battleAction, Character target)
    {
        this.actor = actor;
        this.battleAction = battleAction;
        this.target = target;
    }
}