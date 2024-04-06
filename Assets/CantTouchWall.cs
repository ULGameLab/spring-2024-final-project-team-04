using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantTouchWall : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)

    {
        //Debug.Log("Hit wall");
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
