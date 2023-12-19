using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float origibalYpos;
    private Player player;
    private Outline outline;
    private void Start()
    {
        outline = GetComponentInChildren<Outline>();
        origibalYpos = transform.position.y;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null) player = other.GetComponent<Player>();
            player.canCarry = true;
            player.moveableObjectInRange = gameObject;
            if(outline!=null)outline.enabled = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null) player = other.GetComponent<Player>();
            player.canCarry = false;
            if (outline != null) outline.enabled = false;
        }
    }
}
