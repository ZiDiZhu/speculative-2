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

    [SerializeField] private Transform fxParentTransform; //parent transform for all particles fx objects

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
    private TMP_Text executeButtonText;
    
    [Header("Run-Time")]
    //Temporarily store the selected actor, action and target
    public MemberUI selectedActor;
    public BattleSkill selectedAction;
    public MemberUI selectedTarget;
    public AudioSource audioSource;
    
    //references
    private PartyManager playerParty;
    private PartyManager enemyParty;

    //UI settings
    [SerializeField]private int battleTurnDuration = 2; //duration of each turn in seconds
    
    private void Awake()
    {
        if (instance == null) instance = this;
        
    }



    private void Start()
    {
        executeButtonText = executeBtn.GetComponentInChildren<TMP_Text>();
        ClearActionPanel();
        //battleSystem = BattleSystem.instance;
        battleManager = BattleManager.instance;
        characterUI = CharacterUI.instance;
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        executeBtn.onClick.AddListener(ExecuteBtnOnClick); 

        playerParty = battleManager.GetPartyManager(PartyType.PLAYER);
        enemyParty = battleManager.GetPartyManager(PartyType.ENEMY);
        partyUI.AssignMembers(playerParty);
        enemyUI.AssignMembers(enemyParty);

        executeBtn.interactable = false;
    }

    public void SetBattleSelectionState(BattleSelectionState state){
        battleSelectionState = state;

        switch(battleSelectionState){
            case (BattleSelectionState.ACTOR):
                executeBtn.interactable = false;
                SetExecuteButtonText("Select Actor");
                selectedActor = null;
                selectedAction = null;
                selectedTarget = null;
                break;
            case (BattleSelectionState.ACTION):
                executeBtn.interactable = true;
                break;
            case (BattleSelectionState.TARGET):
                executeBtn.interactable = false;
                SetExecuteButtonText("Select Target");
                break;
        }
    }
    
    public void SetActionPanel(List<BattleSkill> actions)
    {
        ClearActionPanel();
        foreach (BattleSkill action in actions)
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
    }


    public void MemberUIOnClick(MemberUI memberUI){
        
        switch(battleSelectionState){
            case (BattleSelectionState.ACTOR):
                executeBtn.interactable = false;
                ActorSelected(memberUI);
                break;
            case (BattleSelectionState.ACTION): //this member is selected as actor
                executeBtn.interactable = false;
                ActorSelected(memberUI);
                break;
            case (BattleSelectionState.TARGET): //this member is selected as target
                selectedTarget = memberUI;
                TargetSelected(memberUI);
                
                break;
            default:
                executeBtn.interactable = false;
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
                SetBattleSelectionState(BattleSelectionState.ACTOR);
                SetActionDescriptionText(enemyUI.member.GetDescription(), true);
                break;  
        }
    }

    

    public void ActorSelected(MemberUI memberUI){
       
        partyUI.DeselectAll();
        memberUI.Select();
        characterUI.SetCharacter(memberUI.member);
        selectedActor = memberUI;
        battleSelectionState = BattleSelectionState.ACTION;
        SetActionPanel(memberUI.member.actions);
        
        SetBattleSelectionState(BattleSelectionState.ACTION);
        

        string actionDescription = "Select an action for " + memberUI.member.characterName;
        SetActionDescriptionText(actionDescription, true);
    }

    public void ActionUIOnClick(ActionUI actionUI)
    {
        selectedAction = actionUI.action;
        string actionDescription = "";
        actionDescription += actionUI.action.actionName + "\n";
        actionDescription += "MP Cost: " + actionUI.action.mpCost + "\n";
        actionDescription += "Type: " + actionUI.action.actionType + "\n";
        actionDescription += "Target: " + actionUI.action.targetType + "\n";
        actionDescription += "Description: " + actionUI.action.actionDescription + "\n";
        bool canSelectAction = (selectedAction.mpCost < selectedActor.member.GetCurrentMP());
        if (selectedActor.member.GetPartyType() == PartyType.PLAYER)
        {
            if (!canSelectAction)
            {
                executeBtn.interactable = false;
                SetExecuteButtonText("Not Enough MP");
            }
            else
            {
                executeBtn.interactable = true;
                SetExecuteButtonText("Select " + selectedAction.actionName);
            }
        }

        SetActionDescriptionText(actionDescription, false);
    }

    public void ActionConfirmed()
    {
        string actorTxt = "";
        string actionDescription = "";

        switch (selectedAction.targetType)
        {
            case (TargetType.SINGLE_ALLY):
            case (TargetType.SINGLE_OPPONENT): //proceed to select a target
                SetBattleSelectionState(BattleSelectionState.TARGET);
                selectedActor.UpdateMemberUI(selectedAction.actionName + "- Select a Target");
                break;
            case (TargetType.ALL_OPPONENT):
            case (TargetType.ALL_PARTY)://confirm turn action selection
                actorTxt = selectedAction.actionName + " Targeting all Party Members";
                actionDescription = selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on all party members.";
                TargetsSelected(partyUI.GetMemberUIs());
                ConfirmTurnActionSelection(actorTxt, actionDescription);
                break;
            case (TargetType.ALL_ALLY)://confirm turn action selection
                actorTxt = selectedAction.actionName + " Targeting all ally";
                actionDescription = selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on all ally.";
                List<MemberUI> allyUIs = partyUI.GetMemberUIs();
                allyUIs.Remove(selectedActor);  
                TargetsSelected(allyUIs);
                ConfirmTurnActionSelection(actorTxt, actionDescription);
                break;
        }
    }


    void ConfirmTurnActionSelection(string actorTxt, string actionDescription){
        selectedActor.UpdateMemberUI(actorTxt);
        SetActionDescriptionText(actionDescription, true);
        selectedActor.hasSelectedAction = true;
        selectedAction = null;
        selectedTarget = null;
        selectedActor = null;
        SetBattleSelectionState(BattleSelectionState.ACTOR);
        ClearActionPanel();
        partyUI.DeselectAll();
        enemyUI.DeselectAll();
        CheckIfCanExecuteTurn();
    }

    public void TargetSelected(MemberUI targetMemberUI)
    {
        
        ClearSelectedActionForCharacter(selectedActor.member); 
        if (selectedAction.targetType==TargetType.SINGLE_OPPONENT){
            selectedTarget = targetMemberUI;
            targetMemberUI.AddActor(selectedActor.member, selectedAction);
            TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, selectedTarget.member);
            battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(), turnBattleAction);
        }

        ConfirmTurnActionSelection(selectedAction.actionName + " Targeting " + selectedActor.member.characterName, selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on " + selectedTarget.member.characterName);
    }

    public void TargetsSelected(List<MemberUI> targetUI){

        ClearSelectedActionForCharacter(selectedActor.member); //remove indicator for previeus target
        
        foreach (MemberUI ui in targetUI)
        {
            TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, ui.member);
            battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(), turnBattleAction);
            ui.AddActor(selectedActor.member, selectedAction);
        }
    }

    void ClearSelectedActionForCharacter(Character character)
    {
        foreach (MemberUI ui in partyUI.GetMemberUIs())
        {
            ui.RemoveActor(selectedActor.member);
        }
        foreach (MemberUI ui in enemyUI.GetMemberUIs())
        {
            ui.RemoveActor(selectedActor.member);
        }
        PartyManager party = battleManager.GetPartyManager(character.GetPartyType());
        party.DeleteTurnBattleActionsForActor(character);
        selectedActor.hasSelectedAction = false;
    }

    void CheckIfCanExecuteTurn(){
        
        if (partyUI.IsPartyReady())
        {
            executeBtn.interactable = true;
            battleSelectionState = BattleSelectionState.CONFIRM;
            SetExecuteButtonText("EXECUTE TURN");
        }
        else
        {
            executeBtn.interactable = false;
            SetExecuteButtonText("YOUR TURN");
        }
    }

    

    //On Execute Turn Button Click
    public void ExecuteBtnOnClick(){
        switch(battleSelectionState){
            case (BattleSelectionState.CONFIRM):
                StartCoroutine(ExecuteTurnWithDelay(battleTurnDuration));
                SetBattleSelectionState(BattleSelectionState.ACTOR);
                break;
            case (BattleSelectionState.ACTION):
                ActionConfirmed();
                break;
            case (BattleSelectionState.ACTOR):
                if (battleManager.GetBattleState() == BattleState.WON)
                {
                    Debug.Log("You Win!");
                }
                else if (battleManager.GetBattleState() == BattleState.LOST)
                {
                    Debug.Log("You Lose!");
                }
                break;
            case (BattleSelectionState.TARGET):
                SetBattleSelectionState(BattleSelectionState.ACTOR);  
        
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

            PartyManager turnParty; //the party that will execute the action
            MemberUI actorUI, targetUI; //the UI of the actor and target of the action


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

            

            

            //This is gonna be the action that is executed
            TurnBattleAction turnBattleAction = turnParty.turnBattleActions[0];
            

            //Highlight actor 
            if(turnParty.GetPartyType()==PartyType.PLAYER)partyUI.GetMemberUI(turnBattleAction.actor).Select();
            else enemyUI.GetMemberUI(turnBattleAction.actor).Select();
            //highlight target
            if (turnBattleAction.target.GetPartyType() == PartyType.PLAYER) partyUI.GetMemberUI(turnBattleAction.target).Select();
            else enemyUI.GetMemberUI(turnBattleAction.target).Select();
    

            //Get the target's HP before the action
            int targetHpBefore = turnBattleAction.target.GetCurrentHP();

            yield return new WaitForSeconds(seconds * 0.2f);

            //Executes the action and returns a string with the result
            output +=battleManager.TestExecuteTurnBattleAction(turnBattleAction) + "\n";

            
            //Get the target's HP after the action and display the damage/heal
            //TODO: improve the logic of displaying the action taking effect
            //Currently, it can only tell if action was valid and if it was a heal or damage
            int targetHpAfter = turnBattleAction.target.GetCurrentHP(); 
            

            //remove the action from queue            
            turnParty.turnBattleActions.RemoveAt(0);

            SetActionDescriptionText(output, true);
            
            yield return new WaitForSeconds(seconds * 0.2f);
            
            GameObject actionfx = null;

            if (turnBattleAction.IsActionValid()){
                
                actionfx = Instantiate(turnBattleAction.actor.placeHolder_fx, fxParentTransform);
                yield return new WaitForSeconds(seconds * 0.8f);
                Destroy(actionfx);
            }else{
                //invalid action
            }

            enemyUI.UpdatePartyUI();
            partyUI.UpdatePartyUI();

            yield return new WaitForSeconds(seconds*0.8f);
            Destroy(actionfx);
        }
        if(battleManager.GetBattleState() ==BattleState.PLAYERTURN){
            SetActionDescriptionText("Your Turn", true);
        }else if(battleManager.GetBattleState() ==BattleState.WON){
            SetActionDescriptionText("You Won", true);
            executeButtonText.GetComponent<TypewriterEffect>().Run("CONTINUE", executeButtonText);
        }
        else if(battleManager.GetBattleState() ==BattleState.LOST){
            SetActionDescriptionText("Game Over", true);
        }
    }

    public void SetActionDescriptionText(string txt, bool useTypeWriterEffect){
        if (useTypeWriterEffect){
            actionDescriptionText.GetComponent<TypewriterEffect>().Run(txt,actionDescriptionText);
        }else{
            actionDescriptionText.text = txt;
        }
    }
    public void SetExecuteButtonText(string txt){

        if(executeButtonText.text!=txt){
            executeButtonText.GetComponent<TypewriterEffect>().Run(txt, executeButtonText);
        }
       
    }
}
