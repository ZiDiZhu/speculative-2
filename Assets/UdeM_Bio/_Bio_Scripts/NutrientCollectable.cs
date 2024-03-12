using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientCollectable : MonoBehaviour
{
    [SerializeField]private Nutrient nutrient;
    private void Awake()
    {
        nutrient = this.GetComponent<Nutrient>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            VesselPlayer vesselPlayer = other.gameObject.GetComponent<VesselPlayer>();
            vesselPlayer.EatNutrient(nutrient);
            Destroy(gameObject);
        }
    }

}
