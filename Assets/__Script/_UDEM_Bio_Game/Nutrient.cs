using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NutrientType{NONE, MONOSACCHARIDES, AMINO_ACIDS, FATTY_ACID }
public enum NutrientName{NONE, GLUCOSE, FRUCTOSE, GALACTOSE, RIBOSE, DEOXYRIBOSE, VALINE, ALALINE, GLYCINE, CAPRIC_ACID, MYRISTIC_ACID, PALMITOLEIC_ACID }

public class Nutrient : MonoBehaviour
{
    public NutrientType nutrientType;
    public NutrientName NutrientName;
    public AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            VesselPlayer vesselPlayer = other.gameObject.GetComponent<VesselPlayer>();
            StartCoroutine(vesselPlayer.EatNutrient(this));
            Destroy(gameObject);
        }
        
    }

}


