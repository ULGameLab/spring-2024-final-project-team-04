using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSpit : MonoBehaviour
{
    public GameObject poisonPool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Vector3 position = transform.position;
            position.y = other.transform.position.y;
            Instantiate(poisonPool, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
