using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyMovement : MonoBehaviour
{
    AudioSource myaudio;
    public float rotationSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        myaudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
