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
    //public SpriteRenderer charaSpriteRenderer;    
    [SerializeField]private Image charaBodySprite,targetBodySprite; //to display the selected character's sprite
    private TMP_Text executeButtonText;
    
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
        executeButtonText = executeBtn.GetComponentInChildren<TMP_Text>();
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
                SetExecuteButtonText("Select Target");
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
        if (selectedActor.member.GetPartyType() == PartyType.ENEMY)
        {
            CheckIfCanExecuteTurn();
        }else if(selectedActor.member.GetPartyType()==PartyType.PLAYER){
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

    public void ActorSelected(MemberUI memberUI){
       
        partyUI.DeselectAll();
        memberUI.Select();
        characterUI.SetCharacter(memberUI.member);
        selectedActor = memberUI;
        battleSelectionState = BattleSelectionState.ACTION;
        SetActionPanel(memberUI.member.actions);
        charaBodySprite.sprite = memberUI.member.fullBodySprite_Normal;
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
            SetExecuteButtonText("EXECUTE TURN");
        }
        else
        {
            executeBtn.interactable = false;
            SetExecuteButtonText("YOUR TURN");
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
                executeButtonText.GetComponent<TypewriterEffect>().Run("FIGHT", executeButtonText);
                StartCoroutine(ExecuteTurnWithDelay(battleTurnDuration));   
                bool gameEnd = battleManager.IfBattleEnded();
                if (!gameEnd)
                {
                    //add enemy actions
                    enemyParty.AddActionsForAllCharacters(playerParty);
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
                break;
        }
    }

    //equivalent to ExecuteBtnOnClick() in BattleManager, but with a delay
    //remember to update both if you change one
    public IEnumerator ExecuteTurnWithDelay(float seconds){
        List<TurnBattleAction> playerActions = playerParty.turnBattleActions;
        List<TurnBattleAction> enemyActions = enemyParty.turnBattleActions;
        targetBodySprite.gameObject.SetActive(true);
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

            targetBodySprite.gameObject.GetComponentInChildren<TMP_Text>().text = "";
            targetBodySprite.color = Color.white;
            charaBodySprite.color = Color.white;
        
            //This is gonna be the action that is executed
            TurnBattleAction turnBattleAction = turnParty.turnBattleActions[0];

            //If actor is dead
            if(turnBattleAction.actor.characterState == CharacterState.DEAD){
                charaBodySprite.color = Color.red;
            }

            //Shows actor and Target sprites
            charaBodySprite.sprite = turnBattleAction.actor.fullBodySprite_Normal;
            targetBodySprite.sprite = turnBattleAction.target.fullBodySprite_Normal;

            //Get the target's HP before the action
            int targetHpBefore = turnBattleAction.target.GetCurrentHP();        

            //Executes the action and returns a string with the result
            output +=battleManager.TestExecuteTurnBattleAction(turnBattleAction) + "\n";

            
            //Get the target's HP after the action and display the damage/heal
            //TODO: improve the logic of displaying the action taking effect
            //Currently, it can only tell if action was valid and if it was a heal or damage
            
            int targetHpAfter = turnBattleAction.target.GetCurrentHP(); 
            int hpChange = targetHpAfter - targetHpBefore;
            
            bool actionWasValid = (hpChange != 0);
            if (actionWasValid)
            {
                if (hpChange > 0)
                {
                    targetBodySprite.gameObject.GetComponentInChildren<TMP_Text>().color = Color.green; //its a heal
                }
                else
                {
                    targetBodySprite.gameObject.GetComponentInChildren<TMP_Text>().color = Color.red; //its a damage
                }
            }
            else
            {
                targetBodySprite.gameObject.GetComponentInChildren<TMP_Text>().color = Color.white; //its a miss
            }

            //remove the action from queue            
            turnParty.turnBattleActions.RemoveAt(0);

            SetActionDescriptionText(output, true);
            
            yield return new WaitForSeconds(seconds * 0.2f);
            
            GameObject actionfx = null;
            charaBodySprite.sprite = turnBattleAction.actor.fullBodySprite_Action;

            if (actionWasValid){
                
                actionfx = Instantiate(turnBattleAction.actor.placeHolder_fx, fxParentTransform);
                yield return new WaitForSeconds(seconds * 0.8f);
                Destroy(actionfx);
            }

            if(targetHpAfter<=0){
                targetBodySprite.color = Color.red;
            }

            enemyUI.SetParty(allEnemyMembers);
            partyUI.SetParty(allPlayerMembers);
            targetBodySprite.gameObject.GetComponentInChildren<TMP_Text>().text = hpChange.ToString();

            yield return new WaitForSeconds(seconds*0.8f);
            Destroy(actionfx);
        }
        targetBodySprite.gameObject.SetActive(false);
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
            actionDescriptionText.GetComponent<TypewriterEffect>().Run(txt, actionDescriptionText);
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
