using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float origibalYpos;
    private Player player;

    private void Start()
    {
        origibalYpos = transform.position.y;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.canCarry = true;
            player.moveableObjectInRange = gameObject;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null) player = other.GetComponent<Player>();
            player.canCarry = false;
        }
    }
}
