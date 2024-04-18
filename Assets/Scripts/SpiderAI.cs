using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// FSM States for the enemy
//public enum EnemyState { ATTACK, CHASE, MOVING, DEFAULT, DEAD };

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(AudioSource))]
public class SpiderAI : MonoBehaviour
{
    //public AudioSource AttackSound;
    //public AudioSource WalkSound;
    public AudioSource TakeDmgSound;
    public AudioSource DeathSound;
    GameObject player;
    NavMeshAgent agent;
    public float chaseDistance = 10.0f;
    public float maxHp = 50;
    public float health = 50;
    public float currHealth;
    [SerializeField] EnemyHealthBar healthBar;
    //public GameObject[] foodPrefabs; // Array of your food prefabs

    public int currencyOnDeath = 1; // Currency amount to add when the enemy dies
    private CurrencyManager currencyManager;


    protected EnemyState state = EnemyState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);
    Animator animator;
    private float originSpeed;
    private float chaseSpeed;
    AudioSource myaudio;

    private Vector3 lastPosition;
    public float checkInterval = 1f; // Interval to check if the agent is stuck
    private float timer;
    private float radius = 20f;

    //Explosion Effect
    public ParticleSystem deathEffect;
    //bool effectStarted = false;

    public bool gloveDamage = false;
    public bool fireDamage = false;

    //key stuff
    public int MaxCount;
    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {

        currHealth = health;
        //for keys
        MaxCount = DungeonCreator.bugCount;
        //Debug.Log(MaxCount);
        //other
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateHealthBar(health, maxHp);
        animator = GetComponent<Animator>();
        originSpeed = agent.speed;
        chaseSpeed = agent.speed * 1.5f;
        //myaudio = GetComponent<AudioSource>();
        //deathEffect = transform.GetComponent<ParticleSystem>();
        timer = 0f;
        lastPosition = transform.position;
        // Find and cache a reference to the CurrencyManager
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found in the scene.");
        }
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-20.0f, 20.0f), 0, Random.Range(-20.0f, 20.0f));
    }

    Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;

        NavMeshHit navMeshHit;

        // Sample a position on the NavMesh closest to the random direction
        if (NavMesh.SamplePosition(randomDirection, out navMeshHit, radius, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }

        // If no valid position is found, return the center position
        return center;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.DEFAULT:
                animator.SetBool("isWalking", false);
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = EnemyState.CHASE;
                    animator.SetBool("inCombat", true);
                }
                else
                {
                    state = EnemyState.MOVING;
                    animator.SetBool("inCombat", false);
                    destination = GetRandomNavMeshPosition(transform.position, radius);
                    agent.SetDestination(destination);
                }
                break;
            case EnemyState.MOVING:
                if (!agent.hasPath && !agent.pathPending)
                {
                    // If the destination is unreachable, set a new destination
                    state = EnemyState.DEFAULT;
                }
                animator.SetBool("isWalking", true);
                //Debug.Log("Dest = " + destination);
                if (Vector3.Distance(transform.position, destination) <= agent.stoppingDistance)
                {
                    state = EnemyState.DEFAULT;
                }
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = EnemyState.CHASE;
                    animator.SetBool("inCombat", true);
                }
                break;
            case EnemyState.CHASE:
                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    state = EnemyState.DEFAULT;
                    animator.SetBool("inCombat", false);
                    break;
                }
                agent.SetDestination(player.transform.position);
                animator.SetBool("isWalking", true);
                if (!gameObject.CompareTag("Spider")){
                    if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance / 2)
                    {
                        animator.SetBool("fastChase", true);
                        agent.speed = chaseSpeed;
                    }
                    else
                    {
                        animator.SetBool("fastChase", false);
                        agent.speed = originSpeed;
                    }
                }
                if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance + 0.1f)
                {
                    state = EnemyState.ATTACK;
                }
                break;
            case EnemyState.ATTACK:
                Vector3 direction = player.transform.position - transform.position;
                direction.y = 0f;
                // Rotate the enemy to face the player
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
                }
                animator.SetBool("isAttacking", true);
                animator.SetBool("isWalking", false);
                agent.isStopped = true;
                if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
                {
                    state = EnemyState.DEFAULT;
                    animator.SetBool("isAttacking", false);
                }
                break;
            case EnemyState.DEAD:
                agent.isStopped = true;
                break;
            default:
                break;
        }

        // Check if the agent is stuck
        timer += Time.deltaTime;
        //Debug.Log($"Timer: {timer}");
        if (timer >= checkInterval)
        {
            timer = 0f;
            if (IsStuck())
            {
                Debug.Log("Is Stuck!");
                // If the agent is stuck, reset its destination
                state = EnemyState.DEFAULT;
            }
            else
            {
                lastPosition = transform.position;
            }
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            agent.isStopped = false;
        }
    }

    // Call this method to spawn a random food item at a given position
    //public void SpawnRandomFood(Vector3 position)
    //{
    //    int randomIndex = Random.Range(0, foodPrefabs.Length);
    //    GameObject randomFoodPrefab = foodPrefabs[randomIndex];
    //    // Adjust the spawn position to be slightly above the ground
    //    Vector3 spawnPosition = new Vector3(position.x, position.y + 1.0f, position.z);
    //    Instantiate(randomFoodPrefab, spawnPosition, Quaternion.identity);
    //}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Attack"))
        {
            health -= 10;
            healthBar.UpdateHealthBar(health, maxHp);
            animator.SetBool("takeDamage", true);
            StartCoroutine(TurnDamageOff(1));
            TakeDmgSound.Play();
            // Disable all Renderers and Colliders
            col.gameObject.SetActive(false);
            if(health <= 0)
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
            animator.SetBool("takeDamage", true);
            StartCoroutine(TurnDamageOff(1));
            TakeDmgSound.Play();
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
        state = EnemyState.DEAD;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(2.0f);
        state = EnemyState.DEFAULT;
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
        float i = 0;
        while (fireDamage && health > 0 && i <= 7.0f)
        {
            yield return new WaitForSeconds(0.5f);

            TakeDamage(2);
            healthBar.UpdateHealthBar(health, maxHp);
            i += 1.0f;
            if (i == 7.0f)
            {
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
        state = EnemyState.DEAD;
        animator.SetBool("dead", true);
        Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in allColliders) c.enabled = false;
        StartCoroutine(spawnKey());
        StartCoroutine(PlayAndDestroy(4.67f));
    }

    private IEnumerator TurnDamageOff(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("takeDamage", false);
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        //myaudio.Play();
        //SpawnRandomFood(transform.position);
        yield return new WaitForSeconds(waitTime);
        deathEffect.Play();
        DeathSound.Play();
        yield return new WaitForSeconds(0.4f);
        Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer c in allRenderers) c.enabled = false;
        //StopBloodSplatter();
        Destroy(gameObject);
    }

    private IEnumerator spawnKey() {
        DungeonCreator.bugCount--;
        //need 3 keys to spawn when 1/2, 1/3 , and all enemies are killed
        if (EnemyAI.maxKeys < 3)
        {
            if (DungeonCreator.bugCount <= 0)
            {
                Instantiate(key, transform.position + new Vector3(0f, 0.8f, 0.0f), Quaternion.Euler(270f, 0f, 0f));
                EnemyAI.maxKeys++;
            }
            else if (EnemyAI.maxKeys < 2 && DungeonCreator.bugCount <= MaxCount / 3)
            {
                Instantiate(key, transform.position + new Vector3(0f, 0.8f, 0.0f), Quaternion.Euler(270f, 0f, 0f));
                EnemyAI.maxKeys++;
            }
            else if (EnemyAI.maxKeys < 2 && DungeonCreator.bugCount <= MaxCount / 2)
            {
                Instantiate(key, transform.position + new Vector3(0f, 0.8f, 0.0f), Quaternion.Euler(270f, 0f, 0f));
                EnemyAI.maxKeys++;
            }
        }

        //Debug.Log(maxKeys);
        yield return null;
    }

    bool IsStuck()
    {
        // Check if the agent's position hasn't changed significantly
        return (Vector3.Distance(transform.position, lastPosition) < 0.1f && state == EnemyState.MOVING);
    }
}