using System.Collections;
using UnityEngine;
using UnityEngine.AI;
/*
 * EnemyController is player's attack collision
 * This can receive an attack signal from the player
 * 
 * This manages on hit animation
 */

public enum State
{
    Idle,
    Patrol,
    Trace1,
    Trace2,
    Attack,
    Hit,
    Dead,
};

public class EnemyController : MonoBehaviour
{
    // State
    private State state = State.Idle;

    // Component
    [SerializeField]
    private GameObject target;
    private Vector3 targetLastPosition;
    private Vector3 randomPosition;
    [SerializeField]
    private GameObject attackCollision;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private FieldOfView fieldOfView;
    private StatsObject playerStat;

    // Enemy Database
    [SerializeField]
    private EnemyDatabase database;
    [SerializeField]
    private GameObject pocketObject;
    [SerializeField]
    private ItemPocket itemPocket;
    [SerializeField]
    private int enemyID;

    // Movement Control
    private float targetDistance;
    private float patrolDistance;
    [SerializeField]
    private float traceDistance = 15.0f;
    [SerializeField]
    private float attackDistance = 1.0f;
    [SerializeField]
    private float patrolMinDistance = 4.0f;
    [SerializeField]
    private float patrolMaxDistance = 6.0f;

    // System Setting
    private bool isDead = false;
    private bool isPatrol = false;
    //private bool isTrack = false;
    private bool isDelay = false;
    private float patrolDelay = 5f;
    private float attackDelay = 1.755f;
    private float onHitDelay = 2f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
    }

    private void Start()
    {
        playerStat = GameManager.Instance.playerStats;
        database.data[enemyID].currentHP = database.data[enemyID].maxHP;
        itemPocket.GenEnemyItem(enemyID);

        StartCoroutine(CheckState());
    }

    private IEnumerator CheckState()
    {
        while (!isDead && !playerStat.IsDead)
        {
            targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (!fieldOfView.canSeePlayer) // Idle & Patrol
            {
                if (!isPatrol && !isDelay) // Patrol
                {
                    randomPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    randomPosition *= Random.Range(patrolMinDistance, patrolMaxDistance);
                    isPatrol = true;
                    state = State.Patrol;
                }

                patrolDistance = Vector3.Distance(transform.position, randomPosition);
                if (patrolDistance <= 1f) // Idle
                {
                    isDelay = true;
                    isPatrol = false;
                    state = State.Idle;
                    StartCoroutine(Delay(patrolDelay));
                }
                    CheckStateForAction();
            }
            else if (targetDistance <= attackDistance) // Attack
            {
                if (isPatrol || state == State.Idle)
                {
                    isPatrol = false;
                    isDelay = false;
                }
                if (!isDelay)
                {
                    isDelay = true;
                    state = State.Attack;
                    CheckStateForAction();
                    StartCoroutine(Delay(attackDelay));
                }
            }
            //else if (distance <= traceDistance || isTrack == true) // using trace2
            else if (targetDistance <= traceDistance) // 15.0f
            {
                if (isPatrol || state == State.Idle)
                {
                    isPatrol = false;
                    isDelay = false;
                }
                if (fieldOfView.canSeePlayer && !isDelay)
                {
                    state = State.Trace1;
                    CheckStateForAction();
                }
                // else if(isTrack == true)                state = State.trace2;
            }

            if (database.data[enemyID].currentHP <= 0)
            {
                state = State.Dead;
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
            case State.Idle:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                break;

            case State.Patrol:
                navMeshAgent.isStopped = false;
                targetLastPosition = randomPosition;
                navMeshAgent.destination = targetLastPosition;
                animator.SetBool("isTrace", true);
                //isTrack = true;
                break;

            case State.Trace1:
                navMeshAgent.isStopped = false;
                targetLastPosition = target.transform.position;
                navMeshAgent.destination = targetLastPosition;
                animator.SetBool("isTrace", true);
                //isTrack = true;
                break;

                /*
            case State.trace2:
                navMeshAgent.isStopped = false;
                navMeshAgent.destination = targetLastPosition;
                animator.SetBool("isTrace", true);
                if (navMeshAgent.remainingDistance < 2.0f)
                {
                    animator.SetBool("isTrace", false);
                    isTrack = false;
                }
                break;
                */

            case State.Attack:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                animator.SetTrigger("onAttack");
                break;

            case State.Dead:
                navMeshAgent.isStopped = true;
                animator.SetBool("isTrace", false);
                animator.SetTrigger("onDying");
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                GameObject obj = Instantiate(pocketObject);
                obj.name = "ItemPocket";
                Vector3 pos = transform.position + new Vector3(0, 0.5f, 0);
                obj.transform.position = pos;
                break;
        }
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        isDelay = false;
    }

    public void OnHit(int damage)
    {
        if (!isDead)
        {
            database.data[enemyID].currentHP -= damage;

            navMeshAgent.isStopped = true;
            animator.SetBool("isTrace", false);
            animator.SetTrigger("onHit");
            
            isDelay = true;
            StartCoroutine(Delay(onHitDelay));
        }
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    public void OnStatChanged(StatsObject stats)
    {
        // player stats 
        // 죽었을 때
        // 체력이 변하면 OnStatChanged
        // Update 대신 이걸 써라!!!
    }
}
