using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance_to_Dungeon : MonoBehaviour
{

    GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.FindGameObjectWithTag("Floor");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.transform.position = floor.transform.position + new Vector3(0f, 0.8f, 0.0f);
        }
    } 
}
