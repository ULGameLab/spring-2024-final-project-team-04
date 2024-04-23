using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public enum BossState { ATTACK, CHASE, DEFAULT, DEAD };

[RequireComponent(typeof(NavMeshAgent))]
public class CrabBoss : MonoBehaviour
{

    //key stuff
    public GameObject goldKey;

    // Immunity
    private CrabSpawn support;

    public AudioSource AttackSound;
    public AudioSource WalkSound;
    public AudioSource TakeDmgSound;
    public AudioSource DeathSound;
    public AudioSource LaySound;
    GameObject player;
    NavMeshAgent agent;
    public float chaseDistance = 50.0f;
    public float maxHp = 250;
    public float health = 250;
    public float currHealth;
    [SerializeField] EnemyHealthBar healthBar;

    public int currencyOnDeath = 10; // Currency amount to add when the enemy dies
    private CurrencyManager currencyManager;

    // Egg laying
    public GameObject EggsPrefab;
    public Transform layPoint;

    protected BossState state = BossState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);
    Animator animator;
    private float originSpeed;
    private float chaseSpeed;
    AudioSource myaudio;

    private Vector3 lastPosition;
    public float checkInterval = 5f;
    private float timer;
    //private float radius = 20f;

    //Explosion Effect
    //public ParticleSystem deathEffect;
    //bool effectStarted = false;

    public bool gloveDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        currHealth = health;
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateHealthBar(health, maxHp);
        animator = GetComponent<Animator>();
        originSpeed = agent.speed;
        chaseSpeed = agent.speed * 1.5f;
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
        support = FindFirstObjectByType<CrabSpawn>();
        switch (state)
        {
            case BossState.DEFAULT:
                //animator.SetBool("Walking", false);
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = BossState.CHASE;
                }
                break;
            case BossState.CHASE:
                agent.isStopped = false;
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
                animator.SetBool("Attacking", false);
                animator.SetBool("Walking", false);
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                animator.SetBool("LayEggs", true);
                break;
            case BossState.DEAD:
                agent.isStopped = true;
                break;
            default:
                break;
        }

        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && state != BossState.DEAD)
        //{
        //    agent.isStopped = false;
        //}

        timer += Time.deltaTime;
        if (timer >= checkInterval && state != BossState.DEAD)
        {
            timer = -15f;
            state = BossState.SPECIAL;
            StartCoroutine(WaitForSpecial(3.33f));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (support == null)
        {
            if (col.gameObject.CompareTag("Attack"))
            {
                TakeDmgSound.Play();
                health -= 10;
                healthBar.UpdateHealthBar(health, maxHp);
                col.gameObject.SetActive(false);
                if (health <= 0)
                {
                    Die();
                }
            }
            else if (col.CompareTag("GloveAttack") || col.CompareTag("GasArea"))
            {
                TakeDmgSound.Play();
                gloveDamage = true;
                StartCoroutine(ApplyDamage());
            }
        }
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
        DeathSound.Play();
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

    void attackSound()
    {
        AttackSound.Play();
    }

    void walkSound()
    {
        WalkSound.Play();
    }

    void LayEggs()
    {
        LaySound.Play();
        Instantiate(EggsPrefab, layPoint.position, Quaternion.identity);
    }

    private IEnumerator WaitForSpecial(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("LayEggs", false);
        if (state != BossState.DEAD)
        {
            state = BossState.DEFAULT;
        }
    }

}
