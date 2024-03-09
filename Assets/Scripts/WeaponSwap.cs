using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public GameObject weapon1;
    public GameObject weapon2;
    private bool w2unlocked = true;

    // Start is called before the first frame update
    void Start()
    {
        weapon1.SetActive(true);
        weapon2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weapon1.activeSelf && w2unlocked)
            {
                weapon1.SetActive(false);
                weapon2.SetActive(true);
            }
            else if (weapon2.activeSelf)
            {
                weapon1.SetActive(true);
                weapon2.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && w2unlocked)
        {
            if (weapon1.activeSelf)
            {
                weapon1.SetActive(false);
                weapon2.SetActive(true);
            }
            else if (weapon2.activeSelf)
            {
                weapon1.SetActive(true);
                weapon2.SetActive(false);
            }
        }
    }
}
