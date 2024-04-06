using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{

    public GameObject HealthBar;
    public GameObject ShieldBuff;
    public GameObject FlashBomb;
    private static Image HealthBarImage;
    private float health = 75.0f;
    private float healVal = 10.0f;
    private int healthPotCount = 0;
    private int shieldPotCount = 0;
    public int flashbangCount = 0;
    //key stuff
    public static int keyCount = 0;

    AudioSource potionDrink;

    public TextMeshProUGUI healthNum;
    public TextMeshProUGUI shieldNum;
    public TextMeshProUGUI flashNum;

    [SerializeField] FirstPersonController fpc;
    [SerializeField] ItemSwitch item;
    
   

   


    // Start is called before the first frame update
    public void Start()
    {

        potionDrink = GetComponent<AudioSource>();

        if (HealthBar != null)
        {
            HealthBarImage = HealthBar.transform.GetComponent<Image>();
        }
        SetHealthBarValue(health);

        
        healthNum.text = healthPotCount.ToString();
        shieldNum.text = shieldPotCount.ToString();
        flashNum.text = flashbangCount.ToString();
        
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

        
        if((Input.GetKeyDown(KeyCode.Alpha1)) && (healthPotCount != 0) && item.health_inv.activeSelf)
        {
            potionDrink.Play();
            health += healVal;
            healthPotCount--;
            healthNum.text = healthPotCount.ToString();
            Debug.Log("HealthPot Used");
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha2)) && (shieldPotCount != 0) && item.shields_inv.activeSelf)
        {
            StartCoroutine(ShieldPotion());
            shieldPotCount--;
            shieldNum.text = shieldPotCount.ToString();
            Debug.Log("Shield Potions: " + shieldPotCount.ToString());
        }
       /* else if((Input.GetKeyDown(KeyCode.Alpha3)) && (flashbangCount != 0))
        {
            //flashbangCount--;
            //flashNum.text = flashbangCount.ToString();
            //flashbangCount--;
        }
       */
    }

    public void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {

            case "Heal":
                ++healthPotCount;
                healthNum.text = healthPotCount.ToString();
                break;

            case "Shield":
                ++shieldPotCount;
                shieldNum.text = shieldPotCount.ToString();
                Debug.Log("Shield Potions Collected: " + shieldPotCount.ToString());
                break;

            case "Flash":
                ++flashbangCount;
                flashNum.text = flashbangCount.ToString();
                break;

            case "EnemyAttack":
                health -= 10;
                Debug.Log("Enemy attack");
                break;

            case "key":
                keyCount++;
                Debug.Log("key");
                break;
            default:
                break;
        }
    }

    private IEnumerator ShieldPotion()
    {
        potionDrink.Play();
        Debug.Log("Shield Potion used");
        ShieldBuff.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        ShieldBuff.SetActive(false);
    }
    private IEnumerator SpeedPotion()
    {
        fpc.walkSpeed *= 2;
        fpc.sprintSpeed *= 2;
        Debug.Log("Speed Increased");
        yield return new WaitForSeconds(5.0f);
        fpc.walkSpeed = (fpc.walkSpeed / 2);
        fpc.sprintSpeed = (fpc.sprintSpeed / 2);
        Debug.Log("Speed Reset");
    }
}
