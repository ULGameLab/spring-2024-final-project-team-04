using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public enum BossState { ATTACK, CHASE, DEFAULT, DEAD };

[RequireComponent(typeof(NavMeshAgent))]
public class OrcBoss : MonoBehaviour
{
    //key stuff
    public GameObject goldKey;

    //public AudioSource AttackSound;
    //public AudioSource WalkSound;
    //public AudioSource TakeDmgSound;
    //public AudioSource DeathSound;
    public int currencyOnDeath = 10; // Currency amount to add when the enemy dies
    private CurrencyManager currencyManager;

    GameObject player;
    NavMeshAgent agent;
    public float chaseDistance = 50.0f;
    public float maxHp = 250;
    public float health = 250;
    public float currHealth;
    [SerializeField] EnemyHealthBar healthBar;
    public float speedMultiplier = 3f;


    protected BossState state = BossState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);
    Animator animator;
    private float originSpeed;
    private float chaseSpeed;
    AudioSource myaudio;

    private Vector3 lastPosition;
    public float checkInterval = 10f;
    private float timer;
    //private float radius = 20f;

    //Explosion Effect
    //public ParticleSystem deathEffect;
    //bool effectStarted = false;

    public GameObject fireEffect;
    public bool gloveDamage = false;
    public bool fireDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        fireEffect.SetActive(false);
        timer = 0f;
        currHealth = health;
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateHealthBar(health, maxHp);
        animator = GetComponent<Animator>();
        originSpeed = agent.speed;
        chaseSpeed = agent.speed * 2f;
        animator.speed = speedMultiplier;
        // Find and cache a reference to the CurrencyManager
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.DEFAULT:
                
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = BossState.CHASE;
                }
                else
                {
                    animator.SetBool("Walking", false);
                }
                break;
            case BossState.CHASE:
                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    state = BossState.DEFAULT;
                    break;
                }
                agent.SetDestination(player.transform.position);
                animator.SetBool("Walking", true);
                if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance + 0.1f)
                {
                    state = BossState.ATTACK;
                }
                break;
            case BossState.ATTACK:
                Vector3 direction = player.transform.position - transform.position;
                direction.y = 0f;
                // Rotate the enemy to face the player
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
                }
                animator.SetBool("Attacking", true);
                animator.SetBool("Walking", false);
                agent.isStopped = true;
                if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
                {
                    state = BossState.DEFAULT;
                    animator.SetBool("Attacking", false);
                }
                break;
            case BossState.SPECIAL:
                animator.SetBool("Spin", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                agent.speed = chaseSpeed;
                break;
            case BossState.DEAD:
                agent.isStopped = true;
                break;
            default:
                break;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && state != BossState.DEAD)
        {
            agent.isStopped = false;
        }

        timer += Time.deltaTime;
        if (timer >= checkInterval && state != BossState.DEAD && Vector3.Distance(transform.position, player.transform.position) < 15)
        {
            timer = -15f;
            state = BossState.SPECIAL;
            StartCoroutine(WaitForSpecial(5));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Attack"))
        {
            health -= 10;
            healthBar.UpdateHealthBar(health, maxHp);
            //animator.SetBool("takeDamage", true);
            //StartCoroutine(TurnDamageOff(1));
            //TakeDmgSound.Play();
            // Disable all Renderers and Colliders
            col.gameObject.SetActive(false);
            if (health <= 0)
            {
                Die();
            }
            //Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            //foreach (Renderer c in allRenderers) c.enabled = false;
            //Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
            //foreach (Collider c in allColliders) c.enabled = false;
            //gameObject.GetComponent<ParticleSystemRenderer>().enabled = true;
            //StartCoroutine(PlayAndDestroy(myaudio.clip.length));
        }
        else if (col.CompareTag("GloveAttack") || col.CompareTag("GasArea"))
        {
            gloveDamage = true;
            StartCoroutine(ApplyDamage());
        }
        else if (col.CompareTag("FlashAOE"))
        {
            StartCoroutine(FlashBang());
        }
        else if (col.CompareTag("FireAttack"))
        {
            health -= 10;
            healthBar.UpdateHealthBar(health, maxHp);
            //animator.SetBool("takeDamage", true);
            //StartCoroutine(TurnDamageOff(1));
            //TakeDmgSound.Play();
            // Disable all Renderers and Colliders
            col.gameObject.SetActive(false);
            if (health <= 0)
            {
                Die();
            }
            fireDamage = true;
            Debug.Log("Fire Damage");
            StartCoroutine(ApplyFireDamage());
        }
    }

    private IEnumerator FlashBang()
    {
        Debug.Log("Flash working");
        state = BossState.DEAD;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(2.0f);
        state = BossState.DEFAULT;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GloveAttack") || other.CompareTag("GasArea"))
        {
            gloveDamage = false;
        }
    }

    IEnumerator ApplyDamage()
    {
        float i = 0;
        while (gloveDamage && health > 0 && i < 5.0f)
        {
            yield return new WaitForSeconds(0.75f);

            TakeDamage(2);
            healthBar.UpdateHealthBar(health, maxHp);
            i += 1.0f;
        }
    }

    IEnumerator ApplyFireDamage()
    {
        fireEffect.SetActive(true);
        float i = 0;
        while (fireDamage && health > 0 && i <= 7.0f)
        {
            yield return new WaitForSeconds(0.5f);

            TakeDamage(2);
            healthBar.UpdateHealthBar(health, maxHp);
            i += 1.0f;
            if (i == 7.0f)
            {
                fireEffect.SetActive(false);
                fireDamage = false;
                Debug.Log("FireDamage = false");
            }
        }
    }

    void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (currencyManager != null)
        {
            currencyManager.AddCurrency(currencyOnDeath);
        }
        state = BossState.DEAD;
        animator.SetBool("Die", true);
        StartCoroutine(spawnKey());
        Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in allColliders) c.enabled = false;
        StartCoroutine(PlayAndDestroy(4.67f));
    }

    //golden key on death is droped
    private IEnumerator spawnKey()
    {

        Instantiate(goldKey, transform.position + new Vector3(0f, 0.8f, 0.0f), Quaternion.Euler(270f, 0f, 0f));
        yield return null;
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        //myaudio.Play();
        //SpawnRandomFood(transform.position);
        yield return new WaitForSeconds(waitTime);
        //deathEffect.Play();
        //DeathSound.Play();
        yield return new WaitForSeconds(0.4f);
        Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer c in allRenderers) c.enabled = false;
        //StopBloodSplatter();
        Destroy(gameObject);
    }

    private IEnumerator WaitForSpecial(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("Spin", false);
        state = BossState.DEFAULT;
        agent.speed = originSpeed;
    }

}
