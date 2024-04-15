using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDoorStaticList : MonoBehaviour
{
    public static List<GameObject> playerSP = new List<GameObject>();
    void Start()
    {
        playerSP.Clear();
        if (playerSP.Count == 0) // Only initialize the list if it's empty
        {
            playerSP.Add(GameObject.FindGameObjectWithTag("spawnPoint"));
            playerSP.Add(GameObject.FindGameObjectWithTag("sp2"));
            playerSP.Add(GameObject.FindGameObjectWithTag("sp3"));
        }
    }
}
