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
                if(selectedAction!=null)executeBtn.interactable = true;
                SetExecuteButtonText("Select Action");
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
                SetExecuteButtonText("Not Enough MP");
            }
            else
            {
                executeBtn.interactable = true;
                selectedAction = actionUI.action;
                SetExecuteButtonText("Select " + selectedAction.actionName);
            }
        }
        SetBattleSelectionState(BattleSelectionState.ACTION);
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
                selectedActor.Prompt(selectedAction.actionName + "- Select a Target");
                break;
            case (TargetType.ALL_OPPONENT):
                actorTxt = selectedAction.actionName + " Targeting all Ennemies";
                actionDescription = selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on all party members.";
                TargetsSelected(enemyUI.GetMemberUIs());
                ConfirmTurnActionSelection(actorTxt, actionDescription);
                break;
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
        selectedActor.Prompt(actorTxt);
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
        selectedTarget = targetMemberUI;
        List<Character> target = new List<Character>();
        target.Add(selectedTarget.member);
        TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, target);
        battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(), turnBattleAction);

        ConfirmTurnActionSelection(selectedAction.actionName + " Targeting " + selectedTarget.member.characterName, selectedActor.member.characterName + " will perform " + selectedAction.actionName + " on " + selectedTarget.member.characterName);
    }

    public void TargetsSelected(List<MemberUI> targetUI){

        ClearSelectedActionForCharacter(selectedActor.member); //remove indicator for previeus target
        List<Character> target = new List<Character>();
        foreach (MemberUI ui in targetUI)
        {
            target.Add(ui.member);
        }
        TurnBattleAction turnBattleAction = new TurnBattleAction(selectedActor.member, selectedAction, target);
        battleManager.AddTurnBattleAction(selectedActor.member.GetPartyType(), turnBattleAction);
    }

    void ClearSelectedActionForCharacter(Character character)
    {
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

        playerParty.SortTurnBattleActionsBySpeed();
        enemyParty.SortTurnBattleActionsBySpeed();

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
            if(turnParty.GetPartyType()==PartyType.PLAYER)actorUI = partyUI.GetMemberUI(turnBattleAction.actor);
            else actorUI = enemyUI.GetMemberUI(turnBattleAction.actor);
            actorUI.Select();
            //highlight target(s)
            foreach (Character target in turnBattleAction.targets)
            {
                if(target.GetPartyType()==PartyType.ENEMY)targetUI = enemyUI.GetMemberUI(target);
                else targetUI = partyUI.GetMemberUI(target);
                targetUI.Select();
            }

            actorUI.Select();

            //Executes the action and returns a string with the result
            output += turnBattleAction.Execute() + "\n";
            Debug.Log(output);

            actorUI.hasSelectedAction = false;
            

            SetActionDescriptionText(output, true);
            
         

            if (turnBattleAction.actionResult!=TurnBattleAction.ACTION_RESULT.INVALID){
                int count = 0;
                foreach (Character target in turnBattleAction.targets)
                {
                    
                    if(target.GetPartyType()==PartyType.ENEMY)targetUI = enemyUI.GetMemberUI(target);
                    else targetUI = partyUI.GetMemberUI(target);
                    StartCoroutine(actorUI.TakeActionAnimation(targetUI,fxParentTransform));
                    StartCoroutine(targetUI.OnHPChange(turnBattleAction.targetHpChange[count]));
                    yield return new WaitForSeconds(0.5f);
                    count++;
                }
            }else{
                //invalid action
            }

            enemyUI.UpdatePartyUI();
            partyUI.UpdatePartyUI();

            actorUI.Deselect();
            foreach (Character target in turnBattleAction.targets)
            {
                if(target.GetPartyType()==PartyType.ENEMY)targetUI = enemyUI.GetMemberUI(target);
                else targetUI = partyUI.GetMemberUI(target);
                targetUI.Deselect();
            }

            //remove the action from queue            
            turnParty.turnBattleActions.RemoveAt(0);
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
