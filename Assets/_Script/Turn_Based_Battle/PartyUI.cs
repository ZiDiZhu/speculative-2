using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{

    public GameObject memberUIPrefab;
    public List<MemberUI> memberUIs = new List<MemberUI>();
    

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
    
}
