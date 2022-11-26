using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public static player playerSingle;

    public Camera cam;
    public Rigidbody rb;
    public Collider col;

    public Transform ghostModel;

    public float runAccel = 100;

    public int score = 0;

    public float maxHealth = 10;
    public float health;

    float oldCharRot = 0;

    Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        playerSingle = this;
        health = maxHealth;
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

        //ghostModel.rotation = Quaternion.Euler(transform.position - oldPos);
        //ghostModel.rotation = Quaternion.LookRotation(oldPos, transform.up);
        //ghostModel.rotation = Quaternion.Euler(new Vector3(-90, Vector2.SignedAngle(new Vector2(oldPos.x, oldPos.z), new Vector2(transform.position.x, transform.position.z)), 0));
        //float charRot = Vector2.SignedAngle(new Vector2(0, 1), new Vector2(oldPos.x - transform.position.x, -(oldPos.z - transform.position.z)));
        float charRot = Vector2.SignedAngle(new Vector2(0, 1), new Vector2(rb.velocity.x, -rb.velocity.z));
        if (rb.velocity.magnitude > 0.01f)
            oldCharRot = (charRot + oldCharRot) * .5f;
        ghostModel.rotation = Quaternion.Euler(new Vector3(-90, oldCharRot, 0));


        MeshRenderer ghostRend = ghostModel.GetComponent<MeshRenderer>();
        //Material mat = ghostModel.GetComponent<Material>();


        //ghostRend.materials[1].SetColor("_Color", Color.red);
        float healthPercent = (health / maxHealth);
        ghostRend.materials[1].SetColor("_Color", new Color(1, healthPercent, healthPercent));



        if (health < maxHealth)
            health += (maxHealth / 5) * Time.deltaTime;




        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;

        oldPos = transform.position;
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

    public void colliding(Collider other)
    {
        if (other.name.Contains("pumpkin"))
        {
            health -= 1 * Time.deltaTime;

            print("It burns! Health " + health);
        }
    }

}
