using System.Collections;
using UnityEngine;
using UnityEngine.AI;
/*
 * EnemyController is player's attack collision
 * This can receive an attack signal from the player
 * 
 * This manages on hit animation
 */

public class EnemyController : MonoBehaviour
{
    // State
    private enum State { idle, trace1, trace2, attack, hit, dead };
    private State state = State.idle;

    // Component
    [SerializeField]
    private Transform target;
    private Vector3 targetLastPosition;
    [SerializeField]
    private GameObject attackCollision;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private FieldOfView fieldOfView;
    
    // Movement Control
    [SerializeField]
    private float traceDistance = 15.0f;
    [SerializeField]
    private float attackDistance = 1.0f;
    private bool isDead = false;
    private bool isTrack = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();

        StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    private IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float distance = Vector3.Distance(transform.position, target.position);

            //Debug.Log(distance + " " + state);
            if (distance <= attackDistance) // 1.0f
            {
                state = State.attack;
            }
            else if (distance <= traceDistance || isTrack == true) // 15.0f
            {
                if (fieldOfView.canSeePlayer == true)   state = State.trace1;
                else if(isTrack == true)                state = State.trace2;
            }
            else
            {
                state = State.idle;
            }
        }
    }

    private IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.idle:
                    navMeshAgent.isStopped = true;
                    animator.SetBool("isTrace", false);
                    break;

                case State.trace1:
                    navMeshAgent.isStopped = false;
                    targetLastPosition = target.transform.position;
                    navMeshAgent.destination = targetLastPosition;
                    animator.SetBool("isTrace", true);
                    isTrack = true;
                    break;

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

                case State.attack:
                    navMeshAgent.isStopped = true;
                    OnAttackCollision();
                    break;
            }

            yield return null;
        }
    }

    public void OnHit(int damage)
    {
        Debug.Log("Zombie takes " + damage + " Damage!!");

        // Animation
        animator.SetTrigger("onHit");
        navMeshAgent.isStopped = true;
    }

    public void OnAttackCollision()
    {
        animator.SetTrigger("onAttack");
        attackCollision.SetActive(true);
        navMeshAgent.isStopped = true;
    }
}
