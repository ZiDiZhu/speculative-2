using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST, TIE}
public delegate void BattleActionDelegate(Character actor, BattleAction action, Character target);

public class BattleManager: MonoBehaviour
{
    
    public static BattleManager instance { get; private set; } //singleton
    [SerializeField]private BattleState battleState;
    [SerializeField] private BattlePartyManager playerParty;
    [SerializeField] private BattlePartyManager enemyParty;
   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public BattleState GetBattleState()
    {
        return battleState;
    }
    public void SetBattleState(BattleState state)
    {
        battleState = state;
    }


    public BattlePartyManager GetPartyManager(PartyType type){
        if(type == PartyType.PLAYER){
            return playerParty;
        }
        else if(type == PartyType.ENEMY){
            return enemyParty;
        }
        else{
            Debug.Log("Party Type Error");
            return null;
        }
    }

    public void AddTurnBattleAction(PartyType partyType, TurnBattleAction turnBattleAction)
    {
        if (partyType == PartyType.PLAYER)
        {
            playerParty.turnBattleActions.Add(turnBattleAction);
        }
        else if (partyType == PartyType.ENEMY)
        {
            enemyParty.turnBattleActions.Add(turnBattleAction);
        }
        else
        {
            Debug.Log("Party Type Error");
        }
    }

    // Execute the turn actions in the turnBattleActions lists from player and enemy parties
    // Compare the speed of the first action in each list and try execute the faster one
    public void ExecuteTurn()
    {
        enemyParty.SortTurnBattleActionsBySpeed();  

        List<TurnBattleAction> playerActions = playerParty.turnBattleActions;
        List<TurnBattleAction> enemyActions = enemyParty.turnBattleActions;

        while((playerActions.Count > 0 || enemyActions.Count > 0) && !IfBattleEnded())
        {
            //if both parties have actions, compare the speed of the first action in each list
            if (playerActions.Count > 0 && enemyActions.Count > 0)
            {
                if (playerActions[0].actor.speed >= enemyActions[0].actor.speed)
                {
                    TestExecuteTurnBattleAction(playerActions[0]);
                    playerActions.RemoveAt(0);
                }
                else
                {
                    TestExecuteTurnBattleAction(enemyActions[0]);
                    enemyActions.RemoveAt(0);
                }
            }else{
                //execute the action of the party with actions
                if (playerActions.Count > 0)
                {
                    TestExecuteTurnBattleAction(playerActions[0]);
                    playerActions.RemoveAt(0);
                }
                else if (enemyActions.Count > 0)
                {
                    TestExecuteTurnBattleAction(enemyActions[0]);
                    enemyActions.RemoveAt(0);
                }   
            }
            
        }       
    }
    

    //this checks if thea action can be executed, and if so do it
    //if not, return false and recusively call itself until it finds an action that can be executed
    public bool TestExecuteTurnBattleAction(TurnBattleAction turnBattleAction){

        BattleAction battleAction = turnBattleAction.battleAction;
        Character actor = turnBattleAction.actor;
        Character target = turnBattleAction.target;

        //check invalid - return false if the action cannot be executed
        if (target == null) //target is null - should not happen normally 
        {
            Debug.Log(actor.characterName + " can't Execute Action - Target is null!");
            return false;
        }
        
        if (actor.characterState == CharacterState.DEAD) //target is dead
        {
            Debug.Log(actor.characterName + " is Dead and can't Execute Action.");
            return false;
        }
        if (battleAction.mpCost > actor.GetCurrentMP()) //not enough mp - should not happen normally cause UI will prevent the action from being selectable
        {
            Debug.Log(actor.characterName + " can't Execute Action - Not enough MP");
            //TODO; maybe give player 3 seconds to change action?
            return false;
        }
        if (target.characterState==CharacterState.DEAD){
            Debug.Log(actor.characterName + " can't Execute " + battleAction.actionName + " - Target"+ target.characterName +" is dead. ");
            return false;
        }

        //execute the action

        switch(turnBattleAction.battleAction.actionType){
            case ActionType.ATTACK:
                actor.PerformAction(battleAction, target);
                break;
            default:
                Debug.Log("Action Type Error");
                break;
        }
        
        return true;
    }

    
    

    //check for end game conditions. return true if the game reached an end state
    public bool IfBattleEnded(){
        if(playerParty.GetAlivePartyMembers().Count == 0 && enemyParty.GetAlivePartyMembers().Count == 0) SetBattleState(BattleState.TIE);
        else if (enemyParty.GetAlivePartyMembers().Count == 0) SetBattleState(BattleState.WON);
        else if (playerParty.GetAlivePartyMembers().Count == 0) SetBattleState(BattleState.LOST);
        if(battleState == BattleState.WON || battleState == BattleState.LOST || battleState == BattleState.TIE){
            Debug.Log("Battle Ended");
            return true;
        }
        return false;
    }



}
