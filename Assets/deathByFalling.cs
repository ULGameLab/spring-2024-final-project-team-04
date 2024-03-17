using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathByFalling : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("1 fell");

       
        
            Destroy(other);
        
    }
    void OnCollisionEnter(Collision collision)

    {
        Debug.Log("1.1 fell");



        Destroy(collision.gameObject);
    }
}
