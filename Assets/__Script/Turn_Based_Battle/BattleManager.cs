using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST, TIE}
public delegate void BattleActionDelegate(Character actor, BattleSkill action, Character target);

public class BattleManager: MonoBehaviour
{
    
    public static BattleManager instance { get; private set; } //singleton
    [SerializeField] public BattleState battleState { get; private set; }
    [SerializeField] private PartyManager playerParty;
    [SerializeField] private PartyManager enemyParty;
   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Returns the respective party manager
    public PartyManager GetPartyManager(PartyType type){
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

    //check for end game conditions. return true if the game reached an end currentState
    public bool IfBattleEnded(){
        if(playerParty.GetAlivePartyMembers().Count == 0 && enemyParty.GetAlivePartyMembers().Count == 0) battleState = BattleState.LOST;
        else if (enemyParty.GetAlivePartyMembers().Count == 0) battleState = BattleState.WON;
        else if (playerParty.GetAlivePartyMembers().Count == 0) battleState = BattleState.LOST;
        if(battleState == BattleState.WON || battleState == BattleState.LOST || battleState == BattleState.TIE){
            Debug.Log("Battle Ended");
            return true;
        }
        return false;
    }



}
