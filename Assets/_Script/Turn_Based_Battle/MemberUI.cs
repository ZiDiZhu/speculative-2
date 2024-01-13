using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// to be attacked to each member's UI in the battle scene
public class MemberUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Character member;

    public Image outline; //to indicate which member is selected
    [SerializeField]private Image portrait; 
    [SerializeField]private TMP_Text memberName;
    [SerializeField]private TMP_Text memberHP;
    [SerializeField]private TMP_Text memberMP;
    public Slider hpSlider;
    public Slider mpSlider;

    public bool isSelected = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetMember(Character member)
    {
        this.member = member;
        SetMemberUI(member);
    }

    //when the member is clicked, select the member
    public void OnClick()
    {

        if(isSelected)
        {
            isSelected = false;
            outline.enabled = false;
            BattleSystem.instance.selectedMember = null;
            ActionPanelUI.instance.ClearActionPanel();
        }
        else{
            foreach (Transform child in transform.parent)
            {
                MemberUI memberUI = child.GetComponent<MemberUI>();
                if (memberUI != null)
                {
                    memberUI.isSelected = false;
                    memberUI.outline.enabled = false;
                }
            }
            isSelected = true;
            outline.enabled = true;
            BattleSystem.instance.selectedMember = member;
            ActionPanelUI.instance.SetActionPanelUI(member.actions);
        }
        ActionPanelUI.instance.actionDescription.text = "";

    }

    public void SetMemberUI(Character member)
    {
        //portrait.sprite = member.portrait;
        memberName.text = member.characterName;
        memberHP.text = "HP: "+member.currentHP.ToString()+"/"+member.maxHP.ToString();
        memberMP.text = "MP: "+ member.currentMP.ToString()+"/"+member.maxMP.ToString();
        hpSlider.maxValue = member.maxHP;
        hpSlider.value = member.currentHP;
        mpSlider.maxValue = member.maxMP;
        mpSlider.value = member.currentMP;
    }

    //hover to show the outline
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected)outline.enabled = false;
    }

    //update the UI when the member is attacked



}
