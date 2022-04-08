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
    private int enemyID;

    // Movement Control
    private float distance;
    [SerializeField]
    private float traceDistance = 15.0f;
    [SerializeField]
    private float attackDistance = 1.0f;

    // System Setting
    private bool isDead = false;
    //private bool isTrack = false;
    private bool isDelay = false;
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
        database.datas[enemyID].currentHP = database.datas[enemyID].maxHP;

        StartCoroutine(CheckState());
    }

    private IEnumerator CheckState()
    {
        while (!isDead && !playerStat.IsDead)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= attackDistance && !isDelay) // Attack
            {
                isDelay = true;
                state = State.Attack;
                CheckStateForAction();
                StartCoroutine(Delay(attackDelay));
            }
            //else if (distance <= traceDistance || isTrack == true) // using trace2
            else if (distance <= traceDistance && !isDelay) // 15.0f
            {
                if (fieldOfView.canSeePlayer == true)
                {
                    state = State.Trace1;
                    CheckStateForAction();
                }
                // else if(isTrack == true)                state = State.trace2;
            }

            if (distance > traceDistance || playerStat.IsDead)
            {
                state = State.Idle;
                CheckStateForAction();
            }

            if (database.datas[enemyID].maxHP <= 0)
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
                playerStat.AddAttributeExp(AttributeType.CON, database.datas[enemyID].conExp);
                playerStat.AddAttributeExp(AttributeType.STR, database.datas[enemyID].strExp);
                playerStat.AddAttributeExp(AttributeType.DEF, database.datas[enemyID].defExp);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
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
            navMeshAgent.isStopped = true;
            animator.SetBool("isTrace", false);
            database.datas[enemyID].currentHP -= damage;

            animator.SetTrigger("onHit");
            navMeshAgent.isStopped = true;
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
