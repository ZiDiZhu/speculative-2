using System;
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

    public BattleSkill action;
    public TMP_Text actionName;
    public TMP_Text actionCost;
    public TMP_Text targetTxt;
    public GameObject highlight; //enable this to highlight selection
    public bool isSelected = false;
    

    public void OnClick()
    {
        
        BattleUI.instance.ActionUIOnClick(this);
        
        //select this action
        isSelected = true;
        highlight.SetActive(true);
        
        //deselect all other actions
        foreach (Transform child in transform.parent)
        {
            ActionUI actionUI = child.GetComponent<ActionUI>();
            if (actionUI != null)
            {
                actionUI.isSelected = false;
                actionUI.highlight.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(true);
    }
    
    

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) highlight.SetActive(false);
    }

    public void SetAction(BattleSkill action){
        this.action = action;
        GetComponent<Button>().onClick.AddListener(OnClick);
        actionName.text = action.skillName;
        string targetString = "Cost: ";
        targetString = "-" + action.mpCost.ToString()+" mp";
        if(action.addHealing<0){
            targetString += "\n" +action.addHealing +"hp";
        }
        actionCost.text = targetString;
        targetTxt.text = "target type: "+ action.targetType.ToString();
    }
}
