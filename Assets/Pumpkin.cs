using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    public static List<Pumpkin> pumpkins;

    public Rigidbody rb;
    public Collider collide;

    player pl;
    float innateSpeed = 0;

    public Vector3 startPos;

    float aliveTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (pumpkins == null)
            pumpkins = new List<Pumpkin>();

        innateSpeed = Random.value * 20 + 10;
        if (startPos == Vector3.zero)
            startPos = transform.position;

        pumpkins.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.instance.transform.position;
        if (!player.instance.alive || player.instance.victory)
            targetPos = startPos;

        Vector3 vel = rb.velocity;

        float maxSpeed = innateSpeed * Mathf.Clamp01(aliveTime / 10);

        //if (vel.magnitude < 5)
        //if (vel.magnitude < innateSpeed)
        if (vel.magnitude < maxSpeed)
        {
            //rb.AddForce(Vector3.right * 20);
            //rb.AddForce(Vector3.Normalize(pl.rb.transform.position - transform.position) * 50);
            rb.AddForce(Vector3.Normalize(targetPos - transform.position) * 50);
        }

        if (transform.position.y < startPos.y - 5)
            transform.position = startPos;

        aliveTime += Time.deltaTime;
    }

    // player won, play animation
    public void victoryBurst()
    {
        rb.velocity = Vector3.Normalize(transform.position - player.instance.transform.position) * 5;
        rb.velocity += Vector3.up * 12;
    }
    
}
