using System.Collections;
using UnityEngine;

/*
 * PlayerAttackCollision is player's attack collision
 * This can give an attack signal to the enemy
 */

public class EnemyAttackCollision : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnHit(10);
        }
    }

    private IEnumerator AutoDisable()
    {
        // after 0.1s -> disappear object
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

}
