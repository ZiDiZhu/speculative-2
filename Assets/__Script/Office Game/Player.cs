using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    
    public float speed;
    private Vector3 moveDirection;
    public Animator anim;
    public Transform carry; // Transform parent for the objects the player is carrying
    public bool canCarry; // Can the player carry an object?
    public GameObject moveableObjectInRange; // The object the player is currently in range of

    public InputPromptDisplay inputPromptDisplay;

    public enum PlayerState
    {
        IDLE,
        IDLE_CARRY,
        WALK,
        WALK_CARRY,
        TALK
    }
    [SerializeField] private PlayerState state;

    private CharacterController cc;

    void Start()
    {
        cc = GetComponentInChildren<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        if (inputPromptDisplay == null) inputPromptDisplay = FindObjectOfType<InputPromptDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
        MoveObject();
        UpdateAnimation();
    }
    
    // player moves
    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection *= speed;

        cc.Move(moveDirection * Time.deltaTime);

    }
    
    // turning player facing direction according to movement
    private void Turn()
    {
        if (moveDirection.magnitude == 0){
            return;
        }

        var rotation = Quaternion.LookRotation(moveDirection);
        this.GetComponent<Transform>().rotation = Quaternion.RotateTowards(transform.rotation, rotation, speed);
    }
    
    //check for input for moving object
    private void MoveObject(){
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (carry.childCount == 0)
            {
                if (canCarry)
                {
                    //pick up object
                    Debug.Log("Picking up object");
                    Carry(moveableObjectInRange.transform);
                }
            }
            else
            {
                //put down object
                PutDown();
            }
        }
    }


    //check player currentState and update animation
    private void UpdateAnimation(){
        if (moveDirection.magnitude == 0)
        {
            if (state == PlayerState.IDLE || state == PlayerState.IDLE_CARRY)
            {
                return;
            }

            if (carry.childCount == 0)
            {
                state = PlayerState.IDLE;
                anim.CrossFade("IDLE", 0.2f); // Smoothly transition to IDLE currentState
            }
            else
            {
                state = PlayerState.IDLE_CARRY;
                anim.CrossFade("IDLE_CARRY", 0.2f); // Smoothly transition to IDLE_CARRY currentState
            }
        }
        else
        {
            if (state == PlayerState.WALK || state == PlayerState.WALK_CARRY)
            {
                return;
            }
            if (carry.childCount == 0)
            {
                state = PlayerState.WALK;
                anim.CrossFade("WALK", 0.2f); // Smoothly transition to WALK currentState
            }
            else
            {
                state = PlayerState.WALK_CARRY;
                anim.CrossFade("WALK_CARRY", 0.2f); // Smoothly transition to WALK_CARRY currentState
            }
        }
    }
    
    // pick up object and play animation
    public void Carry(Transform obj)
    {
        //pick up object
        anim.CrossFade("PICKUP", 0.1f); // Smoothly transition to PICKUP currentState
        inputPromptDisplay.pickUp.SetActive(false);
        inputPromptDisplay.putDown.SetActive(true);
        StartCoroutine(WaitForPickup(0.8f,obj));
    }
    IEnumerator WaitForPickup(float duration, Transform obj)
    {
        yield return new WaitForSeconds(duration);
        obj.position = carry.position;
        obj.SetParent(carry);
    }

    //put down object and play animation
    public void PutDown()
    {   
        //put down object
        if(carry.childCount!=0){

            anim.CrossFade("PICKUP", 0.1f); // Smoothly transition to PICKUP currentState
            inputPromptDisplay.putDown.SetActive(false);
            inputPromptDisplay.pickUp.SetActive(true);
            Transform ca = carry.GetChild(0);
            carry.DetachChildren();
            StartCoroutine(WaitForPutDown(0.4f, ca));

        }
        
    }
    IEnumerator WaitForPutDown(float duration,Transform ca)
    {
        yield return new WaitForSeconds(duration);
        ca.position = new Vector3(ca.position.x, ca.GetComponent<Moveable>().origibalYpos, ca.position.z);

    }
}
