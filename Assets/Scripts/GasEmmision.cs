using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEmmision : MonoBehaviour
{


    public GameObject GasArea;

    // Start is called before the first frame update
    void Start()
    {
        GasArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Gas_Ground") || other.gameObject.CompareTag("Floor"))
        {
            GasArea.SetActive(true);
        }
    }
}
