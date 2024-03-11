using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalUI : MonoBehaviour
{

    public GameObject AminoAcid, FattyAcid, Monosachirides; //for display

    

    public void DisplayGoalNutrient(NutrientType nutrientType)
    {
        switch (nutrientType)
        {
            case NutrientType.AMINO_ACIDS:
                AminoAcid.GetComponent<Image>().color = Color.white;
                FattyAcid.GetComponent<Image>().color = Color.gray;
                Monosachirides.GetComponent<Image>().color = Color.gray;
                break;
            case NutrientType.FATTY_ACID:   
                AminoAcid.GetComponent<Image>().color = Color.gray;
                FattyAcid.GetComponent<Image>().color = Color.white;
                Monosachirides.GetComponent<Image>().color = Color.gray;
                break;
            case NutrientType.MONOSACCHARIDES:
                AminoAcid.GetComponent<Image>().color = Color.gray;
                FattyAcid.GetComponent<Image>().color = Color.gray;
                Monosachirides.GetComponent<Image>().color = Color.white;
                break;
        
        }
    }

    


}
