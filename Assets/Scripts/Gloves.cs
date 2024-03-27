using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : MonoBehaviour
{
    public GameObject hitbox;
    public GameObject particles;
   // ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        // particle = GetComponent<ParticleSystem>();
        hitbox.SetActive(false);
        particles.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            GloveBlast();
        }
        
    }

    private IEnumerator GloveBlast()
    {
        hitbox.SetActive(true);
        particles.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        hitbox.SetActive(false);
        particles.SetActive(false);
    }
}
