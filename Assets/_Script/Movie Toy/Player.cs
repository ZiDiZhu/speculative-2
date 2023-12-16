using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float speed;
    private Vector3 moveDirection;
    public Animator anim;
    public enum PlayerState
    {
        IDLE,
        WALK
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

        if (moveDirection.magnitude == 0)
        {
            if (state == PlayerState.IDLE)
            {
                return;
            }

            state = PlayerState.IDLE;
            anim.CrossFade(state.ToString(), 0.2f); // Smoothly transition to IDLE state
        }
        else
        {
            if (state == PlayerState.WALK)
            {
                return;
            }

            state = PlayerState.WALK;
            anim.CrossFade(state.ToString(), 0.2f); // Smoothly transition to WALK state
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
}
