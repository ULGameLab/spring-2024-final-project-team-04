using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_to_boss : MonoBehaviour
{

    static List<GameObject> playerSP = new List<GameObject>();
    public GameObject vineDoor;
    // Start is called before the first frame update
    void Start()
    {
        if (playerSP.Count == 0) // Only initialize the list if it's empty
        {
            playerSP.Add(GameObject.FindGameObjectWithTag("spawnPoint"));
            playerSP.Add(GameObject.FindGameObjectWithTag("sp2"));
            playerSP.Add(GameObject.FindGameObjectWithTag("sp3"));
        }
        Debug.Log(playerSP.Count);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Health.keyCount > 0)
        { 
            int whichRoom = UnityEngine.Random.Range(0, playerSP.Count);
            Health.keyCount--;
            collision.transform.position = playerSP[whichRoom].transform.position;
            playerSP.RemoveAt(whichRoom);
            Debug.Log(playerSP.Count);
            //cant use the same door
            Instantiate(vineDoor, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
