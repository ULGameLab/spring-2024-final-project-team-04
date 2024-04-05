using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_key_Spawner : MonoBehaviour
{

    public int MaxCount;
    public GameObject key;
    
    // Start is called before the first frame update
    void Start()
    {
        MaxCount = DungeonCreator.bugCount;
        Debug.Log(MaxCount);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        DungeonCreator.bugCount--;
        //need 3 keys to spawn when 1/3 2/3 , and all enemies are killed
        if (DungeonCreator.bugCount <= MaxCount / 3) {
            Instantiate(key, gameObject.transform.position, Quaternion.identity);
            MaxCount = MaxCount / 3;
        }
        
        //Debug.Log(DungeonCreator.bugCount);

    }
}
