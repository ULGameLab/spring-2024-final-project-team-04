using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterDiningHall : MonoBehaviour
{

    public GameObject pot1;
    public GameObject pot2;
    private bool activated = false;

    void Start() { 
        pot1.GetComponent<EnemySpawnerPot>().enabled = false;
        pot2.GetComponent<EnemySpawnerPot>().enabled = false;
    }

    void activateScripts()
    {
        pot1.GetComponent<EnemySpawnerPot>().enabled = true;
        pot2.GetComponent<EnemySpawnerPot>().enabled = true;
       //avoid errors when pots are destroyed
        activated = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!activated && collision.gameObject.CompareTag("Player"))
        {
            activateScripts();
        }
    }
}
