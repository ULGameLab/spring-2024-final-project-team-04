using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;

    public bool w2unlocked = false;
    public bool w3unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        weapon1.SetActive(true);
        weapon2.SetActive(false);
        weapon3.SetActive(false);
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
            else if (weapon2.activeSelf && w3unlocked)
            {
                weapon2.SetActive(false);
                weapon3.SetActive(true);
            }
            else if(weapon2.activeSelf && !w3unlocked)
            {
                weapon2.SetActive(false);
                weapon1.SetActive(true);
            }
            else if (weapon3.activeSelf)
            {
                weapon3.SetActive(false);
                weapon1.SetActive(true);
            }
        }
        
    }
}
