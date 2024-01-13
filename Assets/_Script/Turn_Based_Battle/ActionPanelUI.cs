using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//to be attached to the ActionPanel in the battle scene, which is the parent of the ActionUIs
public class ActionPanelUI : MonoBehaviour
{
    
    public List<ActionUI>actionUIs = new List<ActionUI>();
    public GameObject actionUIPrefab;
    public TMP_Text actionDescription;
    public Button confirmActionButton;

    private void Awake()
    {
        confirmActionButton.onClick.AddListener(OnConfirmAction);
    }
    
    public void OnConfirmAction()
    {
        foreach (ActionUI actionUI in actionUIs)
        {
            if (actionUI.isSelected)
            {
                //BattleSystem.instance.turnActions.Add(actionUI.action);
            }
        }
    }

    public void SetActionPanelUI(List<ActionData> actions)
    {
        ClearActionPanel();
        foreach (ActionData action in actions)
        {
            GameObject actionUIObject = Instantiate(actionUIPrefab, transform);
            ActionUI actionUI = actionUIObject.GetComponent<ActionUI>();
            actionUI.SetAction(action);
            actionUIs.Add(actionUI);
        }
    }

    public void ClearActionPanel(){
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        actionUIs.Clear();
    }

}
