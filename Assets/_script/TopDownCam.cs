using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCam : MonoBehaviour
{
    public float speed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        gameObject.transform.position = new Vector3(transform.position.x + (x*speed*Time.deltaTime),8,transform.localPosition.z+(z * speed*Time.deltaTime));


        //Rotate();
    }

    //TO DO: fix the problem where rotation doesnt chage movement direction
    void Rotate()
    {
        if(Input.GetKey(KeyCode.Q))
            transform.eulerAngles += new Vector3(0, 1, 0);
        if (Input.GetKey(KeyCode.E))
            transform.eulerAngles += new Vector3(0, -1, 0);
    }
}
