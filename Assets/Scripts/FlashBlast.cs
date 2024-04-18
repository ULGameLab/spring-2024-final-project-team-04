using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBlast : MonoBehaviour
{

    public GameObject flashArea;

    // Start is called before the first frame update
    void Start()
    {
        flashArea.SetActive(false);
        StartCoroutine(flashBlast());
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    private IEnumerator flashBlast()
    {
        yield return new WaitForSeconds(4.0f);
        flashArea.SetActive(true);
        Debug.Log("FlashArea = true");
    }

}
