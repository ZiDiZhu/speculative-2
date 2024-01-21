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

    [SerializeField] private Image deathIndicator; //enable this when the character is dead
    [SerializeField] private GameObject readyIndicator; //set this active when the character has selected an action]

    public bool isSelected = false;
    public bool hasSelectedAction = false;

    private void Start()
    {
 
    }

    public void SetMember(Character member)
    {
        this.member = member;
        SetMemberUI(member);
    }
    
    
    //invokes the BattleUI's MemberUIOnClick function when MemberUI is clicked
    public void PartyMemberOnClick()
    {   
        BattleUI.instance.MemberUIOnClick(this);
        
    }

    public void EnemyMemberOnClick()
    {
        BattleUI.instance.EnemyMemberOnClick(this);
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
        if(readyIndicator!=null)readyIndicator.SetActive(false);
        if(member.pfpSprite!=null)portrait.sprite = member.pfpSprite;
        memberName.text = member.characterName;
        memberHP.text = "HP: "+member.currentHP.ToString()+"/"+member.maxHP.ToString();
        memberMP.text = "MP: "+ member.currentMP.ToString()+"/"+member.maxMP.ToString();
        hpSlider.maxValue = member.maxHP;
        hpSlider.value = member.currentHP;
        mpSlider.maxValue = member.maxMP;
        mpSlider.value = member.currentMP;
        
        if(member.characterState==CharacterState.DEAD){
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().enabled = false;
            GetComponent<Button>().interactable = false;    
            SetActionText("DEAD");
            if(deathIndicator!=null)deathIndicator.enabled = true;
        }else{
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().enabled = true;
            GetComponent<Button>().interactable = true;
            if (deathIndicator != null) deathIndicator.enabled = false;
        }
    
    }

    public void SetActionText(string txt){
        actionText.text = txt;
        if(hasSelectedAction){
            if (readyIndicator != null) readyIndicator.SetActive(true);
        }else{
            if (readyIndicator != null) readyIndicator.SetActive(false);
        }
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
