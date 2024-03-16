using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");

        if (other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger enter");
        if (other.gameObject.CompareTag("Decor") || other.gameObject.CompareTag("Wall"))
        {

            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)

    {
        Debug.Log("Trigger enter");
        if (collision.gameObject.CompareTag("Decor") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Trigger enter");
        if (collision.gameObject.CompareTag("Decor") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

