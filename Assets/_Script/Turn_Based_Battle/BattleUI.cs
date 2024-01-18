using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance { get; private set; } //singleton
    public PartyUI partyUI;
    public PartyUI enemyUI;


    [Header("Action Panel")]
    public GameObject actionUIPrefab;
    public GameObject actionUIParent;
    public TMP_Text actionDescription;
    List<ActionUI> actionUIs = new List<ActionUI>();
    public Button executeTurnBtn;
    public SpriteRenderer charaSpriteRenderer;    
    private TMP_Text battleStateText;
    
    [Header("Run-Time")]
    //Temporarily store the selected actor, action and target
    public MemberUI selectedActor;
    public ActionData selectedAction;
    public MemberUI selectedTarget;

    private BattleSystem battleSystem;
    public enum BattleSelectionState { ACTOR, ACTION, TARGET };
    public BattleSelectionState battleSelectionState;


    private void Awake()
    {
        if (instance == null) instance = this;
        battleStateText = executeTurnBtn.GetComponentInChildren<TMP_Text>();
    }


    private void Start()
    {
        battleSystem = BattleSystem.instance;
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        executeTurnBtn.onClick.AddListener(ExecuteTurn); 

        partyUI.SetParty(battleSystem.partyMembers);
        enemyUI.SetParty(battleSystem.enemies);
        charaSpriteRenderer.sprite = null;
        ClearActionPanel();
        executeTurnBtn.interactable = false;
    }

    public void SetBattleSelectionState(BattleSelectionState state){
        battleSelectionState = state;
        battleStateText.text = "SELECT "+ battleSelectionState.ToString();

        switch(battleSelectionState){
            case (BattleSelectionState.ACTOR):
                enemyUI.DisableSelection();
                break;
            case (BattleSelectionState.ACTION):
                break;
            case (BattleSelectionState.TARGET):
                enemyUI.EnableSelection();
                break;
        }
    }
    
    
    public void SetActionPanel(List<ActionData> actions)
    {
        ClearActionPanel();
        foreach (ActionData action in actions)
        {
            GameObject actionUIObject = Instantiate(actionUIPrefab, actionUIParent.transform);
            ActionUI actionUI = actionUIObject.GetComponent<ActionUI>();
            actionUI.SetAction(action);
            actionUI.GetComponent<Button>().onClick.AddListener(actionUI.OnClick);
            actionUIs.Add(actionUI);
        }
    }

    public void ClearActionPanel(){
        foreach (Transform child in actionUIParent.transform)
        {
            Destroy(child.gameObject);
        }
        actionUIs.Clear();
        actionDescription.text = "";
    }


    //when a member is selected
    public void MemberUIOnClick(MemberUI memberUI){
        
        switch(battleSelectionState){
            case (BattleSelectionState.TARGET): //this member is selected as target
                selectedTarget = memberUI;  
                TargetSelected();
                break;
            default:
                partyUI.DeselectAll(); 
                memberUI.Select();
                selectedActor = memberUI;
                battleSystem.selectedMember = memberUI.member;
                battleSelectionState = BattleSelectionState.ACTION;
                SetActionPanel(memberUI.member.actions);
                charaSpriteRenderer.sprite = memberUI.member.fullBodySprite;
                SetBattleSelectionState(BattleSelectionState.ACTION);
                executeTurnBtn.interactable = false;
                break;  
        }
        
    }

    public void EnemyMemberOnClick(MemberUI enemyUI){
        switch(battleSelectionState){
            case (BattleSelectionState.TARGET): //this member is selected as target
                selectedTarget = enemyUI;  
                TargetSelected();
                break;
            default:
                //display enemy info
                break;  
        }
    }

    


    //when an action is selected
    public void ActionUIOnClick(ActionUI actionUI){
        selectedAction = actionUI.action;
        selectedActor.SetActionText("[-Select Target-]");
        actionDescription.text = actionUI.action.actionDescription;
        SetBattleSelectionState(BattleSelectionState.TARGET);
        executeTurnBtn.interactable = false;
    }

    
    public void TargetSelected()
    {
        
        battleSystem.AddCharacterAction(selectedActor.member, selectedAction, selectedTarget.member);
        selectedActor.hasSelectedAction = true;
        selectedActor.SetActionText(selectedAction.actionName + " on " + selectedTarget.member.characterName);
        partyUI.DeselectAll();
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        ClearActionPanel();
        bool canExecuteTurn = true;
        foreach (MemberUI memberUI in partyUI.memberUIs)
        {
            if (!memberUI.hasSelectedAction)
            {
                canExecuteTurn = false;
                break;
            }
        }
        if (canExecuteTurn)
        {
            executeTurnBtn.interactable = true;
            battleStateText.text = "EXECUTE TURN";
        }else{
            executeTurnBtn.interactable = false;
        }
        selectedAction = null;
        selectedTarget = null;
        selectedActor = null;

    }
    public void ExecuteTurn(){

        int enemyCount = battleSystem.enemies.Count;
        int partyCount = battleSystem.partyMembers.Count;
        battleSystem.ExecuteTeamActions();
        string output = "Executed Actions.. You killed " + (enemyCount - battleSystem.enemies.Count)+" enemie(s) and lost"+(partyCount - battleSystem.partyMembers.Count)+"party member; Enemies Turn..";
        Debug.Log(output);
        partyCount = battleSystem.partyMembers.Count;
        partyUI.SetParty(battleSystem.partyMembers);
        enemyUI.SetParty(battleSystem.enemies);
        battleSystem.AddTurnActionsForAllCharacters(battleSystem.enemies, battleSystem.partyMembers);
        battleSystem.ExecuteTurnActions();
        output += "\nExecuted Actions.. You lost " + (partyCount - battleSystem.partyMembers.Count) + " party member(s); Your Turn..";
        actionDescription.text = output;
    }


}
