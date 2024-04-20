using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    int hitsTaken = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            hitsTaken++;
        }
        if (hitsTaken >= 3)
        {
            if ((other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Attack"))
            && hitsTaken == 3)
            {
                //Debug.Log("Trigger 1" + gameObject);
                Destroy(gameObject);
            }
        }
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Attack")) {
            hitsTaken++;
        }
        if (hitsTaken >= 3)
        {
            if ((collision.gameObject.CompareTag("Decor") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Attack")))
            {
                //Debug.Log("Trigger 3" + gameObject);
                Destroy(gameObject);
            }
        }
    }
    
}

