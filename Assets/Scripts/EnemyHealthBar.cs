using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    //[SerializeField] private Transform target;

    private void Start()
    {
        if (!CompareTag("Boss"))
        {
            cam = FindFirstObjectByType<Camera>();
        }
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}
