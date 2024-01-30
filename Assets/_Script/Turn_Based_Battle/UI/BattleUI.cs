using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleUI : MonoBehaviour
{
    public static BattleUI instance { get; private set; } //this is a singleton

    public enum BattleSelectionState { ACTOR, ACTION, TARGET, CONFIRM };
    public BattleSelectionState battleSelectionState;

    //Link to Canvas UI Elements
    [SerializeField] private PartyUI partyUI;
    [SerializeField] private PartyUI enemyUI;
    [SerializeField] private TMP_Text liveText; //Types out live update of the battle

    //Singleton References
    private CharacterUI characterUI; //to display the selected character's stats and equipment, etc. 
    private BattleSystem battleSystem;


    [Header("Action Panel")]
    public GameObject actionUIPrefab;
    public GameObject actionUIParent;
    public TMP_Text actionDescription;
    List<ActionUI> actionUIs = new List<ActionUI>();
    public Button executeTurnBtn; //button to execute turn, or to confirm action selection
    public SpriteRenderer charaSpriteRenderer;    
    private TMP_Text battleStateText;
    
    [Header("Run-Time")]
    //Temporarily store the selected actor, action and target
    public MemberUI selectedActor;
    public BattleAction selectedAction;
    public MemberUI selectedTarget;

    public AudioSource audioSource;
    

    


    private void Awake()
    {
        if (instance == null) instance = this;
        audioSource = GetComponent<AudioSource>();
        battleStateText = executeTurnBtn.GetComponentInChildren<TMP_Text>();
    }


    private void Start()
    {

        ClearActionPanel();
        battleSystem = BattleSystem.instance;
        characterUI = CharacterUI.instance;
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        executeTurnBtn.onClick.AddListener(ExecuteTurn); 
        
        partyUI.SetParty(battleSystem.partyMembers);
        enemyUI.SetParty(battleSystem.enemies);
        charaSpriteRenderer.sprite = null;
        executeTurnBtn.interactable = false;
        SetLiveText("Battle Start! Selet Actor to give instructions.");
    }

    public void SetBattleSelectionState(BattleSelectionState state){
        battleSelectionState = state;

        SetLiveText("CHOOSE  " + battleSelectionState.ToString());

        switch(battleSelectionState){
            case (BattleSelectionState.ACTOR):
                enemyUI.DisableSelection();
                executeTurnBtn.interactable = false;
                break;
            case (BattleSelectionState.ACTION):
                executeTurnBtn.interactable = true;
                break;
            case (BattleSelectionState.TARGET):
                enemyUI.EnableSelection();
                break;
        }
    }
    
    
    public void SetActionPanel(List<BattleAction> actions)
    {
        ClearActionPanel();
        foreach (BattleAction action in actions)
        {
            GameObject actionUIObject = Instantiate(actionUIPrefab, actionUIParent.transform);
            ActionUI actionUI = actionUIObject.GetComponent<ActionUI>();
            actionUI.SetAction(action);
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


    //when a memberUI is clicked
    public void MemberUIOnClick(MemberUI memberUI){
        
        switch(battleSelectionState){
            case (BattleSelectionState.TARGET): //this member is selected as target
                TargetSelected(memberUI);
                break;
            default:
                ActorSelected(memberUI);
                break;  
        }
        
    }

    //when an enemy memberUI is clicked
    public void EnemyMemberOnClick(MemberUI enemyUI){
        switch(battleSelectionState){
            case (BattleSelectionState.TARGET): //this member is selected as target
                selectedTarget = enemyUI;  
                TargetSelected(enemyUI);
                break;
            default:
                //display enemy info
                break;  
        }
    }

    


    //when an action is selected
    public void ActionUIOnClick(ActionUI actionUI){
        
        selectedAction = actionUI.action;
        
        
        actionDescription.text = selectedAction.actionDescription;

        bool canSelectAction = (selectedAction.mpCost > selectedActor.member.GetCurrentMP());
        if (canSelectAction ){
           actionDescription.text += "\nNot enough MP!";
            executeTurnBtn.interactable = false;
            battleStateText.GetComponent<TypewriterEffect>().Run("Not Enough MP for " + actionUI.action.actionName, battleStateText);
        }
        else
        {
            executeTurnBtn.interactable = true;
            battleStateText.GetComponent<TypewriterEffect>().Run("Select [" + actionUI.action.actionName + "]", battleStateText);
        }

    }



    public void ActorSelected(MemberUI memberUI){
        //liveText.GetComponent<TypewriterEffect>().typeSound.clip = memberUI.member.placeHolder_sfx;
        SetLiveText("Select Action for " + memberUI.member.characterName + " ! ");
       
        partyUI.DeselectAll();
        memberUI.Select();
        characterUI.SetCharacter(memberUI.member);
        selectedActor = memberUI;
        battleSelectionState = BattleSelectionState.ACTION;
        SetActionPanel(memberUI.member.actions);
        charaSpriteRenderer.sprite = memberUI.member.fullBodySprite;
        SetBattleSelectionState(BattleSelectionState.ACTION);
        executeTurnBtn.interactable = false;
    }



    //When a target is selected
    public void TargetSelected(MemberUI targetMemberUI)
    {
        if(selectedAction.targetType == TargetType.SINGLE){
            SetLiveText(selectedActor.member.characterName + " will use " + selectedAction.actionName + " on " + targetMemberUI.member.characterName);
        }
        
        if(selectedAction.targetType!=TargetType.ALL_OPPONENT){
            foreach (MemberUI ui in enemyUI.memberUIs)
            {
                ui.RemoveActor(selectedActor.member);
            }
        }
        foreach (MemberUI ui in partyUI.memberUIs)
        {
            ui.RemoveActor(selectedActor.member);
        }
        selectedActor.hasSelectedAction = true;
        targetMemberUI.AddActor(selectedActor.member, selectedAction);
        selectedTarget = targetMemberUI;
        battleSystem.AddCharacterAction(selectedActor.member, selectedAction, selectedTarget.member);
        if(selectedAction.targetType!=TargetType.ALL_OPPONENT){
            selectedActor.SetStateText(selectedAction.actionName + " on " + selectedTarget.member.characterName);
            partyUI.DeselectAll();
            SetBattleSelectionState(BattleSelectionState.ACTOR);
            ClearActionPanel();
        }
        
        
        bool canExecuteTurn = true;
        foreach (MemberUI ui in partyUI.memberUIs)
        {
            if (!ui.hasSelectedAction&&ui.member.characterState!=CharacterState.DEAD)
            {
                canExecuteTurn = false;
                break;
            }
        }
        if (canExecuteTurn)
        {
            
            executeTurnBtn.interactable = true;
            battleSelectionState = BattleSelectionState.CONFIRM;
            battleStateText.GetComponent<TypewriterEffect>().Run("EXECUTE TURN",battleStateText);
        }else{
            executeTurnBtn.interactable = false;
        }
        if(selectedAction.targetType == TargetType.SINGLE)
        {
            selectedAction = null;
            selectedTarget = null;
            selectedActor = null;
        }
        
    }
    
    public void ExecuteTurn(){
        switch(battleSelectionState){
            case (BattleSelectionState.CONFIRM):
                battleStateText.GetComponent<TypewriterEffect>().Run("FIGHT", battleStateText);
                int enemyCount = battleSystem.enemies.Count;
                int partyCount = battleSystem.partyMembers.Count;
                battleSystem.ExecuteTeamActions();
                string output = "Executed Actions.. You killed " + (enemyCount - battleSystem.enemies.Count) + " enemie(s) and lost" + (partyCount - battleSystem.partyMembers.Count) + "party member; Enemies Turn..";
                Debug.Log(output);
                partyCount = battleSystem.partyMembers.Count;
                partyUI.SetParty(battleSystem.partyMembers);
                enemyUI.SetParty(battleSystem.enemies);
                bool gameEnd = battleSystem.checkIfGameEnd();
                if (!gameEnd)
                {
                    battleSystem.AddTurnActionsForAllCharacters(battleSystem.enemies, battleSystem.partyMembers);
                    battleSystem.ExecuteTurnActions();
                    output += "\nExecuted Actions.. You lost " + (partyCount - battleSystem.partyMembers.Count) + " party member(s); Your Turn..";
                    actionDescription.text = output;
                }

                if (battleSystem.state == BattleState.WON)
                {
                    battleStateText.GetComponent<TypewriterEffect>().Run("YOU WIN!", battleStateText);
                    executeTurnBtn.interactable = false;
                }
                else if (battleSystem.state == BattleState.LOST)
                {
                    battleStateText.GetComponent<TypewriterEffect>().Run("YOU LOSE!", battleStateText);
                    executeTurnBtn.interactable = false;
                }
                else
                {
                    SetBattleSelectionState(BattleSelectionState.ACTOR);
                    executeTurnBtn.interactable = false;
                }
                break;
            case (BattleSelectionState.ACTION):
                ConfirmActionSelection();
                SetLiveText("Select Actor");
                break;
            case (BattleSelectionState.ACTOR):
                SetLiveText("Select Actor to give instructions.");
                break;
            case (BattleSelectionState.TARGET):
                SetLiveText("Selected Action: [" + selectedAction.actionName + "] - Select Target: ");
                break;
        }

        

    }

    public void ConfirmActionSelection(){
        switch (selectedAction.targetType)
        {
            case (TargetType.SINGLE):
                SetLiveText("Selected Action: [" + selectedAction.actionName + "] - Select Target: ");

                selectedActor.SetStateText("[-Select Target-]");
                SetBattleSelectionState(BattleSelectionState.TARGET);
                executeTurnBtn.interactable = false;
                break;
            case (TargetType.ALL_OPPONENT):
                //TODO 
                foreach (MemberUI ui in enemyUI.memberUIs)
                {
                    ui.RemoveActor(selectedActor.member);
                }
                SetLiveText(selectedActor.member.characterName + "Will perform " + selectedAction.actionName + " on all opponents");
                selectedActor.hasSelectedAction = true;
                selectedActor.SetStateText(selectedAction.actionName + " on all opponents");
                foreach (MemberUI ui in enemyUI.memberUIs)
                {
                    TargetSelected(ui);
                }
                selectedAction = null;
                selectedTarget = null;
                selectedActor = null;
                SetBattleSelectionState(BattleSelectionState.ACTOR);
                ClearActionPanel();
                break;
        }
    }

    public void SetLiveText(string text){
        liveText.GetComponent<TypewriterEffect>().Run(text,liveText);
    }

}
