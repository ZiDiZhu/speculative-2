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
    [SerializeField]private BattleState battleState;
    [SerializeField] private PartyManager playerParty;
    [SerializeField] private PartyManager enemyParty;
   

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

    //check for end game conditions. return true if the game reached an end state
    public bool IfBattleEnded(){
        if(playerParty.GetAlivePartyMembers().Count == 0 && enemyParty.GetAlivePartyMembers().Count == 0) SetBattleState(BattleState.LOST);
        else if (enemyParty.GetAlivePartyMembers().Count == 0) SetBattleState(BattleState.WON);
        else if (playerParty.GetAlivePartyMembers().Count == 0) SetBattleState(BattleState.LOST);
        if(battleState == BattleState.WON || battleState == BattleState.LOST || battleState == BattleState.TIE){
            Debug.Log("Battle Ended");
            return true;
        }
        return false;
    }



}
