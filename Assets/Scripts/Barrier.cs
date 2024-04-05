using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public LayerMask supportLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] pillars = Physics.OverlapSphere(transform.position, 100, supportLayer);

        // If no support are found, destroy the barrier
        if (pillars.Length == 0)
        {
            Destroy(gameObject);
        }
    }
}
