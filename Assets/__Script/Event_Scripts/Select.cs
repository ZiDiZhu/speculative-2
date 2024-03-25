using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        GetComponent<Outline>().enabled = true;
        Debug.Log("aaaa");
    }

    private void OnMouseExit()
    {
        GetComponent<Outline>().enabled = false;
    }
}
