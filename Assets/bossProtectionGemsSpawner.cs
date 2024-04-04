using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossProtectionGemsSpawner : MonoBehaviour
{
    public GameObject crystalBall;
    public GameObject floor;
    private Vector3 floorPosition;
    // Start is called before the first frame update
    void Start()
    {
        floorPosition = floor.transform.position;
        spawnCrystalBalls();
    }

    void spawnCrystalBalls() {
        //random position 
        Vector3 floorSize = floor.GetComponent<Renderer>().bounds.size;
        
        for (int i = 0; i < 3; i++)
        {
            float xPosition = UnityEngine.Random.Range(floorPosition.x - floorSize.x / 2f, floorPosition.x + floorSize.x / 2f);
            float zPosition = UnityEngine.Random.Range(floorPosition.z - floorSize.z / 2f, floorPosition.z + floorSize.z / 2f);
            Vector3 randomPosition = new Vector3(xPosition, 1, zPosition);
            //create ball
            Instantiate(crystalBall, randomPosition, Quaternion.identity);
        }
    }
}
