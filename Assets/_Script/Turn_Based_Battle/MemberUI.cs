using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// to be attacked to each member's UI in the battle scene
public class MemberUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Character member;

    public Image outline; //to indicate which member is selected
    [SerializeField]private Image portrait; 
    [SerializeField]private TMP_Text memberName;
    [SerializeField]private TMP_Text memberHP;
    [SerializeField]private TMP_Text memberMP;
    [SerializeField]private TMP_Text actionText; //to display which action is selected
    public Slider hpSlider;
    public Slider mpSlider;

    public bool isSelected = false;
    public bool hasSelectedAction = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
 
    }

    public void SetMember(Character member)
    {
        this.member = member;
        SetMemberUI(member);
    }
    
    
    //invokes the BattleUI's MemberUIOnClick function when MemberUI is clicked
    public void OnClick()
    {
        BattleUI.instance.MemberUIOnClick(this);

    }

    public void Deselect(){
        isSelected = false;
        outline.enabled = false;
        BattleSystem.instance.selectedMember = null;
        BattleUI.instance.ClearActionPanel();
    }

    public void Select(){
        isSelected = true;
        outline.enabled = true;
    }


    public void SetMemberUI(Character member)
    {
        if(member.pfpSprite!=null)portrait.sprite = member.pfpSprite;
        memberName.text = member.characterName;
        memberHP.text = "HP: "+member.currentHP.ToString()+"/"+member.maxHP.ToString();
        memberMP.text = "MP: "+ member.currentMP.ToString()+"/"+member.maxMP.ToString();
        hpSlider.maxValue = member.maxHP;
        hpSlider.value = member.currentHP;
        mpSlider.maxValue = member.maxMP;
        mpSlider.value = member.currentMP;
    }

    public void SetActionText(string txt){
        actionText.text = txt;
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




}
