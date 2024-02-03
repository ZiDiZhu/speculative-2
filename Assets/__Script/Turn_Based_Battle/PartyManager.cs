using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PartyType { PLAYER, ENEMY };

public class PartyManager : MonoBehaviour
{

    [SerializeField]private PartyType partyType;
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sort the turnBattleActions list by actor speed
    //For automating battle logic (e.g. auto tournament / enemy AI)
    public void SortTurnBattleActionsBySpeed(){
        turnBattleActions.Sort((a, b) => a.actor.speed.CompareTo(b.actor.speed));
    }

    public PartyType GetPartyType(){
        return partyType;
    }

    public PartyType GetOppositePartyType(){
        if(partyType == PartyType.PLAYER){
            return PartyType.ENEMY;
        }
        else if(partyType == PartyType.ENEMY){
            return PartyType.PLAYER;
        }
        else{
            Debug.Log("Party Type Error");
            return PartyType.PLAYER;
        }
    }

    public void AddPartyMember(Character member)
    {
        partyMembers.Add(member);
    }

    public void RemovePartyMember(Character member)
    {
        partyMembers.Remove(member);
    }

    public void RemoveDeadMembers(){
        List<Character> deadPartyMembers = GetDeadPartyMembers();
        foreach(Character member in deadPartyMembers){
            RemovePartyMember(member);
        }
    }


    public List<Character> GetAllPartyMembers(){
        return partyMembers;
    }

    public int getPartySize(){
        return partyMembers.Count;
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
            Character target = opponentParty.GetRandomLivingCharacter();
            BattleAction battleAction = member.GetRandomBattleAction(ActionType.ATTACK);
            TurnBattleAction turnBattleAction = new TurnBattleAction(member, battleAction, target);
            turnBattleActions.Add(turnBattleAction);
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
