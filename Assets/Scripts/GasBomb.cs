using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBomb : MonoBehaviour
{

    public GameObject GasPotion;
    public GameObject GasArea;
    public float BulletForce = 5.0f;
    public float destroyTime = 4f;
    //AudioSource myaudio;
    ParticleSystem particle;

    [SerializeField] Health hScript;
    [SerializeField] ItemSwitch item;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(1)) && (Health.gasPotCount >= 1) && item.gas_inv.activeSelf)
        {

            GameObject currentBullet = Instantiate(GasPotion, this.transform.position, this.transform.rotation) as GameObject;

            currentBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletForce);

            //myaudio.Play();
            Destroy(currentBullet, 10.0f);

            Health.gasPotCount -= 1;
            hScript.gasNum.text = Health.gasPotCount.ToString();

        }

    }

   


}
