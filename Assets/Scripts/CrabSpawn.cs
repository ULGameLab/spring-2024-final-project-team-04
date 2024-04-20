using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public enum BossState { ATTACK, CHASE, DEFAULT, DEAD };

[RequireComponent(typeof(NavMeshAgent))]
public class CrabSpawn : MonoBehaviour
{
    //public AudioSource AttackSound;
    //public AudioSource WalkSound;
    //public AudioSource TakeDmgSound;
    //public AudioSource DeathSound;
    GameObject player;
    NavMeshAgent agent;
    public float chaseDistance = 50.0f;
    public float maxHp = 30;
    public float health = 30;
    public float currHealth;
    [SerializeField] EnemyHealthBar healthBar;

    public int currencyOnDeath = 0; // Currency amount to add when the enemy dies
    private CurrencyManager currencyManager;


    protected BossState state = BossState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);
    Animator animator;
    private float originSpeed;
    private float chaseSpeed;
    AudioSource myaudio;

    private Vector3 lastPosition;
    public float checkInterval = 1f; // Interval to check if the agent is stuck
    private float timer;
    //private float radius = 20f;

    //Explosion Effect
    //public ParticleSystem deathEffect;
    //bool effectStarted = false;

    public bool gloveDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = health;
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateHealthBar(health, maxHp);
        animator = GetComponent<Animator>();
        originSpeed = agent.speed;
        chaseSpeed = agent.speed * 1.5f;
        agent.speed = chaseSpeed;
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
                animator.SetBool("Walking", false);
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = BossState.CHASE;
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
            case BossState.DEAD:
                agent.isStopped = true;
                break;
            default:
                break;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            agent.isStopped = false;
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
        if (currencyManager != null)
        {
            currencyManager.AddCurrency(currencyOnDeath);
        }
        state = BossState.DEAD;
        animator.SetBool("Die", true);
        Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in allColliders) c.enabled = false;
    }

    //private IEnumerator TurnDamageOff(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    animator.SetBool("takeDamage", false);
    //}

}
