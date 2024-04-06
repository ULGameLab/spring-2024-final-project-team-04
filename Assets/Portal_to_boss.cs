using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_to_boss : MonoBehaviour
{

    public GameObject[] playerSP = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        GameObject sp0 = GameObject.FindGameObjectWithTag("spawnPoint");
        playerSP[0] = sp0;

        GameObject sp1 = GameObject.FindGameObjectWithTag("sp2");
        playerSP[1] = sp1;

        GameObject sp2 = GameObject.FindGameObjectWithTag("sp3");
        playerSP[2] = sp2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)

    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = playerSP[0].transform.position;
        }
    }
}
