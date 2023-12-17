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

    public enum PlayerState
    {
        IDLE,
        IDLE_CARRY,
        WALK,
        WALK_CARRY,
    }
    [SerializeField] private PlayerState state;

    private CharacterController cc;

    void Start()
    {
        cc = GetComponentInChildren<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
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
        if (moveDirection.magnitude == 0)
        {
            if (state == PlayerState.IDLE||state == PlayerState.IDLE_CARRY)
            {
                return;
            }
            
            if (carry.childCount == 0)
            {
                state = PlayerState.IDLE;
                anim.CrossFade("IDLE", 0.2f); // Smoothly transition to IDLE state
            }
            else
            {
                state = PlayerState.IDLE_CARRY;
                anim.CrossFade("IDLE_CARRY", 0.2f); // Smoothly transition to IDLE_CARRY state
            }
        }
        else
        {
            if (state == PlayerState.WALK||state == PlayerState.WALK_CARRY)
            {
                return;
            }   
            if (carry.childCount == 0)
            {
                state = PlayerState.WALK;
                anim.CrossFade("WALK", 0.2f); // Smoothly transition to WALK state
            }
            else
            {
                state = PlayerState.WALK_CARRY;
                anim.CrossFade("WALK_CARRY", 0.2f); // Smoothly transition to WALK_CARRY state
            }
        }

        
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection *= speed;

        cc.Move(moveDirection * Time.deltaTime);

    }

    private void Turn()
    {
        if (moveDirection.magnitude == 0){
            return;
        }

        var rotation = Quaternion.LookRotation(moveDirection);
        this.GetComponent<Transform>().rotation = Quaternion.RotateTowards(transform.rotation, rotation, speed);
    }
    
    public void Carry(Transform obj)
    {
        //pick up object
        obj.position = carry.position;
        obj.SetParent(carry);
    }

    public void PutDown()
    {   
        //put down object
        if(carry.childCount!=0){
            Transform ca = carry.GetChild(0);

            carry.DetachChildren();
            Debug.Log("Putting down object");
            ca.position = new Vector3(ca.position.x, ca.GetComponent<Moveable>().origibalYpos, ca.position.z);
        }
        
    }
}
