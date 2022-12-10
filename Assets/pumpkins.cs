using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pumpkins : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collide;

    player pl;
    float innateSpeed = 0;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        innateSpeed = Random.value * 20 + 10;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.instance.transform.position;
        if (!player.instance.alive)
            targetPos = startPos;

        Vector3 vel = rb.velocity;

        //if (vel.magnitude < 5)
        if (vel.magnitude < innateSpeed)
        {
            //rb.AddForce(Vector3.right * 20);
            //rb.AddForce(Vector3.Normalize(pl.rb.transform.position - transform.position) * 50);
            rb.AddForce(Vector3.Normalize(targetPos - transform.position) * 50);
        }

        if (transform.position.y < startPos.y - 5)
            transform.position = startPos;
    }
}
