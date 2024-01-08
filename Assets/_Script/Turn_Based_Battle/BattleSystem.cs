using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public BattleState state;
    public Character[] partyMembers;
    public Character[] enemies;

    private int currentPartyMember;
    private int currentEnemy;   

    public delegate void ActionDelegate(Character target);
    public ActionDelegate[] turnActions;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        currentPartyMember = 0;

        turnActions = new ActionDelegate[partyMembers.Length];
        AddTurnAction();
        ExecuteTurnActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExecuteTurnActions(){
        foreach(ActionDelegate action in turnActions){
            action(enemies[currentEnemy]);
        }
    }

    public void AddTurnAction(){
        turnActions[currentPartyMember] = new ActionDelegate(partyMembers[currentPartyMember].Attack);
    }

    IEnumerator SetupBattle()
    {
        // Initialize your party members and enemies
        // Set up UI elements
        // Transition to the player's turn
        yield return new WaitForSeconds(2f); // Add delay for animations or transitions
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public void PlayerTurn(){
        // Display UI for the player to select an action
        // Display UI for the player to select a target
        // Transition to the enemy's turn
        Debug.Log("Player's Turn");
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void EnemyTurn(){
        // Select a random action
        // Select a random target
        // Perform the action
        // Transition to the player's turn
        Debug.Log("Enemy's Turn");
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

}
