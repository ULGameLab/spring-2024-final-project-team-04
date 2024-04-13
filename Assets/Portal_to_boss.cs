using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_to_boss : MonoBehaviour
{
    public GameObject vineDoor;
   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Health.keyCount > 0)
        { 
            int whichRoom = UnityEngine.Random.Range(0, ResetDoorStaticList.playerSP.Count);
            Health.keyCount--;
            collision.transform.position = ResetDoorStaticList.playerSP[whichRoom].transform.position;
            ResetDoorStaticList.playerSP.RemoveAt(whichRoom);
            Debug.Log(ResetDoorStaticList.playerSP.Count);
            //cant use the same door
            Instantiate(vineDoor, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
