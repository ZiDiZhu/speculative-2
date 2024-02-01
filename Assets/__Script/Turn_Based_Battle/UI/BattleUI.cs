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

    //Singleton References
    private CharacterUI characterUI; //to display the selected character's stats and equipment, etc. 
    //private BattleSystem battleSystem;
    private BattleManager battleManager;

    [Header("Action Panel")]
    [SerializeField]private GameObject actionUIPrefab;
    [SerializeField]private GameObject actionUIParent;
    [SerializeField]private TMP_Text actionDescriptionText;
    List<ActionUI> actionUIs = new List<ActionUI>();
    [SerializeField] private Button executeTurnBtn; //button to execute turn, or to confirm action selection
    //public SpriteRenderer charaSpriteRenderer;    
    [SerializeField]private Image charaBodySprite; //to display the selected character's sprite
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
        //battleSystem = BattleSystem.instance;
        battleManager = BattleManager.instance;
        characterUI = CharacterUI.instance;
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        executeTurnBtn.onClick.AddListener(ExecuteTurn); 
        
        
        partyUI.SetParty(battleManager.GetPartyManager(PartyType.PLAYER).GetAllPartyMembers());
        enemyUI.SetParty(battleManager.GetPartyManager(PartyType.ENEMY).GetAllPartyMembers());
        //charaSpriteRenderer.sprite = null;
        //charaBodySprite.sprite = null;
        executeTurnBtn.interactable = false;
    }

    public void SetBattleSelectionState(BattleSelectionState state){
        battleSelectionState = state;


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
        //SetActionDescriptionText("",false);
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
        
        string actionDescription = "";
        actionDescription += actionUI.action.actionName + "\n";
        actionDescription += "MP Cost: " + actionUI.action.mpCost + "\n";
        actionDescription += "Type: " + actionUI.action.actionType + "\n";
        actionDescription += "Target: " + actionUI.action.targetType + "\n";
        actionDescription += "Description: " + actionUI.action.actionDescription + "\n";
        bool canSelectAction = (selectedAction.mpCost < selectedActor.member.GetCurrentMP());
        if (!canSelectAction){
           actionDescription += "\nNot enough MP!";
            executeTurnBtn.interactable = false;
            battleStateText.GetComponent<TypewriterEffect>().Run("Not Enough MP for " + actionUI.action.actionName, battleStateText);
        }
        else
        {
            executeTurnBtn.interactable = true;
            battleStateText.GetComponent<TypewriterEffect>().Run("Select [" + actionUI.action.actionName + "]", battleStateText);
        }
        SetActionDescriptionText(actionDescription, false);
    }



    public void ActorSelected(MemberUI memberUI){
       
        partyUI.DeselectAll();
        memberUI.Select();
        characterUI.SetCharacter(memberUI.member);
        selectedActor = memberUI;
        battleSelectionState = BattleSelectionState.ACTION;
        SetActionPanel(memberUI.member.actions);
        charaBodySprite.sprite = memberUI.member.fullBodySprite;
        SetBattleSelectionState(BattleSelectionState.ACTION);
        executeTurnBtn.interactable = false;

        string actionDescription = "Select an action for " + memberUI.member.characterName;
        SetActionDescriptionText(actionDescription, true);
    }



    //When a target is selected
    public void TargetSelected(MemberUI targetMemberUI)
    {
        string actionDescription = selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on " + selectedTarget.member.characterName;
        SetActionDescriptionText(actionDescription, true);

        //remove all actors from enemyUI -----TODO: actually implement this in battkeManager
        if (selectedAction.targetType!=TargetType.ALL_OPPONENT){
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
        TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, selectedTarget.member);
        battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(),turnBattleAction);
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

    //On Execute Turn Button Click
    public void ExecuteTurn(){
        switch(battleSelectionState){
            case (BattleSelectionState.CONFIRM):
                battleStateText.GetComponent<TypewriterEffect>().Run("FIGHT", battleStateText);
                //string battleOutput = battleManager.ExecuteTurn(); //execute turn - simutaneously for both parties
                //SetActionDescriptionText(battleOutput, true);
                StartCoroutine(ExecuteTurnWithDelay(1.5f));   
                bool gameEnd = battleManager.IfBattleEnded();
                if (!gameEnd)
                {
                    //add enemy actions
                    battleManager.GetPartyManager(PartyType.ENEMY).AddActionsForAllCharacters(battleManager.GetPartyManager(PartyType.PLAYER));
                }

                if (battleManager.GetBattleState() == BattleState.WON)
                {
                    battleStateText.GetComponent<TypewriterEffect>().Run("YOU WIN!", battleStateText);
                    executeTurnBtn.interactable = false;
                }
                else if (battleManager.GetBattleState() == BattleState.LOST)
                {
                    battleStateText.GetComponent<TypewriterEffect>().Run("YOU LOSE!", battleStateText);
                    executeTurnBtn.interactable = false;
                }
                else
                {
                    SetBattleSelectionState(BattleSelectionState.ACTOR);
                    executeTurnBtn.interactable = false;
                }

                partyUI.SetParty(battleManager.GetPartyManager(PartyType.PLAYER).GetAllPartyMembers());
                enemyUI.SetParty(battleManager.GetPartyManager(PartyType.ENEMY).GetAllPartyMembers());

                break;
            case (BattleSelectionState.ACTION):
                ConfirmActionSelection();
                break;
            case (BattleSelectionState.ACTOR):
                break;
            case (BattleSelectionState.TARGET):
                break;
        }
    }

    //equivalent to ExecuteTurn() in BattleManager, but with a delay
    //remember to update both if you change one
    public IEnumerator ExecuteTurnWithDelay(float seconds){
        List<TurnBattleAction> playerActions = battleManager.GetPartyManager(PartyType.PLAYER).turnBattleActions;
        List<TurnBattleAction> enemyActions = battleManager.GetPartyManager(PartyType.ENEMY).turnBattleActions;

        while ((playerActions.Count > 0 || enemyActions.Count > 0) && !battleManager.IfBattleEnded())
        {

            string output = "";
            //if both parties have actions, compare the speed of the first action in each list
            if (playerActions.Count > 0 && enemyActions.Count > 0)
            {
                if (playerActions[0].actor.speed >= enemyActions[0].actor.speed)
                {
                    output += battleManager.TestExecuteTurnBattleAction(playerActions[0]) + "\n";
                    charaBodySprite .sprite = playerActions[0].actor.fullBodySprite;
                    playerActions.RemoveAt(0);
                }
                else
                {
                    output += battleManager.TestExecuteTurnBattleAction(enemyActions[0]) + "\n";
                    charaBodySprite.sprite = enemyActions[0].actor.fullBodySprite;
                    enemyActions.RemoveAt(0);
                }
            }
            else
            {
                //execute the action of the party with actions
                if (playerActions.Count > 0)
                {
                    output += battleManager.TestExecuteTurnBattleAction(playerActions[0]) + "\n";
                    charaBodySprite.sprite = playerActions[0].actor.fullBodySprite;
                    playerActions.RemoveAt(0);
                }
                else if (enemyActions.Count > 0)
                {
                    output += battleManager.TestExecuteTurnBattleAction(enemyActions[0]) + "\n";
                    charaBodySprite.sprite = enemyActions[0].actor.fullBodySprite;
                    enemyActions.RemoveAt(0);
                }
            }
            SetActionDescriptionText(output, true);
            yield return new WaitForSeconds(seconds);
        }
    }


    public void ConfirmActionSelection(){


        switch (selectedAction.targetType)
        {
            case (TargetType.SINGLE):
                selectedActor.SetStateText(selectedAction.actionName);
                SetBattleSelectionState(BattleSelectionState.TARGET);
                executeTurnBtn.interactable = false;
                break;
            case (TargetType.ALL_OPPONENT):
                //TODO 
                foreach (MemberUI ui in enemyUI.memberUIs)
                {
                    ui.RemoveActor(selectedActor.member);
                }
                selectedActor.hasSelectedAction = true;
                string actionDescription =selectedActor.member.characterName +" will perform "+ selectedAction.actionName + " on all opponents";
                SetActionDescriptionText(actionDescription, true);
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

    public void SetActionDescriptionText(string txt, bool useTypeWriterEffect){
        if (useTypeWriterEffect){
            actionDescriptionText.GetComponent<TypewriterEffect>().Run(txt, actionDescriptionText);
        }else{
            actionDescriptionText.text = txt;
        }
    }

}
