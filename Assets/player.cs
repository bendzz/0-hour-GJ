using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public static player playerSingle;

    public Camera cam;
    public Rigidbody rb;
    public Collider col;

    public float runAccel = 100;

    public int score = 0;

    public float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        playerSingle = this;
    }

    // Update is called once per frame
    void Update()
    {


        Transform pl = rb.transform;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //print("hor" + Input.GetAxisRaw("Horizontal"));

            //rb.AddForce(Vector3.right * runAccel);
        }
        rb.AddForce(Input.GetAxisRaw("Horizontal") * pl.right * runAccel);
        rb.AddForce(Input.GetAxisRaw("Vertical") * pl.forward * runAccel);


        Vector3 mousePos = Input.mousePosition;
            //Debug.Log(mousePos.x);
            //Debug.Log(mousePos.y);

        rb.MoveRotation(Quaternion.Euler(new Vector3(0, mousePos.x, 0)));
        //rb.MoveRotation(Quaternion.Euler(new Vector3(-90, mousePos.x, 0)));


        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    print( other.name);
    //    print( "go " + other.gameObject.name);
    //}

    public void collided(Collider other)
    {
        print(other.gameObject.name);

        if (other.name.Contains("Bowl"))
        {
            other.gameObject.SetActive(false);
            score++;

            print("Candy obtained! Score" + score);
        }

        if (other.name.Contains("pumpkin"))
        {
            health--;

            print("Ouch! Health " + health);
        }

    }

    

}
