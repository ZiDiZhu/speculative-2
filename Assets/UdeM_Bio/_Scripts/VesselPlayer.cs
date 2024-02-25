using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NutrientTypeObjective
{
    List<NutrientType> ingredientsList = new List<NutrientType>();
    public int currentIndex = 0;

}

public class VesselPlayer : MonoBehaviour
{   
    int aminoAcidCount = 0;
    int fattyAcidCount = 0;
    int MonosachirideCount = 0;
    int totalCount = 0;
    int correctCount = 0;
    
    public int goalCorrectCount = 10;
    public GameObject endScreen;
    public GameObject pauseScreen;

    public TMP_Text playerText, aminoAcidCountText, fattyAcidCountText, MonosachirideCountText, accuracyText;
    public float textDuration = 1.0f;
    public AudioSource SFXAudioSource;

    public ComboLightUI comboLightUI;
    public GoalUI goalUI;

    public List<NutrientType> ingredientsList = new List<NutrientType>();

    public NutrientType currentTargetNutrientType;
    public AudioClip goodSFX, badSFX, aminoSFX, monoSFX, fattySFX;

    private void Start()
    {
        PauseGame();
        currentTargetNutrientType = GetRandomType();
        goalUI.DisplayGoalNutrient(currentTargetNutrientType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public NutrientType GetRandomType(){
        return ingredientsList[UnityEngine.Random.Range(0, ingredientsList.Count)];
    }

    public void EatNutrient(Nutrient nutrient){
        

        switch(nutrient.nutrientType){
            case (NutrientType.AMINO_ACIDS):
                aminoAcidCount++;
                aminoAcidCountText.text = aminoAcidCount.ToString();
                SFXAudioSource.PlayOneShot(aminoSFX);
                break;  
            case (NutrientType.MONOSACCHARIDES):
                MonosachirideCount++;
                MonosachirideCountText.text = MonosachirideCount.ToString();
                SFXAudioSource.PlayOneShot(monoSFX);
                break;
            case (NutrientType.FATTY_ACID):
                fattyAcidCount++;
                fattyAcidCountText.text = fattyAcidCount.ToString();
                SFXAudioSource.PlayOneShot(fattySFX);
                break;
        
        }

        totalCount++;

        if (nutrient.nutrientType != currentTargetNutrientType)
        {
            comboLightUI.DecreaseLevel();
            StartCoroutine(WaitAndPrint("Wrong TYPE", textDuration));
            SFXAudioSource.PlayOneShot(badSFX);

        }
        else
        {
            correctCount++;
            comboLightUI.IncreaseLevel();
            StartCoroutine(WaitAndPrint("YAY", textDuration));
            SFXAudioSource.PlayOneShot(goodSFX);
            currentTargetNutrientType = GetRandomType();
            goalUI.DisplayGoalNutrient(currentTargetNutrientType);
            WaitAndPrint("Collect A "+ currentTargetNutrientType.ToString(), textDuration);
            CheckWinCondition();
        }

        accuracyText.text = "Accuracy: " + ((float)correctCount / (float)totalCount) * 100 + "%";

        playerText.text = "";
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }   
    
    void CheckWinCondition()
    {
        if (correctCount >= goalCorrectCount)
        {
            EndGame();
        }
    }   



    public void EndGame()
    {
        endScreen.SetActive(true);
        endScreen.transform.GetChild(0).GetComponent<Image>().sprite = comboLightUI.lights[comboLightUI.currentLevel].sprite;
        Time.timeScale = 0;
    }
    

    IEnumerator WaitAndPrint(string txt,float waitTime)
    {
        playerText.GetComponent<TypewriterEffect>().Run(txt, playerText);
        yield return new WaitForSeconds(waitTime);
        playerText.text = "";
    }

}
