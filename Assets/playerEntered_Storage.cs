using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEntered_Storage : MonoBehaviour
{
    private bool activated = false;
    public GameObject Boss;
    public GameObject lights;

    void activateScripts()
    {
        Boss.SetActive(true);
        lights.SetActive(true);
        //
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
