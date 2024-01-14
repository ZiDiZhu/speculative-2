using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{

    public GameObject memberUIPrefab;
    public List<MemberUI> memberUIs = new List<MemberUI>();
    public enum PartyType { PARTY, ENEMY };
    public PartyType partyType;

    public void SetParty(List<Character> partyMembers)
    {
        ClearPartyUI();
        foreach (Character member in partyMembers)
        {
            GameObject memberUIObject = Instantiate(memberUIPrefab, transform);
            MemberUI memberUI = memberUIObject.GetComponent<MemberUI>();
            memberUI.SetMember(member);
            memberUIs.Add(memberUI);
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
            memberUI.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void EnableSelection()
    {
        foreach (MemberUI memberUI in memberUIs)
        {
            memberUI.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void DeselectAll(){
        foreach (MemberUI memberUI in memberUIs)
        {
            memberUI.Deselect();
        }
    }
    
}
