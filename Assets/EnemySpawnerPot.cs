using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPot : MonoBehaviour
{
    public GameObject[] bugs;
    float timePassed = 15f;
    public GameObject Pot;
    private Vector3 PotPosition;
    // Start is called before the first frame update
    void Start()
    {
        PotPosition = Pot.transform.position;
        int whichBug = UnityEngine.Random.Range(0, 2);
        float zPosition = UnityEngine.Random.Range(PotPosition.z + 1, PotPosition.z + 9);
        Vector3 randomPosition = new Vector3(PotPosition.x, 0.1f, zPosition);
        Instantiate(bugs[whichBug], randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        //add later!!
        //pnly spawn enemies if player is in room
        if (timePassed >= 20)
        {
            int whichBug = UnityEngine.Random.Range(0, 2);
            // place bug here 
            //random position 
            float zPosition = UnityEngine.Random.Range(PotPosition.z + 1, PotPosition.z + 9);
            Vector3 randomPosition = new Vector3(PotPosition.x, 0.1f, zPosition);
            //create bug
            Instantiate(bugs[whichBug], randomPosition, Quaternion.identity);
            timePassed = 0;
        }
    }
}
