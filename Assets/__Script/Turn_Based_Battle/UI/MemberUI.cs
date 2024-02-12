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

    [SerializeField] private GameObject actorsContainer; //the container that holds all the other charas that will take action on this character
    [SerializeField] private GameObject actorTemplate; //template - parent has image whic disply pfp of the "actor" that will take action on this character", its child has the tmptext that shows the action

    
    
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
        UpdateMemberUI(member.name + ": Ready");
    }



    public void UpdateMemberUI()
    {
        UpdateHPandMP();
        if (member.characterState == CharacterState.DEAD) OnCharacterKilled();
        

    }

    public void UpdateMemberUI(string txt){
        UpdateMemberUI();
        stateText.GetComponent<TypewriterEffect>().Run(txt, stateText);
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


    public void AddActor(Character actor, BattleSkill action){
        
        GameObject actorUI = Instantiate(actorTemplate,actorsContainer.transform);
        actorUI.SetActive(true);
        actorUI.GetComponent<Image>().sprite = actor.pfpSprite;
        actorUI.GetComponentInChildren<TMP_Text>().text = action.actionType.ToString();
    }

    //WARNING: using the sprite to identify the actor rn
    public void RemoveActor(Character actor){
        foreach(Transform child in actorsContainer.transform){
            if(child.GetComponent<Image>().sprite == actor.pfpSprite){
                Destroy(child.gameObject);
            }
        }
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
