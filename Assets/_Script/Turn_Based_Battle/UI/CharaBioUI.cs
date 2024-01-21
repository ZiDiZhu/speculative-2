using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharaBioUI : MonoBehaviour
{

    [SerializeField]private TMP_Text charaName;
    [SerializeField]private TMP_Text charaDescription;
    [SerializeField]private Image charaImage;
    // Start is called before the first frame update

    private Character character;

    public void SetCharacter(Character character)
    {
        this.character = character;
        charaName.text = character.characterName;
        charaDescription.text = character.description;
        charaImage.sprite = character.pfpSprite;
    }
    
}
