using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSpit : MonoBehaviour
{
    public GameObject poisonPool;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndDestroy(2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Gas_Ground"))
        {
            Vector3 position = transform.position;
            position.y = other.transform.position.y;
            Instantiate(poisonPool, position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
