using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{

    public static BattleSystem instance { get; private set; } //singleton
    public BattleState state;
    public ActionType actionType;
    public List<Character> partyMembers = new List<Character>();
    public List<Character> enemies = new List<Character>();
    public int turnCount = 0;

    //____Action Delegate for turn actions___ First draft
    public delegate void ActionDelegate(Character actor);
    public List<ActionDelegate> turnActions = new List<ActionDelegate>(); //list of actions to be performed. should be the same length as turnActionTargets and can have reapeated actions on different targets
    public List<Character> turnActionTargets = new List<Character>(); //sometimes the same action can be used on multiple targets


    public Character selectedMember; //the member that is currently selected by the player

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

        //AutoTournament();
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
    bool checkIfGameEnd(){
        if(enemies.Count==0){
            Debug.Log("You win!");
            state = BattleState.WON;
            return true;
        }
        if(partyMembers.Count==0){
            Debug.Log("You lose!");
            state = BattleState.LOST;
            return true;
        }
        return false;
    }


    // Take a turn actions (from the turnActions list) for all members on one side of the battle
    // Checks if the target is dead after each action and removes them from the battle if they are
    void ExecuteTurnActions(){
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
            if(target.currentHP<=0){
                target.characterState = CharacterState.DEAD;
                Debug.Log(target.characterName + " has died");
                enemies.Remove(target);
                partyMembers.Remove(target);
                turnActionTargets.RemoveAll(x => x == target);
                //TODO: track dead characters 
                //Destroy(target);
            }
        }
        //clear the turnActions list
        turnActions.Clear();
        turnActionTargets.Clear();  
        turnCount++;
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
        int lowestHP = characters[0].currentHP;
        for (int i = 1; i < characters.Count; i++)
        {
            if (characters[i].currentHP < lowestHP)
            {
                weakestIndex = i;
                lowestHP = characters[i].currentHP;
            }
        }
        return characters[weakestIndex];
    }


}
