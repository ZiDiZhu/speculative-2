using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//this script is attached to the ActionUI prefab, which displays one action from the list of actions a character can perform
//its parent is the ActionPanel
public class ActionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public ActionData action;
    public TMP_Text actionName;
    public TMP_Text actionCost;
    public GameObject highlight; //enable this to highlight selection
    public bool isSelected = false;


    public void OnClick()
    {
        if (isSelected)
        {
            Deselect();
            BattleUI.instance.selectedAction = null;
            BattleUI.instance.selectedTarget = null;
            BattleUI.instance.selectedActor.hasSelectedAction = false;
            BattleUI.instance.selectedActor.SetActionText("[-Select Action-]");
            BattleUI.instance.battleSelectionState = BattleUI.BattleSelectionState.ACTION;
            ActionPanelUI.instance.actionDescription.text = "";
            BattleUI.instance.enemyUI.DisableSelection();
            
        }
        else
        {
            Select();
            BattleUI.instance.selectedAction = action;
            BattleUI.instance.selectedActor.SetActionText("[-Select Target-]");
            ActionPanelUI.instance.actionDescription.text = action.actionDescription; 
            BattleUI.instance.battleSelectionState = BattleUI.BattleSelectionState.TARGET;
            BattleUI.instance.enemyUI.EnableSelection();
        }
    }

    public void Deselect()
    {
        isSelected = false;
        highlight.SetActive(false);
    }

    public void Select()
    {
        foreach (Transform child in transform.parent)
        {
            ActionUI actionUI = child.GetComponent<ActionUI>();
            if (actionUI != null)
            {
                actionUI.isSelected = false;
                actionUI.highlight.SetActive(false);
            }
        }
        isSelected = true;
        highlight.SetActive(true);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(true);
    }
    
    

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) highlight.SetActive(false);
    }

    public void SetAction(ActionData action){
        this.action = action;
        actionName.text = action.actionName;
        actionCost.text = "-" + action.mpCost.ToString();
    }
}
