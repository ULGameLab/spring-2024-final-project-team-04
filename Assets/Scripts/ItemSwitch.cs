using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSwitch : MonoBehaviour
{


    public GameObject health_inv;
    public GameObject shields_inv;
    public GameObject flash_inv;


    // Start is called before the first frame update
    void Start()
    {
        health_inv.SetActive(true);
        shields_inv.SetActive(false);
        flash_inv.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (health_inv.activeSelf)
            {
                health_inv.SetActive(false);
                shields_inv.SetActive(true);
                flash_inv.SetActive(false);
            }
            else if (shields_inv.activeSelf)
            {
                health_inv.SetActive(false);
                shields_inv.SetActive(false);
                flash_inv.SetActive(true);
            }
            else if (flash_inv.activeSelf)
            {
                health_inv.SetActive(true);
                shields_inv.SetActive(false);
                flash_inv.SetActive(false);
            }
        }

    }
}
