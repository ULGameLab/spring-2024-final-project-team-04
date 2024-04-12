using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public GameObject HealthBar;
    public GameObject ShieldBuff;
    public GameObject FlashBomb;
    private static Image HealthBarImage;
    private float health = 75.0f;
    private float maxHealth = 100.0f;
    private float healVal = 10.0f;
    private int healthPotCount = 0;
    private int shieldPotCount = 0;
    public int flashbangCount = 0;
    public int gasPotCount = 0;
    //key stuff
    public static int keyCount = 0;
    public static int bossKeyCount = 0;
    AudioSource potionDrink;

    public TextMeshProUGUI healthNum;
    public TextMeshProUGUI shieldNum;
    public TextMeshProUGUI flashNum;
    public TextMeshProUGUI gasNum;

    [SerializeField] FirstPersonController fpc;
    [SerializeField] ItemSwitch item;
    [SerializeField] SpiderAI spiderHealth;
    [SerializeField] EnemyAI wizardHealth;
    [SerializeField] Gloves gloves;






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
        gasNum.text = gasPotCount.ToString();


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
       
        SetHealthBarValue(health / maxHealth);

        if(health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        
        if((Input.GetMouseButtonDown(1)) && (healthPotCount != 0) && item.health_inv.activeSelf)
        {
            potionDrink.Play();
            health += healVal;
            healthPotCount--;
            healthNum.text = healthPotCount.ToString();
            Debug.Log("HealthPot Used");
        }
        else if ((Input.GetMouseButtonDown(1)) && (shieldPotCount != 0) && item.shields_inv.activeSelf)
        {
            StartCoroutine(ShieldPotion());
            shieldPotCount--;
            shieldNum.text = shieldPotCount.ToString();
            Debug.Log("Shield Potions: " + shieldPotCount.ToString());
        }

        if ((spiderHealth != null && spiderHealth.gloveDamage == true && gloves != null && gloves.hitbox != null && gloves.hitbox.activeSelf)
            || (wizardHealth != null && wizardHealth.gloveDamage == true && gloves != null && gloves.hitbox != null && gloves.hitbox.activeSelf))

        {
            health += 0.0625f;
        }

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
                Debug.Log("key " + keyCount);
                break;

            case "Gold_key":
                bossKeyCount++;
                Debug.Log("Gold_key " + bossKeyCount);
                break;

            case "Speed":
                ++gasPotCount;
                gasNum.text = gasPotCount.ToString();
                break;



            default:
                break;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("GasArea"))
        {
            health -= 0.25f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            portal_to_dungeun.playerSP = collision.gameObject;
            Debug.Log("floor-t");
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
