using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NutrientType{NONE, MONOSACCHARIDES, AMINO_ACIDS, FATTY_ACID }
public enum NutrientName{NONE, GLUCOSE, FRUCTOSE, GALACTOSE, RIBOSE, MANNOSE, VALINE, ALALINE, GLYCINE, SERINE, CAPROIC_ACID, CAPRIC_ACID, MYRISTIC_ACID, PALMITIC_ACID , PALMITOLEIC_ACID,LAURIC_ACID, LAUROLEIC_ACID }

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


