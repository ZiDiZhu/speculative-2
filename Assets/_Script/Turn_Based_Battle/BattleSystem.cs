using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum ActionType { ATTACK, MAGIC, ITEM, DEFEND, RUN }
public class BattleSystem : MonoBehaviour
{

    public BattleState state;
    public ActionType actionType;
    public List<Character> partyMembers = new List<Character>();
    public List<Character> enemies = new List<Character>();

    public int turnCount = 0;

    public delegate void ActionDelegate(Character actor);
    public List<ActionDelegate> turnActions = new List<ActionDelegate>(); //list of actions to be performed. should be the same length as turnActionTargets and can have reapeated actions on different targets
    public List<Character> turnActionTargets = new List<Character>(); //sometimes the same action can be used on multiple targets
    

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.PLAYERTURN;

        TestAddTurnAction();
        ExecuteTurnActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExecuteTurnActions(){

        for(int i = 0; i < turnActions.Count; i++){
            Character target = enemies[0];

            //target exceeds list length, default to first enemy in the list
            if (i<turnActionTargets.Count){
                target = turnActionTargets[i];
            }

            //do the thing
            turnActions[i](target);
            
            //check if target is dead
            if(target.currentHP<=0){
                Debug.Log(target.characterName + " has died");
                enemies.Remove(target);
                partyMembers.Remove(target);
                turnActionTargets.RemoveAll(x => x == target);
            }

            //check if all enemies are dead
            if(enemies.Count==0){
                Debug.Log("You win!");
                state = BattleState.WON;
                turnActions.Clear();
                turnActionTargets.Clear();
                turnCount++;
                return;
            }

            //check if all party members are dead
            if(partyMembers.Count==0){
                Debug.Log("You lose!");
                state = BattleState.LOST;
                return;
            }
            
        }

        turnActions.Clear();
        turnActionTargets.Clear();  
        turnCount++;
    }

    void AddTurnAction(Character actor, ActionType actionType, Character target){
        
        if(actionType==ActionType.ATTACK){
            turnActions.Add(new ActionDelegate(actor.Attack));
            turnActionTargets.Add(target);
        }
    }

    public void TestAddTurnAction(){
        AddTurnAction(partyMembers[0], ActionType.ATTACK, enemies[0]);
        AddTurnAction(partyMembers[1], ActionType.ATTACK, enemies[0]);
    }




}
