using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pumpkins : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collide;

    player pl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pl = player.playerSingle;

        Vector3 vel = rb.velocity;

        if (vel.magnitude < 5)
        {
            //rb.AddForce(Vector3.right * 20);
            rb.AddForce(Vector3.Normalize(pl.rb.transform.position - transform.position) * 20);
        }
    }
}
