using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    public List<MemberUI> memberUIs = new List<MemberUI>();
    
    public void SetParty(List<Character> partyMembers)
    {
        foreach (Transform child in transform)
        {
            MemberUI memberUI = child.GetComponent<MemberUI>();
            if (memberUI != null)
            {
                memberUIs.Add(memberUI);
            }
        }

        for (int i = 0; i < partyMembers.Count; i++)
        {
            memberUIs[i].SetMember(partyMembers[i]);
        }
    }
    
}
