using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal_to_dungeun : MonoBehaviour
{
    public static GameObject playerSP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Health.bossKeyCount > 0)
        {
            Health.bossKeyCount--;
            collision.transform.position = playerSP.transform.position + new Vector3(0f, 0.8f, 0.0f);
        }
    }
}
