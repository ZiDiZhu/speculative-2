using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    private Vector3 moveDirection;
    public Animator anim;

    private CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
        if (moveDirection.magnitude == 0) { anim.SetFloat("Blend", 0f); }
        else
        {
            anim.SetFloat("Blend", 1f);
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
        if (moveDirection.magnitude == 0) { return; }

        var rotation = Quaternion.LookRotation(moveDirection);
        this.GetComponent<Transform>().rotation = Quaternion.RotateTowards(transform.rotation, rotation, speed);
    }
}
