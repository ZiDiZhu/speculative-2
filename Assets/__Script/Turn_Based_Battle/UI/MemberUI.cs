using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// to be attacked to each member's UI in the battle scene
public class MemberUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character member;

    //public Image outline; //to indicate which member is selected
    [SerializeField]private Image portrait; 
    [SerializeField]private TMP_Text memberName;
    [SerializeField]private TMP_Text memberHP;
    [SerializeField]private TMP_Text memberMP;
    [SerializeField]private TMP_Text stateText; //to display what the member is doing. has typewriter effect.
    [SerializeField]private TMP_Text hpChangeText;
    [SerializeField]private Slider hpSlider;
    [SerializeField]private  Slider mpSlider;

    public bool isSelected = false;
    public bool hasSelectedAction = false;

    
    
    //invokes the BattleUI's MemberUIOnClick function when MemberUI is clicked
    public void PartyMemberOnClick()
    {   
        BattleUI.instance.MemberUIOnClick(this);
        //Debug.Log("MemberUIOnClick");
    }
    public void EnemyMemberOnClick()
    {
        BattleUI.instance.EnemyMemberOnClick(this);
        //Debug.Log("EnemyMemberOnClick");
    }


    public void Deselect(){
        isSelected = false;
        if (member.characterState == CharacterState.DEAD)
        {
            portrait.color = Color.red;
        }
        else
        {
            portrait.color = Color.grey;
        }
    }
    public void Select(){
        isSelected = true;
        portrait.color = Color.white;
        if(member.characterState == CharacterState.DEAD){
            portrait.color = Color.red;
        }else{
            portrait.color = Color.white;
        }
    }


    public void SetMemberUI(Character member)
    {
        this.member = member;
        portrait.sprite = member.fullBodySprite_Normal;
        memberName.text = member.characterName;
        Prompt(member.name + ": Ready");
        UpdateHPandMP();
    }

    public IEnumerator OnHPChange(int hpCHnage)
    {

        if (hpCHnage>=0)hpChangeText.color = Color.green;
        else hpChangeText.color = Color.red;

        hpChangeText.text = hpCHnage.ToString();
        yield return new WaitForSeconds(0.5f);
        hpChangeText.text = "";
        UpdateMemberUI();
    }


    public IEnumerator TakeActionAnimation(MemberUI targetUI, Transform fxtransform){
        GameObject actionfx = Instantiate(member.placeHolder_fx, targetUI.transform.position, Quaternion.identity, fxtransform);
        yield return new WaitForSeconds(0.5f);
        Destroy(actionfx);
    

    }


    public void UpdateMemberUI()
    {
        UpdateHPandMP();
        if (member.characterState == CharacterState.DEAD) OnCharacterKilled();
        
    }
    public void Prompt(string txt){
        stateText.GetComponent<TypewriterEffect>().Run(txt, stateText);
    }

    public void UpdateHPChangeText(int change){
        hpChangeText.text = change.ToString();
        if(change > 0){
            hpChangeText.color = Color.green;
        }else{
            hpChangeText.color = Color.red;
        }
    }


    void UpdateHPandMP(){
        memberHP.text = "HP: " + member.GetCurrentHP().ToString() + "/" + member.GetMaxHP().ToString();
        memberMP.text = "MP: " + member.GetCurrentMP().ToString() + "/" + member.GetMaxMP().ToString();
        hpSlider.maxValue = member.GetMaxHP();
        hpSlider.value = member.GetCurrentHP();
        mpSlider.maxValue = member.GetMaxMP();
        mpSlider.value = member.GetCurrentMP();
    }
    void OnCharacterKilled(){
        portrait.color = Color.red;
        stateText.GetComponent<TypewriterEffect>().Run("DEAD", stateText);
    }



    //hover to show the outline
    public void OnPointerEnter(PointerEventData eventData)
    {
        portrait.color = Color.white;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected)portrait.color = Color.grey;
    }




}
