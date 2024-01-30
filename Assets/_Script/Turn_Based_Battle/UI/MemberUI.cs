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
    [SerializeField]private TMP_Text stateText; //to display what the member is doing. has typewriter effect.
    [SerializeField]private Slider hpSlider;
    [SerializeField]private  Slider mpSlider;

    
    [SerializeField] private Image deathIndicator; //enable this when the character is dead
    [SerializeField] private GameObject readyIndicator; //set this active when the character has selected an action]

    public bool isSelected = false;
    public bool hasSelectedAction = false;

    [SerializeField] private GameObject actorsContainer; //the container that holds all the other charas that will take action on this character
    [SerializeField] private GameObject actorTemplate; //template - parent has image whic disply pfp of the "actor" that will take action on this character", its child has the tmptext that shows the action

    private void Start()
    {
 
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
    }

    public void Select(){
        isSelected = true;
        outline.enabled = true;
    }


    public void SetMemberUI(Character member)
    {
        this.member = member;
        if (readyIndicator!=null)readyIndicator.SetActive(false);
        if(member.pfpSprite!=null)portrait.sprite = member.pfpSprite;
        memberName.text = member.characterName;
        memberHP.text = "HP: "+member.GetCurrentHP().ToString()+"/"+member.GetCurrentHP().ToString();
        memberMP.text = "MP: "+ member.GetCurrentHP().ToString()+"/"+member.GetCurrentHP().ToString();
        hpSlider.maxValue = member.GetCurrentHP();
        hpSlider.value = member.GetCurrentHP();
        mpSlider.maxValue = member.GetCurrentHP();
        mpSlider.value = member.GetCurrentHP();
        if(member.characterState==CharacterState.DEAD){
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().enabled = false;
            GetComponent<Button>().interactable = false;    
            SetStateText("DEAD");
            if(deathIndicator!=null)deathIndicator.enabled = true;
            portrait.color = Color.gray;
        }else{
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().enabled = true;
            GetComponent<Button>().interactable = true;
            if (deathIndicator != null) deathIndicator.enabled = false;
        }
    
    } 

    
    public void SetStateText(string txt){
        
        stateText.GetComponent<TypewriterEffect>().Run(txt,stateText); 
        if(hasSelectedAction){
            if (readyIndicator != null) readyIndicator.SetActive(true);
        }else{
            if (readyIndicator != null) readyIndicator.SetActive(false);
        }
    }



    public void AddActor(Character actor, BattleAction action){
        
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

    public void ClearActors(){
        foreach(Transform child in actorsContainer.transform){
            if(child.gameObject.activeSelf)Destroy(child.gameObject);
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
