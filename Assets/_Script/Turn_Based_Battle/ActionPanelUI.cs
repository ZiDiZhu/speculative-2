using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//to be attached to the ActionPanel in the battle scene, which is the parent of the ActionUIs
public class ActionPanelUI : MonoBehaviour
{
    public static ActionPanelUI instance { get; private set; } //singleton
    public List<ActionUI>actionUIs = new List<ActionUI>();
    public GameObject actionUIPrefab;
    public TMP_Text actionDescription;

    private void Awake()
    {
        if(instance==null)instance = this;  
    }

    public void SetActionPanelUI(List<ActionData> actions)
    {
        ClearActionPanel();
        foreach (ActionData action in actions)
        {
            GameObject actionUIObject = Instantiate(actionUIPrefab, transform);
            ActionUI actionUI = actionUIObject.GetComponent<ActionUI>();
            actionUI.SetAction(action);
            actionUI.GetComponent<Button>().onClick.AddListener(actionUI.OnClick);
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
