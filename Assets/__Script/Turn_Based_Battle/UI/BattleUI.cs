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
    [SerializeField] private Button executeBtn; //button to execute turn, or to confirm action selection
    //public SpriteRenderer charaSpriteRenderer;    
    [SerializeField]private Image charaBodySprite; //to display the selected character's sprite
    private TMP_Text battleStateText;
    
    [Header("Run-Time")]
    //Temporarily store the selected actor, action and target
    public MemberUI selectedActor;
    public BattleAction selectedAction;
    public MemberUI selectedTarget;
    public AudioSource audioSource;
    
    //references
    private PartyManager playerParty;
    private PartyManager enemyParty;
    private List<Character> allPlayerMembers;
    private List<Character> allEnemyMembers;

    //UI settings
    [SerializeField]private int battleTurnDuration = 2; //duration of each turn in seconds
    
    private void Awake()
    {
        if (instance == null) instance = this;
        audioSource = GetComponent<AudioSource>();
        battleStateText = executeBtn.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {

        ClearActionPanel();
        //battleSystem = BattleSystem.instance;
        battleManager = BattleManager.instance;
        characterUI = CharacterUI.instance;
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        executeBtn.onClick.AddListener(ExecuteBtnOnClick); 

        playerParty = battleManager.GetPartyManager(PartyType.PLAYER);
        enemyParty = battleManager.GetPartyManager(PartyType.ENEMY);
        allPlayerMembers = playerParty.GetAllPartyMembers();
        allEnemyMembers = enemyParty.GetAllPartyMembers();
        
        
        partyUI.SetParty(allPlayerMembers);
        enemyUI.SetParty(allEnemyMembers);

        executeBtn.interactable = false;
    }

    public void SetBattleSelectionState(BattleSelectionState state){
        battleSelectionState = state;

        switch(battleSelectionState){
            case (BattleSelectionState.ACTOR):
                CheckIfCanExecuteTurn();
                break;
            case (BattleSelectionState.ACTION):
                executeBtn.interactable = true;
                break;
            case (BattleSelectionState.TARGET):
                SetActionDescriptionText("Select a Target", true);
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


    public void MemberUIOnClick(MemberUI memberUI){
        
        switch(battleSelectionState){
            case (BattleSelectionState.TARGET): //this member is selected as target
                selectedTarget = memberUI;
                TargetSelected(memberUI);
                break;
            default:
                ActorSelected(memberUI);
                break;  
        }
        
    }

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
            executeBtn.interactable = false;
            battleStateText.GetComponent<TypewriterEffect>().Run("Not Enough MP for " + actionUI.action.actionName, battleStateText);
        }
        else
        {
            executeBtn.interactable = true;
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
        executeBtn.interactable = false;

        string actionDescription = "Select an action for " + memberUI.member.characterName;
        SetActionDescriptionText(actionDescription, true);
    }

    public void ActionSelected()
    {

        switch (selectedAction.targetType)
        {
            case (TargetType.SINGLE):
                SetBattleSelectionState(BattleSelectionState.TARGET);
                selectedActor.SetStateText(selectedAction.actionName + "- Select a Target");
                executeBtn.interactable = false;
                SetBattleSelectionState(BattleSelectionState.TARGET);
                break;
            case (TargetType.ALL_OPPONENT):
                //TODO 
                selectedActor.SetStateText(selectedAction.actionName + " Targeting all opponents");
                string actionDescription = selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on all opponents";
                SetActionDescriptionText(actionDescription, true);
                TargetsSelected(enemyUI.memberUIs);
                selectedActor.hasSelectedAction = true;
                selectedAction = null;
                selectedTarget = null;
                selectedActor = null;
                SetBattleSelectionState(BattleSelectionState.ACTOR);
                ClearActionPanel();
                break;
        }
    }

    //When a target is selected
    public void TargetSelected(MemberUI targetMemberUI)
    {
        string actionDescription = selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on " + selectedTarget.member.characterName;
        SetActionDescriptionText(actionDescription, true);
        ClearSelectedActionForCharacter(selectedActor.member); //remove indicator for previeus target
        //remove all actors from enemyUI -----TODO: actually implement this in battkeManager
        if (selectedAction.targetType==TargetType.SINGLE){
            selectedTarget = targetMemberUI;
            targetMemberUI.AddActor(selectedActor.member, selectedAction);
            TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, selectedTarget.member);
            battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(), turnBattleAction);
            selectedActor.hasSelectedAction = true;
            selectedActor.SetStateText(selectedAction.actionName + " on " + selectedTarget.member.characterName);
            partyUI.DeselectAll();
            SetBattleSelectionState(BattleSelectionState.ACTOR);
            ClearActionPanel();
            selectedAction = null;
            selectedTarget = null;
            selectedActor = null;
        }
        
        
        CheckIfCanExecuteTurn();
        
    }
    //for multi-target actions
    public void TargetsSelected(List<MemberUI> targetUI){

        ClearSelectedActionForCharacter(selectedActor.member); //remove indicator for previeus target
        
        foreach (MemberUI ui in targetUI)
        {
            TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, ui.member);
            battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(), turnBattleAction);
            ui.AddActor(selectedActor.member, selectedAction);
        }
        selectedActor.hasSelectedAction = true;
        selectedActor.SetStateText(selectedAction.actionName + " on all");
        CheckIfCanExecuteTurn();
    }

    void CheckIfCanExecuteTurn(){
        bool canExecuteTurn = true;
        foreach (MemberUI ui in partyUI.memberUIs)
        {
            if (!ui.hasSelectedAction)
            {
                canExecuteTurn = false;
                break;
            }
        }
        if (canExecuteTurn)
        {
            executeBtn.interactable = true;
            battleSelectionState = BattleSelectionState.CONFIRM;
            battleStateText.GetComponent<TypewriterEffect>().Run("EXECUTE TURN", battleStateText);
        }
        else
        {
            executeBtn.interactable = false;
            battleStateText.GetComponent<TypewriterEffect>().Run("SELECT MEMBER", battleStateText);
        }
    }

    public void ClearSelectedActionForCharacter(Character character){
        foreach (MemberUI ui in partyUI.memberUIs)
        {
            ui.RemoveActor(selectedActor.member);
        }
        foreach (MemberUI ui in enemyUI.memberUIs)
        {
            ui.RemoveActor(selectedActor.member);
        }
        PartyManager party = battleManager.GetPartyManager(character.GetPartyType());
        party.DeleteTurnBattleActionsForActor(character);
        selectedActor.hasSelectedAction = false;
    }

    //On Execute Turn Button Click
    public void ExecuteBtnOnClick(){
        switch(battleSelectionState){
            case (BattleSelectionState.CONFIRM):
                battleStateText.GetComponent<TypewriterEffect>().Run("FIGHT", battleStateText);
                StartCoroutine(ExecuteTurnWithDelay(battleTurnDuration));   
                bool gameEnd = battleManager.IfBattleEnded();
                if (!gameEnd)
                {
                    //add enemy actions
                    enemyParty.AddActionsForAllCharacters(playerParty);
                }

                if (battleManager.GetBattleState() == BattleState.WON)
                {
                    battleStateText.GetComponent<TypewriterEffect>().Run("YOU WIN!", battleStateText);
                    executeBtn.interactable = false;
                }
                else if (battleManager.GetBattleState() == BattleState.LOST)
                {
                    battleStateText.GetComponent<TypewriterEffect>().Run("YOU LOSE!", battleStateText);
                    executeBtn.interactable = false;
                }
                else
                {
                    SetBattleSelectionState(BattleSelectionState.ACTOR);
                    CheckIfCanExecuteTurn();
                }

                partyUI.SetParty(allPlayerMembers);
                enemyUI.SetParty(allEnemyMembers);

                break;
            case (BattleSelectionState.ACTION):
                ActionSelected();
                
                break;
            case (BattleSelectionState.ACTOR):
                break;
            case (BattleSelectionState.TARGET):
                break;
        }
    }

    //equivalent to ExecuteBtnOnClick() in BattleManager, but with a delay
    //remember to update both if you change one
    public IEnumerator ExecuteTurnWithDelay(float seconds){
        List<TurnBattleAction> playerActions = playerParty.turnBattleActions;
        List<TurnBattleAction> enemyActions = enemyParty.turnBattleActions;
        
        while ((playerActions.Count > 0 || enemyActions.Count > 0) && !battleManager.IfBattleEnded())
        {
            PartyManager turnParty;
            string output = "";
            //if both parties have actions, compare the speed of the first action in each list
            if (playerActions.Count > 0 && enemyActions.Count > 0) 
            {
                if (playerActions[0].actor.speed >= enemyActions[0].actor.speed)
                {
                    turnParty = playerParty;
                }
                else
                {
                    turnParty = enemyParty;
                }
            }
            else
            {
                //execute the action of the party with actions
                if (playerActions.Count > 0)
                {
                    turnParty = playerParty;
                }
                else
                {
                    turnParty = enemyParty;
                }
            }
            TurnBattleAction turnBattleAction = turnParty.turnBattleActions[0];
            output +=battleManager.TestExecuteTurnBattleAction(turnBattleAction) + "\n";
            charaBodySprite.sprite = turnBattleAction.actor.fullBodySprite;
            turnParty.turnBattleActions.RemoveAt(0);
            SetActionDescriptionText(output, true);
            enemyUI.SetParty(allEnemyMembers);
            partyUI.SetParty(allPlayerMembers);
            yield return new WaitForSeconds(seconds);
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
