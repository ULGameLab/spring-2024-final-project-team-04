using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashBomb : MonoBehaviour
{
    public GameObject Bullet;
    public float BulletForce = 5.0f;
    public float destroyTime = 4f;
    AudioSource myaudio;

    [SerializeField] Health hScript;
    [SerializeField] ItemSwitch item;

    // Start is called before the first frame update
    void Start()
    {
        myaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(1)) && (hScript.flashbangCount >= 1) && item.flash_inv.activeSelf)
        {

            GameObject currentBullet = Instantiate(Bullet, this.transform.position, this.transform.rotation) as GameObject;

            currentBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletForce);

            myaudio.Play();

            Destroy(currentBullet, destroyTime);

            hScript.flashbangCount -= 1;
            hScript.flashNum.text = hScript.flashbangCount.ToString();
        }
    }
}