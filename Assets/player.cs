using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public static player instance;

    public Camera cam;
    public Rigidbody rb;
    public Collider col;

    public Transform ghostModel;

    public float runAccel = 100;

    public int score = 0;

    public float maxHealth = 10;
    public float health;

    float oldCharRot = 0;

    public bool alive = true;
    public bool victory = false;

    public int candyTotal;
    public int candyEaten;

    public Vector3 startPos;

    // Dead animations 
    /// <summary>
    /// Expand a sphere around the player to throw the pumpkins away
    /// </summary>
    public SphereCollider deadSplosion;
    /// <summary>
    /// Stage of the death animation, what part to play
    /// </summary>
    public int deadAni = 0;
    public GameObject pumpkinThatKilledMe;
    public GameObject ghostPumpkin;



    Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        health = maxHealth;

        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
        //Object[] objects = GameObject.FindObjectsOfType(typeof(Object), true);
        //GameObject.find

        // count candy
        foreach(object obj in objects)
        {
            GameObject go = (GameObject)obj;
            if (go.name.Contains("Bowl"))
                candyTotal++;
        }
        print("candyTotal " + candyTotal);

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        Transform pl = rb.transform;

        if (alive)
        {
            rb.AddForce(Input.GetAxisRaw("Horizontal") * pl.right * runAccel * Time.deltaTime);
            rb.AddForce(Input.GetAxisRaw("Vertical") * pl.forward * runAccel * Time.deltaTime);
        }

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
        if (victory)
            ghostRend.materials[1].SetColor("_Color", new Color(.3f, 1, .8f));

        if (health < maxHealth)
            health += (maxHealth / 5) * Time.deltaTime;
        if (health < 0)
            alive = false;

        //if (health < 0)
        //    Application.

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;

        oldPos = transform.position;
        if (!alive) {
            // Dead, play animation

            if (deadAni == 0) {
                if (deadSplosion == null)
                {
                    //deadSplosion = new SphereCollider();
                    deadSplosion = gameObject.AddComponent<SphereCollider>();

                    rb.isKinematic = true;
                    Physics.IgnoreCollision(deadSplosion, ghostModel.GetComponent<MeshCollider>());

                    deadSplosion.radius = 0;
                } else
                {
                    deadSplosion.radius += 50 * Time.deltaTime;
                    //if (deadSplosion.radius > 10)
                    //    deadAni = 1;

                    ghostModel.localScale -= (500 * Time.deltaTime) * Vector3.one;
                    if (ghostModel.localScale.x < 0)
                        deadAni = 1;
                }
            } else if (deadAni == 1)
            {
                // stage 2 of ani
                //if (deadSplosion != null)
                //    deadSplosion = null;

                //ghostModel = null;
                ghostModel.gameObject.SetActive(false);
                deadSplosion.enabled = false;

                ghostPumpkin = Instantiate(pumpkinThatKilledMe, transform.position, transform.rotation);

                ghostPumpkin.GetComponent<Pumpkin>().startPos = pumpkinThatKilledMe.GetComponent<Pumpkin>().startPos + new Vector3(0, 0, 3);

                deadAni = 2;
            }

        }

        if (candyEaten >= candyTotal)
        {
            //VICTORY
            if (!victory)
            {
                victory = true;

                foreach(Pumpkin p in Pumpkin.pumpkins)
                {
                    p.victoryBurst();
                }
            }
        }


        if (transform.position.y < -5)
            transform.position = startPos;



    }




    //private void OnTriggerEnter(Collider other)
    //{
    //    print( other.name);
    //    print( "go " + other.gameObject.name);
    //}

    public void collided(Collider other)
    {
        print("Hit: " + other.gameObject.name);

        if (other.name.Contains("Bowl"))
        {
            other.gameObject.SetActive(false);
            score++;

            //print("Candy obtained! Score" + score);

            candyEaten++;
        }

        else if (other.name.Contains("pumpkin"))
        {
            health--;

            //print("Ouch! Health " + health);
            pumpkinThatKilledMe = other.gameObject;
        }


        collidingOrCollided(other);
    }

    public void colliding(Collider other)
    {
        if (other.name.Contains("pumpkin"))
        {
            health -= 1 * Time.deltaTime;

            //print("It burns! Health " + health);
        }
        collidingOrCollided(other);
    }

    public void collidingOrCollided(Collider other)
    {
        if (other.name.Contains("Ground"))
        {
            if (victory)
            {
                // win animation
                rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);

                //MeshRenderer ghostRend = ghostModel.GetComponent<MeshRenderer>();
                ////ghostRend.materials[1].SetColor("_Color", new Color(.5f, .8f, 1));
                //ghostRend.materials[1].SetColor("_Color", new Color(0, .8f, 1));
            }
        }
    }

}
