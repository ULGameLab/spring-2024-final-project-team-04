using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    public GameObject CrabSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToHatch(4));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitToHatch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 hatchPoint = transform.position;
        Instantiate(CrabSpawn, hatchPoint, Quaternion.identity);
        hatchPoint.x += 1;
        Instantiate(CrabSpawn, hatchPoint, Quaternion.identity);
        hatchPoint.z += 1;
        Instantiate(CrabSpawn, hatchPoint, Quaternion.identity);
        Destroy(gameObject);
    }
}
