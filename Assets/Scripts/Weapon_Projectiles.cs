using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Projectiles : MonoBehaviour
{
    AudioSource myaudio;
    // Start is called before the first frame update
    void Start()
    {
        myaudio = GetComponent<AudioSource>();
        myaudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Spider"))
        {
            Destroy(gameObject);
        }
        
        }
}
