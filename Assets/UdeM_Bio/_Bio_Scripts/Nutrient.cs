using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NutrientType{NONE, MONOSACCHARIDES, AMINO_ACIDS, FATTY_ACID }
public enum NutrientName{NONE, GLUCOSE, FRUCTOSE, GALACTOSE, RIBOSE, DEOXYRIBOSE, VALINE, ALALINE, GLYCINE, CAPRIC_ACID, MYRISTIC_ACID, PALMITOLEIC_ACID }

public class Nutrient : MonoBehaviour
{
    public NutrientType nutrientType;
    public NutrientName NutrientName;
    public Sprite sprite;
    public AudioClip audioClip;

    public void CopyNutrient(Nutrient nutrient)
    {
        this.nutrientType = nutrient.nutrientType;
        this.NutrientName = nutrient.NutrientName;
        this.sprite = nutrient.sprite;
        this.audioClip = nutrient.audioClip;
    }

}


