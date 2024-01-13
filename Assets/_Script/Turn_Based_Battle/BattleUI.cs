using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public BattleSystem battleSystem;
    
    public PartyUI partyUI;
    public PartyUI enemyUI;

    public ActionPanelUI actionPanelUI;


    private void Awake()
    {
        if(battleSystem==null)battleSystem = BattleSystem.instance;
    }


    private void Start()
    {
        partyUI.SetParty(battleSystem.partyMembers);
        enemyUI.SetParty(battleSystem.enemies);
        actionPanelUI.SetActionPanelUI(battleSystem.partyMembers[0].actions);
    }



}
