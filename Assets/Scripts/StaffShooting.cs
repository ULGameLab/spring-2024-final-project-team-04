using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StaffShooting : MonoBehaviour
{

    public GameObject Bullet;
    public float BulletForce = 100.0f;
    public float destroyTime = 3.0f;
    AudioSource myaudio;
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        myaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0)))
        {

            //create a bullet instance
            GameObject currentBullet = Instantiate(Bullet, this.transform.position, this.transform.rotation) as GameObject;

            //fix scale
            currentBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            //add force to shoot
            currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletForce);

            myaudio.Play();
            particle.Play();

            //Destroy it after a certain time
            Destroy(currentBullet, destroyTime);
        }
    }
}
