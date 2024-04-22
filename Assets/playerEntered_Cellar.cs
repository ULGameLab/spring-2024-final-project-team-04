using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEntered_Cellar : MonoBehaviour
{ 
    private bool activated = false;
    public GameObject Boss;

    void activateScripts()
    {
        Boss.SetActive(true);
        //avoid errors when pots are destroyed
        activated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!activated && other.gameObject.CompareTag("Player"))
        {
            activateScripts();
            gameObject.SetActive(false);
        }
    }
}


