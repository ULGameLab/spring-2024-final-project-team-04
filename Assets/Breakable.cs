using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger 1" + gameObject);

        if (other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger 2" + gameObject);
        if (other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall"))
        {

            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)

    {
        Debug.Log("Trigger 3" + gameObject);
        if (collision.gameObject.CompareTag("Decor") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
}

