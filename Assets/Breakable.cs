using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Trigger 1" + gameObject);
            Destroy(gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Trigger 2" + gameObject);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)

    {
       
        if (collision.gameObject.CompareTag("Decor") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Attack"))
        {
            //Debug.Log("Trigger 3" + gameObject);
            Destroy(gameObject);
        }
    }
    
}

