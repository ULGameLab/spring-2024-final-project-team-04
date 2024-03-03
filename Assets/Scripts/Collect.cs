using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    AudioSource collectSound;
    // Start is called before the first frame update
    void Start()
    {
        collectSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(0, 1.0f, 0);
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            // Disable all Renderers and Colliders
            Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer c in allRenderers) c.enabled = false;
            Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider c in allColliders) c.enabled = false;
            StartCoroutine(PlayAndDestroy(collectSound.clip.length));
        }
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        collectSound.Play();
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
