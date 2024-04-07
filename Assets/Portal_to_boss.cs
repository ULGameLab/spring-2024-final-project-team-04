using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_to_boss : MonoBehaviour
{

    List<GameObject> playerSP = new List<GameObject>();
    public int numOfBosses = 3;
    public GameObject vineDoor;
    // Start is called before the first frame update
    void Start()
    {
        playerSP.Add(GameObject.FindGameObjectWithTag("spawnPoint"));
        playerSP.Add(GameObject.FindGameObjectWithTag("sp2"));
        playerSP.Add(GameObject.FindGameObjectWithTag("sp3"));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Health.keyCount > 0)
        {
            int whichBoss = UnityEngine.Random.Range(0, numOfBosses);
            Debug.Log(whichBoss);
            Health.keyCount--;
            numOfBosses--;
            collision.transform.position = playerSP[whichBoss].transform.position;
            playerSP.RemoveAt(whichBoss);
            //cant use the same door
            Instantiate(vineDoor, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
