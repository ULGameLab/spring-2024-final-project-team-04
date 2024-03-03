using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{

    public GameObject HealthBar;
    private static Image HealthBarImage;
    private float health = 100.0f;
    private float healVal = 10.0f;
  
    // Start is called before the first frame update
    void Start()
    {

        if (HealthBar != null)
        {
            HealthBarImage = HealthBar.transform.GetComponent<Image>();
        }
        SetHealthBarValue(health);

    }

    public static void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;

        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (HealthBarImage.fillAmount < 0.4f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public static void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }
    public static float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthBarValue(health / 100);
    }

    public void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {

            case "Heal":
                health += healVal;
                break;
            
            
            default:
                break;
        }
    }
}
