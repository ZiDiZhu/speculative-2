using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public BattleSystem battleSystem;
    
    public PartyUI partyUI;
    public PartyUI enemyUI;


    private void Awake()
    {
        if(battleSystem==null)battleSystem = BattleSystem.instance;
    }


    private void Start()
    {
        partyUI.SetParty(battleSystem.partyMembers);
    }



}
