using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject openShopMessage;
    public GameObject confirmPurchase;
    public TextMeshProUGUI confirmMessage;
    public List<UnityEngine.Object> availableItems;
    public Transform spawnPoint;

    private Transform playerTransform;
    private UnityEngine.Object selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerTrigger" && !shopMenu.activeSelf)
        {
            Debug.Log("Entered Shop.");
            openShopMessage.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                openShopMessage.SetActive(false);
                shopMenu.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerTrigger")
        {
            Debug.Log("Exited Shop.");
            openShopMessage.SetActive(false);
        }
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
    }

    public void CancelPurchase()
    {
        confirmPurchase.SetActive(false);
    }

    public void Purchase(UnityEngine.Object item)
    {
        confirmMessage.text = "Purchase " + item.name + "?";
        confirmPurchase.SetActive(true);
        selectedObject = item;
    }

    public void ConfirmPurchase()
    {
        confirmPurchase.SetActive(false);
        shopMenu.SetActive(false);
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward;
        Instantiate(selectedObject, spawnPosition, Quaternion.identity);
    }

}
