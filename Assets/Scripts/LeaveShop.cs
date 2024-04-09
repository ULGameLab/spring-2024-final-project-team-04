using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveShop : MonoBehaviour
{
    public GameObject confirmation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Confirmation
            confirmation.SetActive(true);
        }
    }

    // Cancel Button
    public void Cancel()
    {
        confirmation.SetActive(false);
    }

    // Go Button
    public void GoButton()
    {
        SceneManager.LoadScene("Dungeon");
    }

}
