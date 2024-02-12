using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class NutrientTypeObjective
{
    List<NutrientType> ingredientsList = new List<NutrientType>();
    public int currentIndex = 0;

}

public class VesselPlayer : MonoBehaviour
{
    public TMP_Text playerText;
    public float textDuration = 1.0f;
    public AudioSource SFXAudioSource;

    public ComboLightUI comboLightUI;
    public GoalUI goalUI;

    public List<NutrientType> ingredientsList = new List<NutrientType>();

    public NutrientType goalNutrient;
    public AudioClip goodSFX, badSFX, aminoSFX, monoSFX, fattySFX;

    private void Start()
    {
        goalNutrient = GetRandomType();
        goalUI.DisplayGoalNutrient(goalNutrient);
    }

    public NutrientType GetRandomType(){
        return ingredientsList[UnityEngine.Random.Range(0, ingredientsList.Count)];
    }

    public void EatNutrient(Nutrient nutrient){
        StartCoroutine(Eat(nutrient));
        
    }


    public IEnumerator Eat(Nutrient nutrient){

        switch (nutrient.nutrientType){
            case NutrientType.AMINO_ACIDS:SFXAudioSource.PlayOneShot(aminoSFX);break;
            case NutrientType.MONOSACCHARIDES:SFXAudioSource.PlayOneShot(monoSFX);break;
            case NutrientType.FATTY_ACID:SFXAudioSource.PlayOneShot(fattySFX);break;
            
        }

        if (nutrient.nutrientType != goalNutrient)
        {
            comboLightUI.DecreaseLevel();
            playerText.GetComponent<TypewriterEffect>().Run("Wrong TYPE",playerText);
            SFXAudioSource.PlayOneShot(badSFX);
            
        }
        else
        {
            comboLightUI.IncreaseLevel();
            playerText.GetComponent<TypewriterEffect>().Run("YAY",playerText);
            SFXAudioSource.PlayOneShot(goodSFX);
            goalNutrient = GetRandomType();
            goalUI.DisplayGoalNutrient(goalNutrient);

        }
        
        yield return new WaitForSeconds(textDuration);
        playerText.text = "";

        

    }

}
