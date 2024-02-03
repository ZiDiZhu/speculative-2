using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VesselPlayer : MonoBehaviour
{
    public TMP_Text playerText;
    public float textDuration = 1.0f;
    public AudioSource SFXAudioSource;

    public IEnumerator EatNutrient(Nutrient nutrient){
        playerText.GetComponent<TypewriterEffect>().Run("Collected " + nutrient.NutrientName.ToString(),playerText);
        SFXAudioSource.PlayOneShot(nutrient.audioClip);
        yield return new WaitForSeconds(textDuration);
        playerText.text = "";
    }

}
