using System.Collections.Generic;
using UnityEngine;
using static BattleSkill;
using static Character;

public class PartyManager : MonoBehaviour
{
    public enum PartyType { PLAYER, ENEMY };

    public PartyType partyType { get; private set; }
    [SerializeField]private List<Character> partyMembers = new List<Character>(); //including dead members
    public List<TurnBattleAction> turnBattleActions = new List<TurnBattleAction>(); //list of turn actions for each party

    // Start is called before the first frame update

    private void Awake()
    {
        partyMembers.Clear();
        foreach (Character member in GetComponentsInChildren<Character>())
        {
            AddPartyMember(member);
        }

        turnBattleActions.Clear();
        
    }

    //Sort the turnBattleActions list by actor speed
    //For automating battle logic (e.g. auto tournament / enemy AI)
    public void SortTurnBattleActionsBySpeed(){
        turnBattleActions.Sort((a, b) => a.actor.speed.CompareTo(b.actor.speed));
    }

    public void AddPartyMember(Character member)
    {
        partyMembers.Add(member);
    }

    public void RemovePartyMember(Character member)
    {
        partyMembers.Remove(member);
    }


    public List<Character> GetAllPartyMembers(){
        return partyMembers;
    }

    public Character GetRandomLivingCharacter(){
        List<Character> alivePartyMembers = GetAlivePartyMembers();
        int randomIndex = Random.Range(0, alivePartyMembers.Count);
        return alivePartyMembers[randomIndex];
    }
    
    public List<Character> GetAlivePartyMembers(){
        List<Character> alivePartyMembers = new List<Character>();
        foreach(Character member in partyMembers){
            if(member.characterState == CharacterState.ALIVE){
                alivePartyMembers.Add(member);
            }
        }
        return alivePartyMembers;
    }

    public List<Character> GetDeadPartyMembers(){
        List<Character> deadPartyMembers = new List<Character>();
        foreach(Character member in partyMembers){
            if(member.characterState == CharacterState.DEAD){
                deadPartyMembers.Add(member);
            }
        }
        return deadPartyMembers;
    }
    
    //For each living character in the party, perform a random action on a random living character in the enemy party
    //Takes anthor party as a parameter to determine the target of the action
    public void AddActionsForAllCharacters(PartyManager opponentParty){
        foreach(Character member in GetAlivePartyMembers()){
            BattleSkill battleAction = member.GetRandomSkill();
            List<Character> targets = new List<Character>();
            switch (battleAction.targetType)
            {
                case TargetType.SINGLE_OPPONENT:
                    targets.Add(opponentParty.GetRandomLivingCharacter());
                    TurnBattleAction turnAction = new TurnBattleAction(member, battleAction, targets);
                    turnBattleActions.Add(turnAction);
                    break;
                case TargetType.ALL_OPPONENT:
                    TurnBattleAction turnBattleActionAll = new TurnBattleAction(member, battleAction, opponentParty.GetAlivePartyMembers());
                    turnBattleActions.Add(turnBattleActionAll);
                    break;
                case TargetType.SELF:
                    targets.Add(member);
                    TurnBattleAction turnBattleActionSelf = new TurnBattleAction(member, battleAction, targets);
                    turnBattleActions.Add(turnBattleActionSelf);
                    break;
                case TargetType.SINGLE_ALLY:
                    targets.Add(GetRandomLivingCharacter());
                    TurnBattleAction turnBattleActionAlly = new TurnBattleAction(member, battleAction, targets);
                    turnBattleActions.Add(turnBattleActionAlly);
                    break;
                case TargetType.ALL_ALLY:   
                    TurnBattleAction turnBattleActionAllAlly = new TurnBattleAction(member, battleAction, GetAlivePartyMembers());
                    turnBattleActions.Add(turnBattleActionAllAlly);
                    break;
                default:
                    break;
            }
        }   
    }

    //delete previously queued actions for the actor when you change the action or target
    public void DeleteTurnBattleActionsForActor(Character actor){

        List<TurnBattleAction> turnBattleActionsToDelete = new List<TurnBattleAction>();
        foreach(TurnBattleAction turnBattleAction in turnBattleActions){
            if(turnBattleAction.actor == actor){
                turnBattleActionsToDelete.Add(turnBattleAction);
            }
        }
        foreach(TurnBattleAction turnBattleAction in turnBattleActionsToDelete){
            turnBattleActions.Remove(turnBattleAction);
        }
        
    }

    
}
