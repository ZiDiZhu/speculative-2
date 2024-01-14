using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance { get; private set; } //singleton
    public PartyUI partyUI;
    public PartyUI enemyUI;
    public ActionPanelUI actionPanelUI;
    public Button executeTurnBtn;
    

    public enum BattleSelectionState { ACTOR,ACTION,TARGET };
    public BattleSelectionState battleSelectionState;

    [SerializeField]private BattleSystem battleSystem;

    //Temporarily store the selected actor, action and target
    public MemberUI selectedActor;
    public ActionData selectedAction;
    public MemberUI selectedTarget;


    private void Awake()
    {
        if (instance == null) instance = this;
        battleSystem = BattleSystem.instance;
    }


    private void Start()
    {
        battleSystem = BattleSystem.instance;
        battleSelectionState = BattleSelectionState.ACTOR;
        executeTurnBtn.onClick.AddListener(ExecuteTurn);

        partyUI.SetParty(battleSystem.partyMembers);
        enemyUI.SetParty(battleSystem.enemies);
        actionPanelUI.SetActionPanelUI(battleSystem.partyMembers[0].actions);
    }

    public void TurnActionSelected()
    {
        battleSystem.AddCharacterAction(selectedActor.member, selectedAction, selectedTarget.member);
        selectedActor.hasSelectedAction = true;
        selectedActor.SetActionText(selectedAction.actionName + " on " + selectedTarget.member.characterName);
        partyUI.DeselectAll();
        selectedAction = null;
        selectedTarget = null;
        selectedActor = null;
        battleSelectionState = BattleSelectionState.ACTOR;
        actionPanelUI.ClearActionPanel();
        enemyUI.DisableSelection();
    
    }
    public void ExecuteTurn(){

        int enemyCount = battleSystem.enemies.Count;
        battleSystem.ExecuteTeamActions();
        string output = "Executed Actions.. You killed " + (battleSystem.enemies.Count - enemyCount)+" enemie(s); Enemies Turn..";
        Debug.Log(output);
        int partyCount = battleSystem.partyMembers.Count;
        partyUI.SetParty(battleSystem.partyMembers);
        enemyUI.SetParty(battleSystem.enemies);
        battleSystem.AddTurnActionsForAllCharacters(battleSystem.enemies, battleSystem.partyMembers);
        battleSystem.ExecuteTurnActions();
        output += "\nExecuted Actions.. You lost " + (partyCount - battleSystem.partyMembers.Count) + " party member(s); Your Turn..";
        actionPanelUI.actionDescription.text = output;

    }


}
