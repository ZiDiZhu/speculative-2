using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUI : MonoBehaviour
{

    public GameObject memberUIPrefab;
    public List<MemberUI> memberUIs = new List<MemberUI>();
    public PartyType partyType;

    public void SetParty(List<Character> partyMembers)
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
                    memberUI.SetStateText("ENEMY");
                }
                else if (partyType == PartyType.PLAYER)
                {
                    memberUI.GetComponent<Button>().onClick.AddListener(memberUI.PartyMemberOnClick);
                    memberUI.SetStateText("[-Select Action-]");
                }
            }

        }
    }

    public void ClearPartyUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        memberUIs.Clear();
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