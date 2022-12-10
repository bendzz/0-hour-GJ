using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        player.instance.colliding(other);

    }
    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        //print("go " + other.gameObject.name);
        player.instance.collided(other);
    }
}
