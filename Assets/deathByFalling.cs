using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathByFalling : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("YOu fell");

       
        
            Destroy(other);
        
    }
    void OnCollisionEnter(Collision collision)

    {
        Debug.Log("YOu fell");



        Destroy(collision.gameObject);
    }
}
