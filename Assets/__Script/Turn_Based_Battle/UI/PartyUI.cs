using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUI : MonoBehaviour
{

    public GameObject memberUIPrefab;
    public List<MemberUI> memberUIs = new List<MemberUI>();
    [SerializeField]private List<Character> partyMembers = new List<Character>();
    public PartyType partyType;


    public void AssignMembers(List<Character> charas){
        partyMembers = charas;
    }
    
    public void SetParty()
    {
        ClearPartyUI();
        foreach (Character member in partyMembers)
        {
            GameObject memberUIObject = Instantiate(memberUIPrefab, transform);
            MemberUI memberUI = memberUIObject.GetComponent<MemberUI>();
            memberUI.SetMemberUI(member);
            memberUIs.Add(memberUI);

            if(memberUI.member.characterState!=CharacterState.DEAD){
                if (partyType == PartyType.ENEMY)
                {
                    memberUI.GetComponent<Button>().onClick.AddListener(memberUI.EnemyMemberOnClick);
                    memberUI.SetStateText(member.characterState.ToString());
                }
                else if (partyType == PartyType.PLAYER)
                {
                    memberUI.GetComponent<Button>().onClick.AddListener(memberUI.PartyMemberOnClick);
                    memberUI.SetStateText("Ready");
                }
            }

        }
    }

    public void ClearPartyUI()
    {
        memberUIs.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
    }

    public void DisableSelection()
    {
        foreach (MemberUI memberUI in memberUIs)
        {
            memberUI.GetComponent<Button>().interactable = false;
            memberUI.Deselect();
        }
    }

    public void EnableSelection()
    {
        foreach (MemberUI memberUI in memberUIs)
        {
            memberUI.GetComponent<Button>().interactable = true;
        }
    }

    public void DeselectAll(){
        foreach (MemberUI memberUI in memberUIs)
        {
            memberUI.Deselect();
        }
    }
    
}
