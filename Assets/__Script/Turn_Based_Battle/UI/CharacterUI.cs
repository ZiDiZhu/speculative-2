using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//to display the selected character's stats and equipment, etc
public class CharacterUI : MonoBehaviour
{
    public static CharacterUI instance { get; private set; } //singleton
    private Character character;
    private BattleUI battleUI;


    [SerializeField] private Button btn_bio, btn_stats, btn_equip, btn_log;
    [SerializeField] private GameObject bioPanel, statsPanel, equipPanel, logPanel;
    [SerializeField] private AttributeUI strengthUI, precisionUI, agilityUI, speedUI, luckUI;


    private void Awake()
    {
        if (instance == null) instance = this;
        btn_bio.onClick.AddListener(ShowBioPanel);
        btn_stats.onClick.AddListener(ShowStatsPanel);
        btn_equip.onClick.AddListener(ShowEquipPanel);
        btn_log.onClick.AddListener(ShowLogPanel);
    }

    // Start is called before the first frame update
    void Start()
    {
        HideAllPanels();
        battleUI = BattleUI.instance;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBioPanel(){
        HideAllPanels();
        bioPanel.SetActive(true);
    }
    public void ShowStatsPanel(){
        HideAllPanels();
        statsPanel.SetActive(true);
    }
    public void ShowEquipPanel(){
        HideAllPanels();
        equipPanel.SetActive(true);
    }
    public void ShowLogPanel(){
        HideAllPanels();
        logPanel.SetActive(true);
    }

    public void HideAllPanels(){
        bioPanel.SetActive(false);
        statsPanel.SetActive(false);
        equipPanel.SetActive(false);
        logPanel.SetActive(false);
    }

    public void SetCharacter(Character character){
        this.character = character;
        strengthUI.SetAttribute(character.strength, 10);
        precisionUI.SetAttribute(character.precision, 10);
        speedUI.SetAttribute(character.speed, 10);
        agilityUI.SetAttribute(character.agility, 10);
        luckUI.SetAttribute(character.luck, 10);
        bioPanel.GetComponent<CharaBioUI>().SetCharacter(character);
        
    }

}
