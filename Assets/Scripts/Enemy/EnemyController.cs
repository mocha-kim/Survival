using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

/*
 * EnemyController is player's attack collision
 * This can receive an attack signal from the player
 * 
 * This manages on hit animation
 */


public class EnemyController : MonoBehaviour
{
    // State
    private EnemyState state;

    // Component
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject attackCollision;
    private EnemyAttackCollision enemyAttackCollision;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private FieldOfView fieldOfView;
    private StatsObject playerStat;

    // Enemy Database
    private Enemy info;
    [SerializeField]
    private int enemyID;

    // Movement Control
    private Vector3 targetLastPosition;
    private Vector3 randomPosition;
    private float targetDistance;
    private float patrolDistance;
    private float traceDistance = 15.0f;
    private float attackDistance = 1.0f;
    private float patrolMinDistance = 4.0f;
    private float patrolMaxDistance = 11.0f;

    // System Setting
    private float currentHP;
    private bool isDead = false;
    private bool isPatrol = false;
    private bool isTrace = false;
    private bool isIdle = false;
    private bool isAttack = false;
    private bool isHit = false;
    private bool isStun = false;
    private bool isReact = false;
    private int hitCount = 0;
    private int reactCount = 0;
    private float idleDelay = 5f;
    private float attackDelay = 1.755f;
    private float hitDelay = 2f;
    private float reactDelay = 3f;

    [SerializeField]
    private AudioClip hitClip;
    private AudioSource audioSource;

    public bool IsDead => isDead;
    public bool CanSeePlayer => fieldOfView.canSeePlayer;
    public float GetDistance => Vector3.Distance(transform.position, target.transform.position);

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
        enemyAttackCollision = attackCollision.GetComponent<EnemyAttackCollision>();
        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        playerStat = GameManager.Instance.playerStats;
        info = GameManager.Instance.GetEnemyData(enemyID);
        currentHP = info.maxHP;
        enemyAttackCollision.SetEnemyInfo(info.id);
        randomPosition = transform.position;

        StartCoroutine(CheckState());
    }

    private IEnumerator CheckState()
    {
        while (!isDead && !playerStat.IsDead)
        {
            targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (isStun) // Hit
            {
                if (isHit) // On Hit
                {
                    isHit = false;
                    isReact = true;
                    state = EnemyState.Hit;
                    CheckStateForAction();
                    StartCoroutine(HitDelay(hitDelay));
                }
            }

            else if (!CanSeePlayer && !isAttack) // Idle & Patrol
            {
                patrolDistance = Vector3.Distance(transform.position, randomPosition);

                if (isReact) // Trace after (Attack, Hit, CanSeePlayer)
                {
                    if (isTrace)
                    {
                        isPatrol = false;
                        isTrace = false;
                        reactCount++;
                        StartCoroutine(ReactDelay(reactDelay));
                    }
                    state = EnemyState.Trace;
                    CheckStateForAction();
                }

                else if (isPatrol && patrolDistance <= 1f) // Idle
                {
                    isIdle = true;
                    isPatrol = false;
                    state = EnemyState.Idle;
                    StartCoroutine(IdleDelay(idleDelay));
                    CheckStateForAction();
                }

                else if (!isPatrol && !isIdle) // Patrol
                {
                    isPatrol = true;
                    state = EnemyState.Patrol;
                    randomPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    randomPosition *= Random.Range(patrolMinDistance, patrolMaxDistance);
                    randomPosition += transform.position;
                    CheckStateForAction();
                }
            }

            else if(CanSeePlayer && !isAttack)
            {
                if (targetDistance <= attackDistance) // Attack
                {
                    isAttack = true;
                    isReact = true;
                    isTrace = true;
                    isPatrol = false;
                    state = EnemyState.Attack;
                    StartCoroutine(AttackDelay(attackDelay));
                    CheckStateForAction();
                }

                else if (targetDistance <= traceDistance) // Trace
                {
                    isReact = true;
                    isTrace = true;
                    isPatrol = false;
                    state = EnemyState.Trace;
                    CheckStateForAction();
                }
            }

            if (currentHP <= 0)
            {
                state = EnemyState.Dead;
                isDead = true;
                CheckStateForAction();
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0);
    }

    private void CheckStateForAction()
    {
        switch (state)
        {
            case EnemyState.Idle:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                break;

            case EnemyState.Patrol:
                navMeshAgent.isStopped = false;
                targetLastPosition = randomPosition;
                navMeshAgent.destination = targetLastPosition;
                animator.SetBool("isTrace", true);
                //isTrack = true;
                break;

            case EnemyState.Trace:
                navMeshAgent.isStopped = false;
                targetLastPosition = target.transform.position;
                navMeshAgent.destination = targetLastPosition;
                animator.SetBool("isTrace", true);
                //isTrack = true;
                break;

            case EnemyState.Attack:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                animator.SetTrigger("onAttack");
                break;

            case EnemyState.Hit:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                animator.SetTrigger("onHit");
                break;

            case EnemyState.Dead:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                animator.SetTrigger("onDying");
                ItemPocketGenerator.Instance.GenItemPocket(transform.position, info);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                break;
        }
    }

    private IEnumerator IdleDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isIdle = false;
    }

    private IEnumerator AttackDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isAttack = false;
    }

    private IEnumerator ReactDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (reactCount > 0)
        {
            reactCount--;
        }
        if (reactCount == 0)
        {
            isReact = false;
        }
    }

    private IEnumerator HitDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (hitCount > 0)
        {
            hitCount--;
        }
        if (hitCount == 0)
        {
            isStun = false;
        }
    }

    public void OnHit(int damage)
    {
        if (!isDead)
        {
            audioSource.volume = 0.75f;
            audioSource.PlayOneShot(hitClip);
            isHit = true;
            isStun = true;
            isTrace = true;
            hitCount++;
            currentHP -= damage;
        }
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
}
