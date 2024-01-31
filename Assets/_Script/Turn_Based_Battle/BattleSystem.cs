using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{

    public static BattleSystem instance { get; private set; } //singleton
    public BattleState state;
    public List<Character> partyMembers = new List<Character>();
    public List<Character> enemies = new List<Character>();
    public int turnCount = 0;

    //____Action Delegate for turn actions___ First draft___ doesn't require action data
    public delegate void ActionDelegate(Character actor);
    public List<ActionDelegate> turnActions = new List<ActionDelegate>(); //list of actions to be performed. should be the same length as turnActionTargets and can have reapeated actions on different targets
    public List<Character> turnActionTargets = new List<Character>(); //sometimes the same action can be used on multiple targets


    //____Action Delegate for turn actions___ Second draft --- takes action data
    public delegate void ActionDelegate2(BattleAction action, Character target);
    public List<ActionDelegate2> turnActions2 = new List<ActionDelegate2>(); //list of actions to be performed. should be the same length as turnActionTargets and can have reapeated actions on different targets
    public List<Character> turnActionTargets2 = new List<Character>(); //sometimes the same action can be used on multiple targets
    public List<BattleAction> turnActionData = new List<BattleAction>(); //list of actions to be performed. should be the same length as turnActionTargets and can have reapeated actions on different targets


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


    //automatically run a battle until one side is defeated
    public void AutoTournament(){
        
        while(state != BattleState.WON && state != BattleState.LOST){
            
            if(state == BattleState.PLAYERTURN){
                AddTurnActionsForAllCharacters(partyMembers, enemies);
                ExecuteTurnActions();
                if (checkIfGameEnd()) break;
                state = BattleState.ENEMYTURN;

            }
            else if(state == BattleState.ENEMYTURN){
                AddTurnActionsForAllCharacters(enemies, partyMembers);
                ExecuteTurnActions();
                if (checkIfGameEnd()) break;    
                state = BattleState.PLAYERTURN;
                
            }
        }  
    }

    //returns true if the game has ended
    public bool checkIfGameEnd(){
        bool hasWon = true;
        foreach(Character enemy in enemies){
            if(enemy.characterState!=CharacterState.DEAD){
                hasWon = false;
                break;
            }
        }
        //if all enemies are dead, it's a win
        if (hasWon){
            Debug.Log("You win!");
            state = BattleState.WON;
            return true;
        }
        
        //if any party members are alive, it's end if the game.
        foreach(Character member in partyMembers){
            if(member.characterState!=CharacterState.DEAD){
                return false;
            }
        }

        Debug.Log("You lose!");
        state = BattleState.LOST;
        return true;
    }



    // Take a turn actions (from the turnActions list) for all members on one side of the battle
    // Checks if the target is dead after each action and removes them from the battle if they are
    public void ExecuteTurnActions(){
        for(int i = 0; i < turnActions.Count; i++){
            if(checkIfGameEnd()){
                break;
            }
            //default target    
            Character target = enemies[0];
            //target exceeds list length, default to first enemy in the list
            if (i<turnActionTargets.Count){
                target = turnActionTargets[i];
            }
            //do the thing
            turnActions[i](target);
            //check if target is dead
            if(target.GetCurrentHP() <=0){
                target.characterState = CharacterState.DEAD;
                Debug.Log(target.characterName + " has died");
                turnActionTargets.RemoveAll(x => x == target);
                //TODO: track dead characters 
            }
        }
        //clear the turnActions list
        turnActions.Clear();
        turnActionTargets.Clear();  
        turnCount++;
    }


    public void AddCharacterAction(Character actor, BattleAction actionData, Character target){
        if (actor == null){ return; }
        if (actor.characterState==CharacterState.DEAD){ return;}
        //Add delegate actioins to the turnActions list
        turnActions2.Add(new ActionDelegate2(actor.PerformAction));
        turnActionTargets2.Add(target);
        turnActionData.Add(actionData);
    }

    // calls the actions in the turnActions2 list
    public void ExecuteTeamActions(){
        for(int i = 0; i < turnActions2.Count; i++){
            if(checkIfGameEnd()){
                break;
            }
            //default target    
            Character target = enemies[0];
            //target exceeds list length, default to first enemy in the list
            if (i<turnActionTargets2.Count){
                target = turnActionTargets2[i];
            }
            //!-----------CALLS THE ACTIon------------------!
            turnActions2[i](turnActionData[i], target);
            //check if target is dead
            if(target.GetCurrentHP() <=0){
                target.characterState = CharacterState.DEAD;
                Debug.Log(target.GetCurrentHP() + " has died");
                turnActionTargets2.RemoveAll(x => x == target); //remove all actions where this dead character is the target
            }
        }
    }






    void AddBasicAttackAction(Character actor, ActionType actionType, Character target){
        if (actor == null){ return; }
        //Add delegate actioins to the turnActions list
        if (actionType == ActionType.ATTACK)
        {
            turnActions.Add(new ActionDelegate(actor.Attack));
            turnActionTargets.Add(target);
        }

    }


    // Add turn actions for all characters on one side of the battle
    // Currently the strat is to attack the weakest enemy
    public void AddTurnActionsForAllCharacters(List<Character> actors, List<Character> targets)
    {
        Character weakestTarget = GetWeakestCharacter(targets);
        foreach (Character actor in actors)
        {
            AddBasicAttackAction(actor, ActionType.ATTACK, weakestTarget);
        }
    }

    public Character GetWeakestCharacter(List<Character> characters)
    {
        int weakestIndex = 0;
        int lowestHP = characters[0].GetCurrentHP();
        for (int i = 1; i < characters.Count; i++)
        {
            if (characters[i].GetCurrentHP() < lowestHP)
            {
                weakestIndex = i;
                lowestHP = characters[i].GetCurrentHP();
            }
        }
        return characters[weakestIndex];
    }


}
