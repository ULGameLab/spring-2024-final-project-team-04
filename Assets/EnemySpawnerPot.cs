using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPot : MonoBehaviour
{
    public GameObject[] bugs;
    public static int bugCount = 0;
    public GameObject Pot;
    private Vector3 PotPosition;
    // Start is called before the first frame update
    void Start()
    {
        PotPosition = Pot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //add later!!
        //pnly spawn enemies if player is in room
        if (bugCount <= 2) {
            Debug.Log("Bug here!");
            // Pick a random bug
            int whichBug = UnityEngine.Random.Range(0, 2);
            // place bug here 
            //random position 
            float zPosition = UnityEngine.Random.Range(PotPosition.z + 1 , PotPosition.z + 9);
            Vector3 randomPosition = new Vector3(PotPosition.x, 0.1f, zPosition);
            //create bug
            Instantiate(bugs[whichBug], randomPosition, Quaternion.identity);
            bugCount++;
        }
    }
}
